using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int changePosition = 140;
    int initialPosition = 0;
    public Canvas canvas;
    private DeckOfCards deckOfCards;


    void Start()
    {
        deckOfCards = new DeckOfCards();
        DealInitialCards();
    }

    private void DealInitialCards()
    {
        Card playerCard1 = deckOfCards.DrawCard();
        Card playerCard2 = deckOfCards.DrawCard();
        // Display cards on UI
        DisplayCard(playerCard1);
        DisplayCard(playerCard2);
    }

    private void DisplayCard(Card card)
    {
    // sarqumenq mi hat gameobject anuny Card
    GameObject cardGameObject = new GameObject("Card");

    // Add the Image component to the GameObject
    Image image = cardGameObject.AddComponent<Image>();

    // Make the new GameObject a child of the canvas
    cardGameObject.transform.SetParent(canvas.transform, false);

    // Optional: Set the size of the card image
    image.rectTransform.sizeDelta = new Vector2(128, 196); // Set size of the card image
    //avelacnumenq imagei vra mer cardi sprite
    image.sprite = card.cardImage;
    
    // Optional: Position the card on the canvas
    image.rectTransform.anchoredPosition = new Vector2(initialPosition, 0); // Position the card in the center or wherever you need it
    initialPosition = initialPosition + changePosition;
    }

    public void DrawCard()
    {
        Card playerCardDrawn = deckOfCards.DrawCard();
        DisplayCard(playerCardDrawn);
    }
}
