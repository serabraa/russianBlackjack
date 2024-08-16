using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    bool[] ammo = new bool[6];
    int ammoCount = 0;
    int hammerIndex = 0;



    public void CylinderRotation()
    {
        if(hammerIndex==5)
        {
            hammerIndex = 0; 
        }else
        {
        hammerIndex +=1;
        }
        Debug.Log("hammer is pointing at " + hammerIndex);
    }

    public void ShowCurrentPosition()
    {
        Debug.Log(ammo[hammerIndex]? "Bullet present" : "No bullet");
    }

    public void PutABullet()
{
    if (ammoCount == ammo.Length)
    {
        Debug.Log("All positions are filled");
        return;
    }

    int bulletPosition = Random.Range(0, ammo.Length);
    while (ammo[bulletPosition]) // Only retry if the randomly selected position is filled
        {
        bulletPosition = Random.Range(0, ammo.Length);
        }
    
    ammo[bulletPosition] = true;
    ammoCount++;
    Debug.Log($"Bullet placed at position {bulletPosition}");
}


    public void Shoot()
    {
        if(ammoCount== 0 || ammo[hammerIndex]==false)
        {
            Debug.Log("missing bullet or wrong hammer position");
            return;
        }else 
        {
            ammo[hammerIndex]=false;
            ammoCount--;
            Debug.Log("Shot was taken");
        }


    }

}
