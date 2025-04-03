using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using DG.Tweening;



public class GameController : MonoBehaviour
{   
    
    public int playerScore = 0;
    public int dealerScore = 0;
    int currentAngerValue=0;        // anger CHANGE value of the boss, 20 for the first boss
    int currentChillValue =0;       // chill CHANGE value of the boss 
    int quantityOfCards = 0;        // number of cards which will be dealt during the game
    int points = 0;                 // points for the shop. angerpoints
    bool enemyTurn = false;
    public Canvas canvas;
    private Dealer dealer;              // dealer for dealer things
    private int currentBossIndex = 0;   //current boss's index
    private List<Dealer> bosses;        //list of the dealers who are represented as bosses
    private Player player;              // player for player things
    private DeckOfCards deckOfCards;
    private DeckOfCards reserverDeckOfCards;
    private Card hiddenCard;            //is being used in AnimateCardDraw
    private GameObject hiddenCardGO;    //is being used in AnimateCardDraw
    [SerializeField] private Die die;
    [SerializeField] private RemainingDeckPanel remainingDeckPanel;
    [SerializeField] private Revolver revolver;
    [SerializeField] private BossRevolver bossRevolver;
    [SerializeField] public UIManager uiManager;
    [SerializeField] private Transform playerCardsPanel; // UI panel for player cards
    [SerializeField] private Transform dealerCardsPanel; // UI panel for dealer cards
    [SerializeField] private Slider Bet;                // Slider for placing a bet
    [SerializeField] Vector2 deckPosition;              // position of the deck
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Slider anger;                      //anger bar
    [SerializeField] Image bossImage;
    [SerializeField] Sprite[,] bossSprites = new Sprite[6, 5]; // 6 bosses, 5 sprites each
    private List<Card> playerCards;
    private List<Card> dealerCards;
    private Tweener faceShakeTween; // Store reference to the shake tween


