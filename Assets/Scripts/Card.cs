using UnityEngine;

public class Card 
{
    public string suit;         //cardi mast(qyap,xach)
    public string rank;         //cardi anun(karol,tuz)
    public int value;           //tvayin arjeqy karti
    public Sprite cardImage;    //cardi nkary
    public Sprite cardBack;     //hetevi masy cardi


    public Card(string Suit, string Rank, int Value, Sprite CardImage,Sprite CardBack)
    {
        suit = Suit;
        rank = Rank;
        value = Value;
        cardImage = CardImage;
        cardBack = CardBack;
    }



    
    //     // Override the Equals method so it will work while comparing. With the default Equals() it does not work correctly in Unity, while classes had MONOBEHAVIOUR, without it it works fine
    // public override bool Equals(object obj)
    // {
    //     if (obj == null || this.GetType() != obj.GetType())
    //         return false;

    //     Card other = (Card)obj;
    //     return this.suit == other.suit && this.rank == other.rank;
    // }

    // // Override GetHashCode
    // public override int GetHashCode()
    // {
    //     // Use hash code of concatenated suit and rank strings
    //     // It's simple and usually sufficient, but you can optimize it based on your needs
    //     int hash = 17;
    //     hash = hash * 31 + (suit ?? "").GetHashCode();
    //     hash = hash * 31 + (rank ?? "").GetHashCode();
    //     return hash;
    // }
}
