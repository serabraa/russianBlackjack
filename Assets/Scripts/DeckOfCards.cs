using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DeckOfCards 
{
    private List<Card> cards; //remainingCards, cards that are being played
    private List<Card> fullDeckCards; //All cards in the full deck
    private Sprite cardBack;
    private int numberOfDecks;


    public DeckOfCards(int numberOfDecks = 1)
    {
        this.numberOfDecks = numberOfDecks;
        cards = new List<Card>();
        fullDeckCards = new List<Card>();
        InitializeDeck();
    }

    private void InitializeDeck()               //hertov initialize kenenq sax cardery
    {
        string[] suits = {"Hearts","Clubs","Diamonds","Spades"};    //sirt,xach,qyap,xar
        string[] ranks = {"2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"};
        Dictionary <string,int> values = new Dictionary<string, int>
        {
            {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6}, {"7", 7}, {"8", 8}, {"9", 9}, {"10", 10}, {"Jack", 10},
            {"Queen", 10}, {"King", 10}, {"Ace", 11}
        };
        // cards.Clear();                                     

        string pathToback = "Cards/CardBack";              
        cardBack = Resources.Load<Sprite>(pathToback);    //cardback qcenq
    for(int deck = 0; deck< numberOfDecks; deck++)        //deckeri qanakov ktpenq edqan deck, xosqi numberOfDecks = 4 => 4 kalod qart
    {
        foreach(var suit in suits)
        {
            foreach(var rank in ranks)
            {
                string path = $"Cards/{suit}_{rank}";
                Sprite image = Resources.Load<Sprite>(path);
                Card card = new Card(suit, rank, values[rank], image,cardBack);
                cards.Add(card);    //this is added to our actual playing cards
                fullDeckCards.Add(card);    // this list is not being touched anywehre, it is only for keeping the initial full deck purposes
            }
        }
    }
        
    }
    public Card DrawCard()          //card qashel
    {
        if(cards.Count == 0)
        {
            return null;            //ete card chka kalodi mej apa null
        }
        Card card = cards[Random.Range(0,cards.Count)];        //ete ka random me cardm kqashenq kalodic
        // Debug.Log(card.value + card.rank);
        cards.Remove(card);                                    //hanum enq kalodic
        // Debug.Log(cards.Count);//debug purposes
        return card;                                            
    }

    public bool IsDeckEmpty()
    {
        if (cards.Count <4){return true;}
        return false;
    }

    public List<Card> GetRemainingCards()   
    {
        return cards;  //returns a List of remaining cards
    }    

    public List<Card> GetFullDeck()
    {
        return fullDeckCards;  //return a List of initial full deck
    }
        // public Card DrawKing()
    // {
    //     Card card = cards[11];
    //     return card; 
    // }
    // public Card DrawAce()        //debug purposes
    // {
    //     Card card = cards[12];
    //     return card;
    // }
    
}
