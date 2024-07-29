using System.Collections.Generic;
using UnityEngine;


public class DeckOfCards 
{
    private List<Card> cards;
    private Sprite cardBack;


    public DeckOfCards()
    {
        cards = new List<Card>();
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

        foreach(var suit in suits)
        {
            foreach(var rank in ranks)
            {
                string path = $"Cards/{suit}_{rank}";
                Sprite image = Resources.Load<Sprite>(path);
                Card card = new Card(suit, rank, values[rank], image,cardBack);
                cards.Add(card);
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
        return card;                                            
    }
    
}
