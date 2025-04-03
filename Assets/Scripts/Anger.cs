using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Anger : MonoBehaviour
{
    [SerializeField] private Slider angerBar;

    public void ChangeAnger(int amount)
    {
        int angerValue = (int) angerBar.value;
        angerValue+= amount;
        angerBar.value = angerValue;
    }
}
