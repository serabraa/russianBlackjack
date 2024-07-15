using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    int userScore = 0;
    int dealerScore = 0;
    int changePosition = 140;
    int initialPosition = 0;
    int initialPositionEnemy = 0;
    int initialPositionY = 200;
    
    public Canvas canvas;
    private DeckOfCards deckOfCards;

    [SerializeField] public TMP_Text userScoreUI;


    void Start()
    {
        deckOfCards = new DeckOfCards();
        DealInitialCards();
    }

    private void DealInitialCards()                 //dealing initial two cards for the player and for the de
    {
        Card playerCard1 = deckOfCards.DrawCard();
        Card playerCard2 = deckOfCards.DrawCard();
        Card enemyCard1 = deckOfCards.DrawCard();
        Card enemyCard2 = deckOfCards.DrawCard();
        // Display cards on UI
        DisplayCard(playerCard1);
        DisplayCard(playerCard2);
        DisplayEnemyCard(enemyCard1);
        DisplayEnemyCardBack(enemyCard2);

    }

    private void DisplayCard(Card card)
    {
        CreateImageOfACardAndAdjustPositions(card,"PlayerCard",initialPosition,0,true);
        initialPosition = initialPosition + changePosition;
        UpdateScore(card.value);
    }

        private void DisplayEnemyCard(Card card)
    {
        CreateImageOfACardAndAdjustPositions(card,"EnemyCard", initialPositionEnemy,initialPositionY,true);
        initialPositionEnemy = initialPositionEnemy + changePosition;

    }
        private void DisplayEnemyCardBack(Card card)
        {
            CreateImageOfACardAndAdjustPositions(card,"EnemyCard", initialPositionEnemy,initialPositionY,false);
            
        }


        private void CreateImageOfACardAndAdjustPositions(Card card,string nameOfACard, int posX,int posY,bool spriteImage)
    {
        // sarqumenq mi hat gameobject anuny Card
        GameObject cardGameObject = new GameObject(nameOfACard);

        // Add the Image component to the GameObject
        Image image = cardGameObject.AddComponent<Image>();

        // Make the new GameObject a child of the canvas
        cardGameObject.transform.SetParent(canvas.transform, false);

        AdjustPositions(card, image, posX, posY, spriteImage);
    }
    // private void AdjustPositions(Card card, Image image)
    // {
    //     // Optional: Set the size of the card image
    //     image.rectTransform.sizeDelta = new Vector2(128, 196); // Set size of the card image
    //     //avelacnumenq imagei vra mer cardi sprite
    //     image.sprite = card.cardImage;
        
    //     // Optional: Position the card on the canvas
    //     image.rectTransform.anchoredPosition = new Vector2(initialPosition, 0); // Position the card in the center or wherever you need it   
    // }

        private void AdjustPositions(Card card, Image image, int posX,int posY, bool spriteImage)
    {
        // Optional: Set the size of the card image
        image.rectTransform.sizeDelta = new Vector2(128, 196); // Set size of the card image
        //avelacnumenq imagei vra mer cardi sprite
        if(spriteImage==true){
        image.sprite = card.cardImage;
        }else{
        image.sprite = card.cardBack;}
        
        // Optional: Position the card on the canvas
        image.rectTransform.anchoredPosition = new Vector2(posX, posY); // Position the card in the center or wherever you need it   
    }



    public void DrawCardPlayer()                        //this one is for the player's card drawing
    {
        Card playerCardDrawn = deckOfCards.DrawCard();
        DisplayCard(playerCardDrawn);
    }

    public void DrawCard()
    {
        Card enemyCardDrawn = deckOfCards.DrawCard();
    }

    public void UpdateScore(int value)
    {
        userScore = userScore + value;
        userScoreUI.SetText("Score: " + userScore);
        CheckState();
    }

    public void CheckState()
    {
        if(userScore ==21){
            Debug.Log("blackjack!");
        }
        else if(userScore>21){
            Debug.Log("you blow");
        }
        else 
        Debug.Log("you good!");

    }
    
    //ENEMY AI STARTS HERE

    private void CalculateValue(Card card)
    {
        Debug.Log(card.rank);
        Debug.Log(card.suit);
        Debug.Log(card.value);
        dealerScore = dealerScore + card.value;
        CheckDealer(dealerScore);
    }
    private void CheckDealer(int dlrscore)
    {
        if(dlrscore <17){
            Debug.Log("chenq qashum 17 e kam mec");
        }else DrawCardForDealer();
    }
    public void DrawCardForDealer()
    {
        Debug.Log("qashumenq qani or 17ic cacre");
        Card drawenCard = deckOfCards.DrawCard();
        CalculateValue(drawenCard);

    }
}
