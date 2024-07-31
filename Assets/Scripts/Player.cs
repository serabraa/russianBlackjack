
public class Player : HPRelated
{
    int playerHealth=0;


    public void Setup(int amountHP)
    {
        playerHealth = amountHP;
    }

    public void AdjustHP(int amountHP)
    {
        playerHealth += amountHP;
    }

    public int Showhp()
    {
        return playerHealth;
    }
}
