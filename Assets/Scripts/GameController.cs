using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using DG.Tweening;



public class GameController : MonoBehaviour
{   
    

    public int userScore = 0;
    public int dealerScore = 0;
    bool enemyTurn = false;
    int aceCountPlayer= 0;              // ace counting to track the ace conditions
    int aceCountDealer = 0;             // ace counting to track the ace conditions
    public Canvas canvas;
    private Dealer dealer;              // dealer for dealer things
    private int currentBossIndex = 0;   //current boss's index
    private List<Dealer> bosses;        //list of the dealers who are represented as bosses
    private Player player;              // player for player things
    private DeckOfCards deckOfCards;
    private Card shownCard;             //shownCard of a Dealer. Is used in the GamblerBehavior
    private GameObject shownCardGO;     //shownCard GO of a Dealer. Is used in the GamblerBehavior
    private Card hiddenCard;            //needs for a method ResetImage
    private GameObject hiddenCardGO;    //needed for a method ResetImage
    [SerializeField] private Revolver revolver;
    [SerializeField] public UIManager uiManager;
    [SerializeField] private Transform playerCardsPanel; // UI panel for player cards
    [SerializeField] private Transform dealerCardsPanel; // UI panel for dealer cards
    [SerializeField] private Slider Bet;                // Slider for placing a bet
    // [SerializeField] Transform playerHandPanel;
    // [SerializeField] Transform dealerHandPanel;
    [SerializeField] Vector2 deckPosition;
    [SerializeField] GameObject cardPrefab;
    private List<Card> playerCards;
    private List<Card> dealerCards;



    void Start()
    { 
        InitializeBosses();     // dealer initialization
        StartNextBoss();        // setting the first delaer by this method
        // dealer = new Dealer();
        player = new Player();
        dealer.Setup(50);
        player.Setup(50);
        deckOfCards = new DeckOfCards(1);   
        playerCards = new List<Card>(); //list of player cards
        dealerCards = new List<Card>(); //list of dealer cards
        // revolver = new Revolver();
        DealInitialCards();
        CheckStateBeforeStand();
    }

    private void StartNextBoss()
    {
        if(currentBossIndex < bosses.Count)
        {
            dealer = bosses[currentBossIndex];
            currentBossIndex++;
            Debug.Log($"You are now facing boss {currentBossIndex}.");
        }else
        {
            Debug.Log("All bosses defeated!");
        }
    }

    private void InitializeBosses()
    {
        bosses = new List<Dealer>
        {
            new Dealer(new GamblerBehavior()),
            new Dealer(new RookieBehavior()),
            new Dealer(new SatanBehavior())
            
        };
    }
    void CleanEverything()
    {
        playerCards.Clear();
        dealerCards.Clear();
    }

    private void DealInitialCards()
    {
        CleanEverything();
        DrawCard(true);     //one for the player
        DrawCard(true);     //two for the player
        DrawCard(false);    // one for the dealer
        DrawCard(false,true);    // two for the dealer,hidden
    }

    public void DrawCard(bool isPlayer)
    {
        Card drawnCard = deckOfCards.DrawCard();
        if(isPlayer){
            playerCards.Add(drawnCard);
        }else dealerCards.Add(drawnCard);

        AnimateCardDraw(drawnCard,isPlayer);
    }
    public void DrawCard(bool isPlayer, bool isHidden)
    {
        Card drawnCard = deckOfCards.DrawCard();
        if(isPlayer){
            playerCards.Add(drawnCard);
        }else dealerCards.Add(drawnCard);
        drawnCard.HideCard();
        AnimateCardDraw(drawnCard,isPlayer);
        }

