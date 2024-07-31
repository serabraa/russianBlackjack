
public class Dealer : HPRelated
{
    int dealerHealth=0;


    public Dealer ()
    {
        
    }
    public void Setup(int amountHP)
    {
        dealerHealth = amountHP;
    }

    public void AdjustHP(int amountHP)
    {
        dealerHealth += amountHP;
    }
        public int Showhp()
    {
        return dealerHealth;
    }
}
