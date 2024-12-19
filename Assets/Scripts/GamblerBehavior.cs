using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblerBehavior : IBossBehavior
{
    private bool hasBluffed = false;
    public void OnDefeated()
    {
        Debug.Log("GAMBLER HAS BEEN DEFEATED");
    }

    public void TakeTurn(GameController gameController)
    {
            if (!hasBluffed)
        {
            Debug.Log("The Gambler bluffs with a fake card!");
            hasBluffed = true;
        }
        else
        {
            Debug.Log("The Gambler plays normally.");
        }
    }
}