    private void AnimateCardDraw(Card card, bool isPlayer)
    {
        Transform targetPanel = isPlayer ? playerCardsPanel : dealerCardsPanel; //Get the correct panel
        List<Card> handList = isPlayer ? playerCards : dealerCards; // Get the correct list

        // 1️⃣ Get the index to calculate spacing
        int cardIndex = handList.Count - 1;
        float cardSpacing = 60f; // Adjust spacing between cards
        Vector3 offsetPosition = targetPanel.position + new Vector3(cardIndex * cardSpacing, 0, 0);


        // Instatiating GameObjects to work with their Images
        GameObject cardGO = Instantiate(cardPrefab,deckPosition,Quaternion.identity,targetPanel);
        Image cardImage = cardGO.gameObject.GetComponent<Image>();
        cardImage.sprite = card.cardBack;

        //Storing the needed info of the hidden card UI
        if (!isPlayer && handList.Count == 2)
        {
        hiddenCardGO = cardGO; // Store reference to the hidden card UI
        hiddenCard = card; // Store the actual card object
        }

        // 5️⃣ Animate the card moving & scaling in
        Sequence cardSequence = DOTween.Sequence();
        cardSequence.Append(cardGO.transform.DOScale(1.2f, 0.4f)) // Slight scale-up
                .Join(cardGO.transform.DOMove(offsetPosition, 0.6f).SetEase(Ease.OutQuad)) // Move with offset
                .Join(cardGO.transform.DORotate(Vector3.forward * UnityEngine.Random.Range(-10, 10), 0.5f)) // Random tilt
                .Append(cardGO.transform.DOScale(1f, 0.2f)) // Normalize scale
                .AppendInterval(0.2f) // Wait before flipping
                .AppendCallback(() => FlipCard(cardGO, card, isPlayer)); // Flip to show face-up
    }
    private void FlipCard(GameObject cardGO, Card card, bool isPlayer)
{
    Image cardImage = cardGO.GetComponent<Image>();
    if (cardImage != null)
    {
        Sequence flipSequence = DOTween.Sequence();
        flipSequence.Append(cardGO.transform.DORotate(new Vector3(0, 90, 0), 0.15f)) // Rotate halfway
                    .AppendCallback(() => cardImage.sprite = card.cardImage) // Change to face-up
                    .Append(cardGO.transform.DORotate(Vector3.zero, 0.15f)); // Rotate back to normal
    }
}

    // private void DealInitialCards()                 //dealing initial two cards for the player and for the dealer
    // {
    //     IsDeckEmpty();
    //     uiManager.HitStandActivity(true);
    //     Card playerCard1 = deckOfCards.DrawCard();
    //     Card playerCard2 = deckOfCards.DrawCard();
    //     Card enemyCard1 = deckOfCards.DrawCard();
    //     Card enemyCard2 = deckOfCards.DrawCard();
    // //
    //     CountInitialAces(playerCard1,false);
    //     CountInitialAces(playerCard2,false);
    //     CountInitialAces(enemyCard1,true);
    //     CountInitialAces(enemyCard2,true);
    // //
    //     enemyCard2.HideCard();
    //     hiddenCard = enemyCard2;
    //     // Display cards on UI
    //     DisplayCard(playerCard1);
    //     DisplayCard(playerCard2);
    //     DisplayEnemyCard(enemyCard1);
    //     DisplayEnemyCard(enemyCard2);

    // //
    //     shownCard = enemyCard1;//for 2nd boss(Gambler)
        

    // }

    // //////////// start of ace logic
    private void CountInitialAces(Card card, bool isEnemy)        //counts aces for further handling logic
    {
        if(card.rank =="Ace")
        {
            int aceCount = isEnemy ? ++aceCountDealer: ++aceCountPlayer;
            Debug.Log($"Ace count for {(isEnemy ? "dealer" : "player")} is {aceCount}");
        }
        
    }

    private void AceRecalculationFinal(bool isEnemy)         //handles initialy dealt ace value's logic (1 or 11)
    {
        
        if(!isEnemy && aceCountPlayer!=0  && userScore>21 )
        {
            userScore -=10;
            aceCountPlayer--;
            Debug.Log("after -- ace count for the player is " + aceCountPlayer);
        }else if(isEnemy && aceCountDealer!=0 && dealerScore>21 )
        {
            dealerScore-=10;
            aceCountDealer--;
            Debug.Log("after -- ace count for the dealer is " + aceCountDealer);
        }
    }
    private void HandleDrawnAce(Card card,bool isEnemy)     // Ace check for cards that are being drawn, and value handling
    {
        if(card.rank=="Ace"){
            int score = isEnemy? dealerScore:userScore;
            if (score + card.value>21) {
                if(isEnemy) dealerScore -= 10;
                else userScore -= 10;
            } else {
                if(isEnemy) aceCountDealer++;
                else aceCountPlayer++;
                Debug.Log($"Ace count for {(isEnemy ? "dealer" : "player")} is {(isEnemy ? aceCountDealer : aceCountPlayer)}");
            }
        }
    }
    // private void DrawnAceHandlingPlayer(Card card)          // (player)
    // {
    //     if(card.rank=="Ace" && userScore + card.value >21)
    //     {
    //         userScore=userScore-10;
    //     }else if(card.rank=="Ace" && userScore + card.value <=21)
    //     {
    //         aceCountPlayer++;
    //         Debug.Log("ace count for the player is " + aceCountPlayer);
    //     }
    // }

