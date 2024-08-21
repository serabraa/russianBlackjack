using UnityEngine;
using UnityEngine.UI;
using TMPro;


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
    [SerializeField] private Transform playerCardsPanel; // Assign this in Unity Editor
    [SerializeField] private Transform dealerCardsPanel; // Assign this in Unity Editor



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

    void Update()
    {
        // DrawCardForDealer();
    }

    private void DealInitialCards()                 //dealing initial two cards for the player and for the dealer
    {
        Card playerCard1 = deckOfCards.DrawCard();
        Card playerCard2 = deckOfCards.DrawCard();
        Card enemyCard1 = deckOfCards.DrawCard();
        Card enemyCard2 = deckOfCards.DrawCard();
    //
        InitialCheckForTuz(playerCard1,false);
        InitialCheckForTuz(playerCard2,false);
        InitialCheckForTuz(enemyCard1,true);
        InitialCheckForTuz(enemyCard2,true);
    //
        enemyCard2.HideCard();
        hiddenCard = enemyCard2;
        // Display cards on UI
        DisplayCard(playerCard1);
        DisplayCard(playerCard2);
        DisplayEnemyCard(enemyCard1);
        DisplayEnemyCard(enemyCard2);
        

    }

    // //////////// start
    private void InitialCheckForTuz(Card card, bool isEnemy)        //counts initial aces for further handling logic
    {
        if(card.rank =="Ace")
        {
            if(isEnemy)
            {
                aceCountDealer++;
            }else
            {
                aceCountPlayer++;
            }
            
        }
        
    }

    private void TuzRecalculationHandling(bool isEnemy)         //handles initialy dealt ace value's logic (1 or 11)
    {
        
        if(!isEnemy && aceCountPlayer!=0  && userScore>21 )
        {
            userScore -=10;
            aceCountPlayer--;
        }else if(isEnemy && aceCountDealer!=0 && dealerScore>21 )
        {
            dealerScore-=10;
            aceCountDealer--;
        }
    }
    private void DrawnTuzCaseCheckPlayer(Card card)           // Ace check for cards that are being drawn
    {
        if(card.rank=="Ace" && userScore >21)
        {
            userScore=userScore-10;
        }
    }

    private void DrawnTuzCaseCheckDealer(Card card)           // Ace check for cards that are being drawn
    {
        if(card.rank=="Ace" && dealerScore >21)
        {
            dealerScore=dealerScore-10;
        }
    }

    // //////////// end

    private void DisplayCard(Card card)
    {
        CreateAndPositionCard(card, playerCardsPanel, true);
        UpdateScore(card.value);
    }

    private void DisplayEnemyCard(Card card)
    {
        CreateAndPositionCard(card, dealerCardsPanel, false);
        CalculateValue(card);
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
        DrawnTuzCaseCheckPlayer(playerCardDrawn);                  //if drawn card is an Ace and userscore >21 then it becomes 1
        DisplayCard(playerCardDrawn);
    }


    public void DrawCardForDealer()                 //if <17 the draw card for dealer
    {
        if(enemyTurn==true)
        {   
            while(dealerScore<17)
             {
                Card drawenCard = deckOfCards.DrawCard();
                DrawnTuzCaseCheckDealer(drawenCard);
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
        TuzRecalculationHandling(false);
        uiManager.UpdateScore(userScore);
        // CheckState();
    }


    //ENEMY AI STARTS HERE

    private void CalculateValue(Card card)
    {
        // Debug.Log(card.rank);
        // Debug.Log(card.suit);
        // Debug.Log(card.value);
        dealerScore = dealerScore + card.value;
        TuzRecalculationHandling(true);
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
            // CheckGameEnd();
        
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
        player.AdjustHP(20);
        dealer.AdjustHP(-20);
    }
       public void PlayerLost()
    {
        player.AdjustHP(-20);
        dealer.AdjustHP(20);
    }


    // public void CheckGameEnd()
    // {
    //     if(player.Showhp() >= 0 && dealer.Showhp()>=0)
    //     {

    //     }
    // }



}
