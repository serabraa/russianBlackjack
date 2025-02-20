using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookieBehavior : IBossBehavior
{   
    public GameController gameController;
    public int getAngry = 20; //anger value(after dealer's loss)
    public int getChilly = 15;// chill value (after dealer's win)


    public RookieBehavior(GameController gameController)
    {
        this.gameController = gameController;
        gameController.SetAnger(getAngry);
        gameController.SetChill(getChilly);
    }
    public void OnDefeated()
    {
        Debug.Log("rookie has been defeated");
    }

    public void TakeTurn(GameController gameController)
    {
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