    // private void DrawnAceHandlingDealer(Card card)          // Ace check for cards that are being drawn, and value handling(dealer)
    // {
    //     if(card.rank=="Ace" && dealerScore + card.value >21)
    //     {
    //         dealerScore=dealerScore-10;
    //     }else if(card.rank=="Ace" && dealerScore + card.value <=21)
    //     {
    //         aceCountDealer++;
    //          Debug.Log("ace count for the dealer is " + aceCountDealer);
    //     }
    // }

    // //////////// end of ace logic

    // private void DisplayCard(Card card)
    // {
    //     CreateAndPositionCard(card, playerCardsPanel, true);
    //     UpdateScore(card.value);
    // }

    // public void DisplayEnemyCard(Card card)
    // {
    //     CreateAndPositionCard(card, dealerCardsPanel, false);
    //     CalculateValue(card.value);
    // }

    // private void CreateAndPositionCard(Card card, Transform parentPanel, bool isPlayer)
    // {
    //     string cardName = isPlayer? "PlayerCard" : "DealelCard";
    //     GameObject cardGameObject = new GameObject(cardName);
    //     if (card == hiddenCard)
    //     {
    //         hiddenCardGO = cardGameObject;
    //     }
    //     cardGameObject.transform.SetParent(parentPanel, false);

    //     Image image = cardGameObject.AddComponent<Image>();
    //     image.sprite = card.cardImage;

    //     RectTransform rect = cardGameObject.GetComponent<RectTransform>();
    //     rect.sizeDelta = new Vector2(128*2, 196*2); // Card size
    //     // rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f); // Center anchor

    //     AdjustCardPosition(parentPanel, rect, isPlayer);
    // }

    // private void AdjustCardPosition(Transform parentPanel, RectTransform rect, bool isPlayer)
    // {
    //     int cardCount = parentPanel.childCount - 1; // Existing children count before adding new card
    //     float offset = cardCount * 140; // Horizontal offset; adjust as necessary
    //     rect.anchoredPosition = new Vector2(offset, 0); // Set position relative to the parent panel
    //     Debug.Log($"Adjusting card position: Card Count = {parentPanel.childCount}, Offset = {offset}");
    //     // offset = 0f;
    // }


    public void DrawCardForBehavior() //sxal metod besamp, bayc hly or edpes. stexcvace or behaviorneri het ashxati
    {
        Card cardDrawn = deckOfCards.DrawCard();
        HandleDrawnAce(cardDrawn,true); 
        // DisplayEnemyCard(cardDrawn);
    }
    public void DrawCardPlayer()                        //this one is for the player's card drawing
    {
        Card playerCardDrawn = deckOfCards.DrawCard();
        HandleDrawnAce(playerCardDrawn,false);                  //if drawn card is an Ace and userscore >21 then it becomes 1
        // DisplayCard(playerCardDrawn);
    }


    public void DealersTurn()                 //if <17 the draw card for dealer, for the stand button OnClick()
    {
        if(enemyTurn==true)
        {   
            dealer.TakeTurn(this);
        }

    }



    public void UpdateScore(int value)
    {

        userScore = userScore + value;
        AceRecalculationFinal(false);
        uiManager.UpdateScore(userScore);
        // CheckState();
    }


    //ENEMY AI STARTS HERE

    private void CalculateValue(int value)  //calculates enemy's score
    {
        // Debug.Log(card.rank);
        // Debug.Log(card.suit);
        // Debug.Log(card.value);
        dealerScore = dealerScore + value;
        AceRecalculationFinal(true);
        Debug.Log("dealer's score is" + dealerScore);
    }


    private void ExposeHiddenCard(Card card, GameObject cardGO)
    {
        card.ExposeCard();
        FlipCard(hiddenCardGO,hiddenCard,false);
    }
    public void PlayerStand()
    {
        enemyTurn = true;
        // ExposeHiddenCard(hiddenCard, hiddenCardGO);   //resets image to display the hidden card
        ExposeHiddenCard(hiddenCard,hiddenCardGO);
        uiManager.HitStandActivity(false);
    }

    public void CheckState()
    {

        if((userScore >dealerScore && userScore <=21) || dealerScore>21)
        {
            uiManager.ShowMessage("win");
            PlayerWon();    
        }
        else if((userScore<dealerScore && dealerScore<=21 )|| userScore  >21)
        {
            uiManager.ShowMessage("lose");
            PlayerLost();
        }
        else if(userScore==dealerScore)
        {
            uiManager.ShowMessage("draw");
        }else if(userScore==21 && dealerScore!= 21)
        {
            uiManager.ShowMessage("blackjack");
        }
            uiManager.UpdateDealersHealth(dealer.Showhp());
            uiManager.UpdatePlayersHealth(player.Showhp());
            CheckGameEnd();
        
    }

