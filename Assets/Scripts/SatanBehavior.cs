using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatanBehavior : IBossBehavior
{
    public void OnDefeated()
    {
        Debug.Log("NOO YOU HAVE DEFEATED THE SATAN");
    }

    public void TakeTurn(GameController gameController)
    {
        Debug.Log("Satan always has a perfect hand of 21.");
        if (gameController.userScore < 21)
        {
            Debug.Log("Satan punishes the player for not having 21!");
            gameController.PlayerLost();
        }
        else
        {
            Debug.Log("Satan is impressed by your skill.");
        }
        gameController.CheckState();
    }
}
