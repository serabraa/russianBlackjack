using UnityEngine;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    [SerializeField] Sprite[] dieSprites = new Sprite[6];
    [SerializeField] Image dice;
    int thrownNumber = 0;
public void CastADie()
{
    thrownNumber = Random.Range(0,6);
    ChangeDieSprite(thrownNumber);
    Debug.Log(thrownNumber);
}
public void ChangeDieSprite(int someNum)
{
    switch(someNum)
    {
        case 0:
        dice.sprite = dieSprites[0];
        break;
        case 1:
        dice.sprite = dieSprites[1];
        break;
        case 2:
        dice.sprite = dieSprites[2];
        break;
        case 3:
        dice.sprite = dieSprites[3];
        break;
        case 4:
        dice.sprite = dieSprites[4];
        break;
        case 5:
        dice.sprite = dieSprites[5];
        break;
    }
    
}
public int GetThrownNumber()
{
    return thrownNumber;
}
public void ToggleDie(bool on)
{
    if(on)
    {
        dice.gameObject.SetActive(on);
    }else
    dice.gameObject.SetActive(on);
}

}
