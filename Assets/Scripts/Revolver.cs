using UnityEngine.UI;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    bool[] ammo = new bool[6];
    int ammoCount = 0;
    int hammerIndex = 0;
    [SerializeField] Sprite [] cylinderSprites;
    // [SerializeField] GameObject cylinder;
    [SerializeField] Image cylinderImage;


    void Start()
    {
        cylinderImage.sprite = cylinderSprites[ammoCount];

    }
    private void ChangeCylinderSprite()
    {
        cylinderImage.sprite = cylinderSprites[ammoCount];
    }
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
        ChangeCylinderSprite();
    }

    public void ShowCurrentPosition()
    {
        Debug.Log(ammo[hammerIndex]? "Bullet present" : "No bullet");
    }

    
    
    public void BuyABullet()
    {
        if (ammoCount == ammo.Length)
        {
            Debug.Log("Can not but more ammo");
            return;
        }
        PutABullet();
    }
    private void PutABullet()
{

    int bulletPosition = Random.Range(0, ammo.Length);
    while (ammo[bulletPosition]) // Only retry if the randomly selected position is filled
        {
        bulletPosition = Random.Range(0, ammo.Length);
        }
    
    ammo[bulletPosition] = true;
    ammoCount++;
    Debug.Log($"Bullet placed at position {bulletPosition}");
    ChangeCylinderSprite();
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
            ChangeCylinderSprite();
        }


    }

}