    public void CheckStateBeforeStand()
    {

        if(userScore == 21)
        {
            uiManager.ShowMessage("blackjack");
        }
        else if (userScore >21)
        {
            uiManager.ShowMessage("lose");
        }
    }

    public void PlayerWon()
    {
        player.AdjustHP((int)Bet.value);
        dealer.AdjustHP(-(int)Bet.value);
    }
    public void PlayerLost()
    {
        player.AdjustHP(-(int)Bet.value);
        dealer.AdjustHP((int)Bet.value);
    }


public void CheckGameEnd()
{
    if(player.Showhp() <= 0 || dealer.Showhp() <= 0)
    {
        // End the game and maybe show some end game UI here
        Debug.Log("Game Over! Resetting game...");
        ResetGame(true);
        StartNextBoss();
    }
    else
    {
        // If no one is below 0 HP, restart the dealing process
        ResetGame(false);
    }
}

public void ResetGame(bool isGameEnded)
{
    if(!isGameEnded)
    {
        StartCoroutine(ClearCardsAndResetGame());
    }else
        StartCoroutine(ClearCardsAndResetGame());
    // uiManager.gameOverToggle(true);


}

public IEnumerator ClearCardsAndResetGame()
{
    yield return new WaitForSeconds(3);
    foreach (Transform child in playerCardsPanel) {
        Destroy(child.gameObject);
    }
    foreach (Transform child in dealerCardsPanel) {
        Destroy(child.gameObject);
    }
    StartCoroutine(ResetGameNextFrame());

}

public IEnumerator ResetGameNextFrame()
{
    yield return null;
    // Reset game state
    userScore = 0;
    dealerScore = 0;
    aceCountPlayer = 0;  // Assuming you have these counters for aces
    aceCountDealer = 0;

    // Reset UI elements
    uiManager.UpdateScore(0);  // Assuming you have a method to reset the score display
    uiManager.HideAllMessages();


    // Restart the dealing process
    DealInitialCards();
}


public void IsDeckEmpty()
{
    bool result = deckOfCards.IsDeckEmpty();
    Debug.Log(result);
}


public void CheckOnDealer()     //checking if a shot hit a dealer or not
{
    if(revolver.CheckHit()){
        dealer.Die();
        uiManager.UpdateDealersHealth(dealer.Showhp());
    }
}


    // public void DrawAce()                                //debug purposes
    // {
    //     Card playerCardDrawn =deckOfCards.DrawAce();
    //     HandleDrawnAce(playerCardDrawn,false);                   //if drawn card is an Ace and userscore >21 then it becomes 1
    //     DisplayCard(playerCardDrawn);
    // }

public Card JustDrawACard() //method for drawing a Card from the deck, used in the GamblerBehavior
{
    return deckOfCards.DrawCard();
}

public Card GetShownCard() //method for getting a shown card of a dealer at initial hand, used in the GamblerBehavior
{
    return shownCard;
}
public void SetShownCard(string suit, string rank, int value,Sprite cardImage)
{
    shownCard.SetCard(suit,rank,value,cardImage);
}

public Transform GetShownCardGO()
{
    return dealerCardsPanel.GetChild(0);    ////method for getting a shown card GameObject of a dealer at initial hand, used in the GamblerBehavior
}
public void RecalculateScore(Card original, Card replaced)  //method for recalculation of the dealer's score
{
        if(original.rank == "Ace" && replaced.rank == "Ace")
        {
            return;
        }else if(original.rank == "Ace" && replaced.rank != "Ace")
        {
            aceCountDealer--;
            dealerScore = dealerScore - original.value + replaced.value;
            AceRecalculationFinal(true);
        }else if(original.rank != "Ace" && replaced.rank == "Ace")
        {
            aceCountDealer++;
            dealerScore = dealerScore - original.value + replaced.value;
            AceRecalculationFinal(true);
        }else dealerScore = dealerScore - original.value + replaced.value;
        
        
        Debug.Log("dealer's score is" + dealerScore);
}
public List<Card> RemainingDeck()   //getting remaining cards of the deck
{
    return deckOfCards.GetRemainingCards();
}
public List<Card> GetFullDeck()     //getting cars of the full deck
{
    return deckOfCards.GetFullDeck();
}
public Card getHiddenCard()
{
    return hiddenCard;
}
}
