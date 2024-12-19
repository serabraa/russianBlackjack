using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossBehavior 
{
    void TakeTurn(GameController gameController);
    void OnDefeated();
}
