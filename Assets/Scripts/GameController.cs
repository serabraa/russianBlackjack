using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour
{   
    

    int userScore = 0;
    int dealerScore = 0;
    int changePosition = 140;
    int initialPosition = 0;
    int initialPositionEnemy = 0;
    int initialPositionY = 200;

    bool enemyTurn = false;
    
    public Canvas canvas;
    private Dealer dealer;              // dealer for dealer things
    private Player player;              // player for player things
    private DeckOfCards deckOfCards;
    private Card hiddenCard;            //needs for a method ResetImage
    private GameObject hiddenCardGO;    //needed for a method ResetImage
    [SerializeField] public UIManager uiManager;
    [SerializeField] Transform cardContainer ;


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
        enemyCard2.HideCard();
        hiddenCard = enemyCard2;
        // Display cards on UI
        DisplayCard(playerCard1);
        DisplayCard(playerCard2);
        DisplayEnemyCard(enemyCard1);
        DisplayEnemyCard(enemyCard2);
        

    }

    private void DisplayCard(Card card)
    {
        CreateImageOfACardAndAdjustPositions(card,"PlayerCard",initialPosition,0);
        initialPosition = initialPosition + changePosition;
        UpdateScore(card.value);
    }


    private void DisplayEnemyCard(Card card)                           //displays enemy's card and calculates it's value
    {
        CreateImageOfACardAndAdjustPositions(card,"EnemyCard", initialPositionEnemy,initialPositionY);
        initialPositionEnemy = initialPositionEnemy + changePosition;
        CalculateValue(card);
    }



    private GameObject CreateGameObjectOfACard(string nameOfACard,Card card)
    {
        GameObject cardGO = new GameObject(nameOfACard);
        cardGO.transform.SetParent(cardContainer,false);
        if(card == hiddenCard){
            hiddenCardGO =  cardGO;
        }
        return cardGO;
    }

    


    private void CreateImageOfACardAndAdjustPositions(Card card,string nameOfACard, int posX,int posY)
    {
        
        // sarqumenq mi hat gameobject anuny Card
        GameObject cardGameObject = CreateGameObjectOfACard(nameOfACard,card);

        // Add the Image component to the GameObject
        Image image = cardGameObject.AddComponent<Image>();


        image.sprite = card.cardImage;

        AdjustPositions(card, image, posX, posY);
    }

    private void AdjustPositions(Card card, Image image, int posX,int posY)
    {
        // Optional: Set the size of the card image
        image.rectTransform.sizeDelta = new Vector2(128, 196); // Set size of the card image

        // Optional: Position the card on the canvas
        image.rectTransform.anchoredPosition = new Vector2(posX, posY); // Position the card in the center or wherever you need it   
    }




    public void DrawCardPlayer()                        //this one is for the player's card drawing
    {
        Card playerCardDrawn = deckOfCards.DrawCard();
        DisplayCard(playerCardDrawn);
    }



    public void UpdateScore(int value)
    {
        userScore = userScore + value;
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
        Debug.Log("dealer's score is" + dealerScore);
    }

    public void DrawCardForDealer()
    {
        if(enemyTurn==true)
        {   
            while(dealerScore<17)
             {
                // Debug.Log("qashumenq qani or 17ic cacre");
                Card drawenCard = deckOfCards.DrawCard();
                DisplayEnemyCard(drawenCard);
            }
            if(dealerScore >=17)
            {
                CheckState();
            }
        }

    }
    private void ResetImage(Card card, GameObject cardGO)
    {
        Image image = cardGO.GetComponent<Image>();
        image.sprite = card.cardImage;
    }
    public void PlayerStand()
    {
        enemyTurn = true;
        hiddenCard.ExposeCard();
        ResetImage(hiddenCard, hiddenCardGO);
    }

    public void CheckState()
    {

        if((userScore >dealerScore && userScore <=21) || dealerScore>21)
        {
            uiManager.ShowMessage("win");
            PlayerWon();
            // uiManager.UpdateDealersHealth(dealer.Showhp());
            // uiManager.UpdatePlayersHealth(player.Showhp());
            // Debug.Log("players hp is "+player.Showhp());
            // Debug.Log("dealers hp is "+dealer.Showhp());
        }
        else if((userScore<dealerScore && dealerScore<=21 )|| userScore  >21)
        {
            uiManager.ShowMessage("lose");
            PlayerLost();
            // Debug.Log("players hp is "+player.Showhp());
            // Debug.Log("dealers hp is "+dealer.Showhp());
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



}
