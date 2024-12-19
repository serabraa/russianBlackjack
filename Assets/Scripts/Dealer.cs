using UnityEngine;

public class Dealer : HPRelated
{
    private int dealerHealth = 0;
    private IBossBehavior bossBehavior;


    public Dealer (IBossBehavior behavior)
    {
        bossBehavior = behavior;
    }
    public void Setup(int amountHP)
    {
        dealerHealth = amountHP;
    }

    public void AdjustHP(int amountHP)
    {
        dealerHealth += amountHP;
        if( dealerHealth<= 0)
        {
            Die();
        }
    }
    public int Showhp()
    {
        return dealerHealth;
    }
    public void Die()
    {
        Debug.Log("dead dealer :(");
        dealerHealth =-1;
        bossBehavior.OnDefeated();
    }

    public void TakeTurn(GameController gameController)
    {
        bossBehavior.TakeTurn(gameController);
    }
}
