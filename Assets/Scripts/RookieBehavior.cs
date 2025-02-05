using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookieBehavior : IBossBehavior
{
    public void OnDefeated()
    {
        Debug.Log("rookie has been defeated");
    }

    public void TakeTurn(GameController gameController)
    {
        gameController.SetAnger(90);
            while(gameController.dealerScore<17)
            {
                gameController.DrawCard(false);
            }
            if(gameController.dealerScore >=17)
            {
                gameController.CheckState();
            }
    }
}
