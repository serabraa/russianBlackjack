using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPanel : MonoBehaviour
{
    [SerializeField] GameObject bossPanel;         // boss panel with info in it
    [SerializeField] GameObject closeButton;

    public void OpenBossPanel()
    {
        bossPanel.SetActive(true);
    }

    // public void CloseBossPanel()
    // {
    //     bossPanel.SetActive(false);
    // }
}
