using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameController gameController;
    int jokerPrice = 25;
    int chillPillPrice = 10;
    int bulletPrice = 80;
    int peekRevolverPrice = 40;
public void openShopPanel()
{
    shopPanel.SetActive(true);
}

public void BuyJoker()
{
    int playerPoints = gameController.GetPlayerPoints();
    Debug.Log(playerPoints);
    if(playerPoints >= jokerPrice)
    {
        gameController.UpdatePlayerPoints(jokerPrice);
        gameController.AddJoker();
    }
}
public void BuyChillPill()
{
    int playerPoints = gameController.GetPlayerPoints();
    Debug.Log(playerPoints);
    if(playerPoints >= chillPillPrice)
    {
        gameController.UpdatePlayerPoints(chillPillPrice);
        gameController.AddChillPill();
    }
}
public void BuyBullet()
{
    Debug.Log(GetPoints());
    if(GetPoints() >= bulletPrice)
    {
        gameController.UpdatePlayerPoints(bulletPrice);
        gameController.AddBullet();
    }
}
public void BuyRevolverPeek()
{
Debug.Log(GetPoints());
    if(GetPoints() >= peekRevolverPrice)
    {
        gameController.UpdatePlayerPoints(peekRevolverPrice);
        gameController.AddPeek();
    }    
}
public int GetPoints()
{
    int playerPoints = gameController.GetPlayerPoints();
    return playerPoints;
}
}
