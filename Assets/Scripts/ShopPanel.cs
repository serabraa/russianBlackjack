using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] GameObject shopPanel;

    [SerializeField] GameObject openShopButton;

public void openShopPanel()
{
    shopPanel.SetActive(true);
}
}
