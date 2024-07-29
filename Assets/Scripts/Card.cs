using UnityEngine;

public class Card 
{
    public string suit;         //cardi mast(qyap,xach)
    public string rank;         //cardi anun(karol,tuz)
    public int value;           //tvayin arjeqy karti
    public Sprite cardImage;    //cardi nkary
    public Sprite cardBack;     //hetevi masy cardi
    private Sprite cardImageReserve;


    public Card(string Suit, string Rank, int Value, Sprite CardImage,Sprite CardBack)
    {
        suit = Suit;
        rank = Rank;
        value = Value;
        cardImage = CardImage;
        cardBack = CardBack;
    }

    //helper methods
    public void HideCard()
    {
        cardImageReserve = cardImage;
        cardImage=cardBack;
    }
    public void ExposeCard()
    {
        cardImage= cardImageReserve;
    }
}
