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
    private DeckOfCards deckOfCards;
    private Card hiddenCard;            //needs for a method ResetImage
    private GameObject hiddenCardGO;    //needed for a method ResetImage
    [SerializeField] public TMP_Text userScoreUI;
    [SerializeField] public TMP_Text userWonUI;
    [SerializeField] public TMP_Text userLostUI;
    [SerializeField] public TMP_Text blackJackUI;
    [SerializeField] public TMP_Text drawUI;


    void Start()
    {
        deckOfCards = new DeckOfCards();
        DealInitialCards();
        CheckStateBeforeStand();
    }

    void Update()
    {
        DrawCardForDealer();
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


        // Make the new GameObject a child of the canvas
        cardGameObject.transform.SetParent(canvas.transform, false);

        AdjustPositions(card, image, posX, posY);
    }

    private void AdjustPositions(Card card, Image image, int posX,int posY)
    {
        // Optional: Set the size of the card image
        image.rectTransform.sizeDelta = new Vector2(128, 196); // Set size of the card image
        //avelacnumenq imagei vra mer cardi sprite

        image.sprite = card.cardImage;
        
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
        userScoreUI.SetText("Score: " + userScore);
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

    private void DrawCardForDealer()
    {
        if(enemyTurn==true)
        {   
            if(dealerScore<17)
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
            userWonUI.gameObject.SetActive(true);
        }
        else if((userScore<dealerScore && dealerScore<=21 )|| userScore  >21)
        {
            userLostUI.gameObject.SetActive(true);
        }
        else if(userScore==dealerScore)
        {
            drawUI.gameObject.SetActive(true);
        }else if(userScore==21 && dealerScore!= 21)
        {
            blackJackUI.gameObject.SetActive(true);
        }
        
    }

    public void CheckStateBeforeStand()
    {

        if(userScore == 21)
        {
            blackJackUI.gameObject.SetActive(true);
        }
        else if (userScore >21)
        {
            userLostUI.gameObject.SetActive(true);
        }
    }
}
