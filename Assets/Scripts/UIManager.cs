using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] public TMP_Text userScoreUI;
    [SerializeField] public TMP_Text userWonUI;
    [SerializeField] public TMP_Text userLostUI;
    [SerializeField] public TMP_Text blackJackUI;
    [SerializeField] public TMP_Text drawUI;
    
 
    public void UpdateScore(int score)
    {
        userScoreUI.SetText("Score: " + score);
    }

    public void ShowMessage(string message)
    {
        switch(message)
        {
            case "win":
                userWonUI.gameObject.SetActive(true);
                break;
            case "lose":
                userLostUI.gameObject.SetActive(true);
                break;
            case "draw":
                drawUI.gameObject.SetActive(true);
                break;
            case "blackjack":
                blackJackUI.gameObject.SetActive(true);
                break;
        }
    }

    public void HideAllMessages()
    {
        userWonUI.gameObject.SetActive(false);
        userLostUI.gameObject.SetActive(false);
        drawUI.gameObject.SetActive(false);
        blackJackUI.gameObject.SetActive(false);
    }
}
