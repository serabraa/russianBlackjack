using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;



public class GameController : MonoBehaviour
{   
    

    int userScore = 0;
    int dealerScore = 0;
    // int changePosition = 140;
    bool enemyTurn = false;
    int aceCountPlayer= 0;              // ace counting to track the ace conditions
    int aceCountDealer = 0;             // ace counting to track the ace conditions
    public Canvas canvas;
    private Dealer dealer;              // dealer for dealer things
    private Player player;              // player for player things
    private DeckOfCards deckOfCards;
    private Card hiddenCard;            //needs for a method ResetImage
    private GameObject hiddenCardGO;    //needed for a method ResetImage
    [SerializeField] public UIManager uiManager;
    [SerializeField] private Transform playerCardsPanel; // UI panel for player cards
    [SerializeField] private Transform dealerCardsPanel; // UI panel for dealer cards
    [SerializeField] private Slider Bet;                // Slider for placing a bet



    void Start()
    {   
        
        dealer = new Dealer();
        player = new Player();
        dealer.Setup(50);
        player.Setup(50);
        deckOfCards = new DeckOfCards();
        DealInitialCards();
        CheckStateBeforeStand();
    }


    private void DealInitialCards()                 //dealing initial two cards for the player and for the dealer
    {
        Card playerCard1 = deckOfCards.DrawCard();
        Card playerCard2 = deckOfCards.DrawCard();
        Card enemyCard1 = deckOfCards.DrawCard();
        Card enemyCard2 = deckOfCards.DrawCard();
    //
        CountInitialAces(playerCard1,false);
        CountInitialAces(playerCard2,false);
        CountInitialAces(enemyCard1,true);
        CountInitialAces(enemyCard2,true);
    //
        enemyCard2.HideCard();
        hiddenCard = enemyCard2;
        // Display cards on UI
        DisplayCard(playerCard1);
        DisplayCard(playerCard2);
        DisplayEnemyCard(enemyCard1);
        DisplayEnemyCard(enemyCard2);
        

    }

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

    private void DisplayCard(Card card)
    {
        CreateAndPositionCard(card, playerCardsPanel, true);
        UpdateScore(card.value);
    }

    private void DisplayEnemyCard(Card card)
    {
        CreateAndPositionCard(card, dealerCardsPanel, false);
        CalculateValue(card.value);
    }

    private void CreateAndPositionCard(Card card, Transform parentPanel, bool isPlayer)
    {
        string cardName = isPlayer? "PlayerCard" : "DealelCard";
        GameObject cardGameObject = new GameObject(cardName);
        if (card == hiddenCard)
        {
            hiddenCardGO = cardGameObject;
        }
        cardGameObject.transform.SetParent(parentPanel, false);

        Image image = cardGameObject.AddComponent<Image>();
        image.sprite = card.cardImage;

        RectTransform rect = cardGameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(128, 196); // Card size
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f); // Center anchor

        AdjustCardPosition(parentPanel, rect, isPlayer);
    }

    private void AdjustCardPosition(Transform parentPanel, RectTransform rect, bool isPlayer)
    {
        int cardCount = parentPanel.childCount - 1; // Existing children count before adding new card
        float offset = cardCount * 140; // Horizontal offset; adjust as necessary
        rect.anchoredPosition = new Vector2(offset, 0); // Set position relative to the parent panel
    }



    public void DrawCardPlayer()                        //this one is for the player's card drawing
    {
        Card playerCardDrawn = deckOfCards.DrawCard();
        HandleDrawnAce(playerCardDrawn,false);                  //if drawn card is an Ace and userscore >21 then it becomes 1
        DisplayCard(playerCardDrawn);
    }


    public void DrawCardForDealer()                 //if <17 the draw card for dealer
    {
        if(enemyTurn==true)
        {   
            while(dealerScore<17)
             {
                Card drawenCard = deckOfCards.DrawCard();
                HandleDrawnAce(drawenCard,true); 
                DisplayEnemyCard(drawenCard);
            }
            if(dealerScore >=17)
            {
                CheckState();
            }
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

    private void CalculateValue(int value)
    {
        // Debug.Log(card.rank);
        // Debug.Log(card.suit);
        // Debug.Log(card.value);
        dealerScore = dealerScore + value;
        AceRecalculationFinal(true);
        Debug.Log("dealer's score is" + dealerScore);
    }


    private void ResetImage(Card card, GameObject cardGO)
    {
        Image image = cardGO.GetComponent<Image>();
        image.sprite = card.cardImage;
    }
    public void PlayerStand()
    {
        enemyTurn = true;
        hiddenCard.ExposeCard(); //exposes  the  hidden card
        ResetImage(hiddenCard, hiddenCardGO);   //resets image to display the hidden card
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
        ResetGame();
    }
    else
    {
        // If no one is below 0 HP, restart the dealing process
        ResetGame();
    }
}

public void ResetGame()
{


    // Clear cards from both panels
    foreach (Transform child in playerCardsPanel) {
        Destroy(child.gameObject);
    }
    foreach (Transform child in dealerCardsPanel) {
        Destroy(child.gameObject);
    }

    // Reset game state
    userScore = 0;
    dealerScore = 0;
    aceCountPlayer = 0;  // Assuming you have these counters for aces
    aceCountDealer = 0;

    // Reset UI elements
    uiManager.UpdateScore(0);  // Assuming you have a method to reset the score display


    // Restart the dealing process
    DealInitialCards();
}





    // public void DrawAce()                                //debug purposes
    // {
    //     Card playerCardDrawn =deckOfCards.DrawAce();
    //     HandleDrawnAce(playerCardDrawn,false);                   //if drawn card is an Ace and userscore >21 then it becomes 1
    //     DisplayCard(playerCardDrawn);
    // }


}
