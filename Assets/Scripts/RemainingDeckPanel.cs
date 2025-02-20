using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RemainingDeckPanel : MonoBehaviour
{

    [SerializeField] GameObject remainingDeckPanel;         // just our panel where cards will be shown
    [SerializeField] private Transform gridParent;          // Parent object with GridLayoutGroup
    [SerializeField] private GameObject cardPrefab;         // Prefab for individual card UI
    
    [SerializeField] GameController gameController;         //gamecontroller for methods to work with the deck
    Card hiddenCard;
    Sprite hiddenCardSprite;
    public void OpenInfoPanel()
    {
        remainingDeckPanel.SetActive(true);
        ClearAll();
        CreateRemainingCards();
        }

    public void CloseInfoPanel()
    {
        remainingDeckPanel.SetActive(false);
                ClearAll();

    }
        // Returns remaining cards as a string
    public string RemainingDeck()
    {
        List<Card> remainingCards = gameController.RemainingDeck();
        StringBuilder sb = new StringBuilder();

        foreach (Card card in remainingCards)
        {
            sb.AppendLine($"{card.rank} of {card.suit}");
        }

        return sb.ToString();
    }
    public void ClearAll()
    {
        // Clear existing cards in the grid
        foreach (Transform child in gridParent)
        {
            Debug.Log(child);
            Destroy(child.gameObject);
        }
    }
    public void CreateRemainingCards()
    {
         // Get remaining cards
        List<Card> fullDeckCards = gameController.GetFullDeck();
        Debug.Log($"Full deck size: {fullDeckCards.Count}");
        List<Card> remainingCards = gameController.RemainingDeck();
        Debug.Log($"Remaining deck size: {remainingCards.Count}");
        hiddenCard = gameController.GetHiddenCard();

        // Generate UI for each card
        foreach (var card in fullDeckCards)
        {
            GameObject cardGO = Instantiate(cardPrefab, gridParent); // Create card UI
            Image cardImage = cardGO.GetComponent<Image>();
            if (cardImage == null)
        {
            Debug.LogError("Prefab is missing an Image component!");
            continue;
        }
            cardImage.sprite = card.cardImage;   // Assign card face
            Debug.Log($"Instantiated card: {card.rank} of {card.suit}");

            if(card == hiddenCard)
            {
                hiddenCardSprite = hiddenCard.getExposedImage();
                cardImage.sprite = hiddenCardSprite;
                Debug.Log($"Skipping hidden card: {card.rank} of {card.suit}");
                continue;
            }
            if(!remainingCards.Contains(card))
            {
                Color color = cardImage.color;
                color.a = 0.4f;
                cardImage.color = color;
            }
        }    
    }
}