    private void LoadBossSprites()
    {
        // Manually assign sprites for each boss and anger level
        bossSprites[0, 0] = Resources.Load<Sprite>("Faces/Rookie/rookieDealer");
        bossSprites[0, 1] = Resources.Load<Sprite>("Faces/Rookie/rookieDealer2");
        bossSprites[0, 2] = Resources.Load<Sprite>("Faces/Rookie/rookieDealer3");
        bossSprites[0, 3] = Resources.Load<Sprite>("Faces/Rookie/rookieDealer4");
        bossSprites[0, 4] = Resources.Load<Sprite>("Faces/Rookie/rookieDealer5");
    }
    void Start()
    { 
        InitializeBosses();     // dealer initialization
        StartNextBoss();        // setting the first delaer by this method
        LoadBossSprites();      // loads the sprites of the bosses
        // dealer = new Dealer();
        player = new Player();
        dealer.Setup(50);
        player.Setup(50);
        deckOfCards = new DeckOfCards(1);   
        playerCards = new List<Card>(); //list of player cards
        dealerCards = new List<Card>(); //list of dealer cards
        // revolver = new Revolver();
        bossRevolver.LoadGun(); //loading boss's gun. randomly in one chamber
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
       
            new Dealer(new RookieBehavior(this)),
            new Dealer(new GamblerBehavior()),
            new Dealer(new SatanBehavior())
            
        };
    }
    private void CleanEverything()
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
        CheckBlackjack();       //check for a blackjack in an initial hand
        CheckAnger();           //checks the level of the boss anger
    }

    public void UpdateScore(bool isPlayer)      //now UpdateScore is used for both the player and the dealer, Ace Handling logic is inside
    {   
        int newScore = 0;
        List<Card> targetList = isPlayer ? playerCards : dealerCards;   //selecting right cards list
        int aceCount =0;

        foreach(Card c in targetList)
        {
            newScore += c.value;
            if(c.rank == "Ace")
            {
                aceCount++;
            }
        }
        while(newScore >21 && aceCount !=0) //is being used for ace handling
        {
            newScore -=10;
            aceCount--;
        }

        if(isPlayer)
        {
            playerScore = newScore;
            uiManager.UpdateScore(newScore);
        }else 
        {
            dealerScore = newScore;
        }
        Debug.Log($"Score for the {(isPlayer ?  "player" : "dealer" )} is {newScore}");
        aceCount =0;
    }

    public void DrawCard(bool isPlayer)
    {
        Card drawnCard = deckOfCards.DrawCard();
        if(isPlayer){
            playerCards.Add(drawnCard);
        }else dealerCards.Add(drawnCard);

        AnimateCardDraw(drawnCard,isPlayer);
        UpdateScore(isPlayer);
        UpdateRemainingCards();
    }
    public void DrawCard(bool isPlayer, bool isHidden)
    {
        Card drawnCard = deckOfCards.DrawCard();
        if(isPlayer){
            playerCards.Add(drawnCard);
        }else dealerCards.Add(drawnCard);
        drawnCard.HideCard();
        AnimateCardDraw(drawnCard,isPlayer);
        UpdateScore(isPlayer);
        UpdateRemainingCards();
        }

    public void UpdateRemainingCards() {
        quantityOfCards--;
        uiManager.UpdateRemainingCards(quantityOfCards);
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

    public void DealersTurn()                 //if <17 the draw card for dealer, for the stand button OnClick()
    {
        if(enemyTurn==true)
        {   
            dealer.TakeTurn(this);
        }

    }


    private void ExposeHiddenCard()
    {
        hiddenCard.ExposeCard();
        FlipCard(hiddenCardGO,hiddenCard,false);
        hiddenCard = null;
        hiddenCardGO = null;
    }
    public void PlayerStand()
    {
        enemyTurn = true;
        ExposeHiddenCard();  //exposes the hidden card
    }

    public void CheckState()
    {

        if((playerScore >dealerScore && playerScore <=21) || dealerScore>21)
        {
            uiManager.ShowMessage("win");
            PlayerWon();    
        }
        else if((playerScore<dealerScore && dealerScore<=21 )|| playerScore  >21)
        {
            uiManager.ShowMessage("lose");
            PlayerLost();
        }
        else if(playerScore==dealerScore)
        {
            uiManager.ShowMessage("draw");
        }else if(playerScore==21 && dealerScore!= 21)
        {
            uiManager.ShowMessage("blackjack");
        }
            uiManager.UpdateDealersHealth(dealer.Showhp());
            uiManager.UpdatePlayersHealth(player.Showhp());
            CheckGameEnd();
        
    }

    public void CheckStateBeforeStand()
    {

        if(playerScore == 21)
        {
            uiManager.ShowMessage("blackjack");
            ExposeHiddenCard();
            PlayerWon();
            ResetGame(true);
        }
        else if (playerScore >21)
        {
            uiManager.ShowMessage("lose");
            ExposeHiddenCard();
            PlayerLost();
            ResetGame(false);
        }
    }
        private void CheckBlackjack() //check this, maybe this is avelord
    {
        if (playerScore == 21)
        {
            PlayerWon();
            ExposeHiddenCard();
            ResetGame(false);
        }
    }
    // ✅ Public method to update the boss face sprite based on anger
    public void UpdateBossFace(int faceIndex)
    {
        //currentBossIndex is needed here after the fix
    if (bossImage.sprite == bossSprites[0, faceIndex]) return;

    // ✅ Smooth transition using fade effect
    Sequence faceTransition = DOTween.Sequence();
    faceTransition.Append(bossImage.DOFade(0, 0.3f)) // 1️⃣ Fade out
                  .AppendCallback(() => bossImage.sprite = bossSprites[0, faceIndex]) // 2️⃣ Change sprite
                  .Append(bossImage.DOFade(1, 0.3f)); // 3️⃣ Fade in

 
        // ✅ Stop previous shake animation (prevents stacking)
    if (faceShakeTween != null && faceShakeTween.IsActive())
    {
        faceShakeTween.Kill();
    }

    // ✅ Continuous shake when anger is above 70
    if (faceIndex > 1 && faceIndex < 4)  // Moderate anger
    {
        faceShakeTween = bossImage.transform.DOShakePosition(2f, 8f, 5, 50, false, true)
            .SetLoops(-1, LoopType.Yoyo); // ✅ Keep shaking continuously
    }
    else if (faceIndex == 4) // Maximum anger
    {
        faceShakeTween = bossImage.transform.DOShakePosition(2.5f, 15f, 6, 60, false, true)
            .SetLoops(-1, LoopType.Yoyo); // ✅ More intense continuous shake
    }
    else  // If anger drops below 70, stop shaking
    {
        if (faceShakeTween != null && faceShakeTween.IsActive())
        {
            faceShakeTween.Kill(); // ✅ Stop shaking completely
        }
    }
    }
    


        private void CheckAnger()
        {
            die.CastADie();
            if(anger.value < 50)
            {
                // bossImage.sprite = bossSprites[0,0];
                UpdateBossFace(0);
                return;
            }else if(anger.value >=50 && anger.value<=70)
            {
                Debug.Log("anger value is up 50");
                points++;
                UpdateBossFace(1);
                die.ToggleDie(true);
                if(die.GetThrownNumber() == 0)
                {
                    bossRevolver.PullTrigger();
                    Debug.Log("boss shoots the dealer!");
                }
                // bossImage.sprite = bossSprites[0,1];
            }else if(anger.value>70 && anger.value <=80)
            {
                Debug.Log("anger value is up 70");
                points+=2;
                UpdateBossFace(2);
                die.ToggleDie(true);
                if(die.GetThrownNumber() == 0 || die.GetThrownNumber() == 1)
                {
                    bossRevolver.PullTrigger();
                    Debug.Log("boss shoots the dealer!");
                }
                // bossImage.sprite = bossSprites[0,2];
            }else if(anger.value>80 && anger.value <=90)
            {
                Debug.Log("anger value is up 80");
                 points+=3;
                 UpdateBossFace(3);
                die.ToggleDie(true);
                if(die.GetThrownNumber() == 0 || die.GetThrownNumber() == 1 || die.GetThrownNumber() == 2)
                {
                    bossRevolver.PullTrigger();
                    Debug.Log("boss shoots the dealer!");
                }
                //  bossImage.sprite = bossSprites[0,3];
            }else if(anger.value>90)
            {
                Debug.Log("anger value is up 90");
                 points+=5;
                 UpdateBossFace(4);
                die.ToggleDie(true);
                if(die.GetThrownNumber() != 5)
                {
                    bossRevolver.PullTrigger();
                    Debug.Log("boss shoots the dealer!");
                }
                //  bossImage.sprite = bossSprites[0,4];
            }
            uiManager.UpdatePoints(points);
            uiManager.UpdateAngerValue((int)anger.value);
        }

    public void PlayerWon()
    {
        player.AdjustHP((int)Bet.value);
        dealer.AdjustHP(-(int)Bet.value);
        SetAngerSlider(true);
    }
    public void PlayerLost()
    {
        player.AdjustHP(-(int)Bet.value);
        dealer.AdjustHP((int)Bet.value);
        SetAngerSlider(false);
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
    die.ToggleDie(false);

}

public IEnumerator ResetGameNextFrame()
{
    yield return null;
    // Reset game state
    playerScore = 0;
    dealerScore = 0;

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
    //     HandleDrawnAce(playerCardDrawn,false);                   //if drawn card is an Ace and playerscore >21 then it becomes 1
    //     DisplayCard(playerCardDrawn);
    // }

public Card JustDrawACard() //method for drawing a Card from the deck, used in the GamblerBehavior
{
    Card drawnCard = deckOfCards.DrawCard();
    dealerCards.Add(drawnCard);
    return drawnCard;
}

public Card GetShownCard() //method for getting a shown card of a dealer at initial hand, used in the GamblerBehavior
{
    return dealerCards[0];
}

public Transform GetShownCardGO()
{
    return dealerCardsPanel.GetChild(0);    ////method for getting a shown card GameObject of a dealer at initial hand, used in the GamblerBehavior
}

public void ReplaceShownCard(Card newCard) // Used in GamblerBehavior
{
    Transform shownCardGO = GetShownCardGO();
    Card shownCard = GetShownCard();
    if (shownCardGO == null) return; // Safety check

    Image shownCardImage = shownCardGO.GetComponent<Image>();
    if (shownCardImage == null) return; // Safety check

    // Store the old card before replacing it
    Card oldCard = shownCard; 
    
    // Adjusting the dealer's score
    dealerScore -= shownCard.value;
    dealerCards.Remove(oldCard);

    // // Replace the visual representation
    // shownCardImage.sprite = newCard.cardImage;

    // Replace the logic reference
    shownCard = newCard;
    
    // Recalculate the dealer's score
    UpdateScore(false);

    // Flip the new shown card
    BluffCardAnimation(shownCardGO.gameObject, newCard);
}
private void BluffCardAnimation(GameObject cardGO, Card newCard)
{
    Image cardImage = cardGO.GetComponent<Image>();
    if (cardImage == null) return; // Safety check

    Sequence bluffSequence = DOTween.Sequence();

    // Store the original color
    Color originalColor = cardImage.color;

    bluffSequence.Append(cardImage.DOColor(Color.red, 0.1f)) // 1️⃣ Flash red
                 .Append(cardImage.DOColor(originalColor, 0.1f)) // 2️⃣ Restore original color
                 .Append(cardImage.DOFade(0, 0.3f)) // 3️⃣ Fade out smoothly
                 .AppendCallback(() => cardImage.sprite = newCard.cardImage) // 4️⃣ Change sprite AFTER fade-out
                 .Append(cardImage.DOFade(1, 0.4f).SetEase(Ease.InOutQuad)) // 5️⃣ Smooth fade-in
                 .Join(cardGO.transform.DOPunchScale(Vector3.one * 0.15f, 0.4f, 8, 0.8f)); // 6️⃣ More natural bounce

    bluffSequence.Play();
}




public List<Card> RemainingDeck()   //getting remaining cards of the deck
{
    return deckOfCards.GetRemainingCards();
}
public List<Card> GetFullDeck()     //getting cars of the full deck
{
    return deckOfCards.GetFullDeck();
}
public Card GetHiddenCard()
{
    return hiddenCard;
}
public void SetAnger(int bossAnger)
{
    currentAngerValue = bossAnger;
}
public void SetAngerSlider(bool playerWon)
{
    float targetValue = playerWon ? anger.value + currentAngerValue : anger.value - currentChillValue;

    // ✅ Ensure the target value stays within slider limits
    targetValue = Mathf.Clamp(targetValue, anger.minValue, anger.maxValue);

    // ✅ Smoothly transition to the new anger value
    anger.DOValue(targetValue, 2f).SetEase(Ease.OutQuad);
}

public void SetChill(int chill)
{
    currentChillValue = chill;
}
public void SetQuantityOfCards(int cards)
{
    quantityOfCards = cards;
}
}
