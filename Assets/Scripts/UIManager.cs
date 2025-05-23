using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] public TMP_Text userCountUI;//count
    [SerializeField] public TMP_Text userScoreUI;
    [SerializeField] public TMP_Text dealerScoreUI;
    [SerializeField] public TMP_Text userWonUI;
    [SerializeField] public TMP_Text userLostUI;
    [SerializeField] public TMP_Text blackJackUI;
    [SerializeField] public TMP_Text drawUI;
    [SerializeField] public TMP_Text playersHealth;
    [SerializeField] public TMP_Text dealersHealth;
    [SerializeField] public TMP_Text pointsUI;
    [SerializeField] public TMP_Text remainningCardsUI;
    [SerializeField] public TMP_Text peekUI; //shows in the text form, whether we peek on the ammo or not
    [SerializeField] public Button hit;
    [SerializeField] public Button stand;
    [SerializeField] public GameObject gameOver;
    [SerializeField] TMP_Text angerValueText;
    public TMP_Text betValue;
    
    public void UpdatePeekUI(bool isRight, int hammerPosition)
    {
        if(isRight){peekUI.SetText("you have peeked, the ammo is in the position " + (hammerPosition+1));}
        else peekUI.SetText("you have peeked, the ammo is NOT in the position " + (hammerPosition+1));
    }
    public void UpdatePlayerScore(int score)
    {
        userScoreUI.SetText("Player's score is " + score);
    }

    public void UpdateDealerScore(int score)
    {
        dealerScoreUI.SetText("Dealer's score is " + score);
    }
    public void UpdateRemainingCards(int cards)
    {
        remainningCardsUI.SetText("remaining cards: " + cards);
    }
    public void UpdateAngerValue(int anger)
    {
        angerValueText.SetText("anger: " + anger);
    }
    public void UpdateDealersHealth(int dealerHealth)
    {
        dealersHealth.SetText("Dealer's health is " + dealerHealth );
    }

    public void UpdatePlayersHealth(int playerHealth)
    {
        playersHealth.SetText("Player's health is " + playerHealth );
    }


    public void UpdateScore(int score)
    {
        userCountUI.SetText("Count: " + score);
    }
    public void UpdatePoints(int points)
    {
        pointsUI.SetText("Points: " + points);
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
    
    public void ChangeBetValue(float value)
    {
        betValue.SetText(""+Mathf.RoundToInt(value));
    }

    public void HitStandActivity(bool parameter)
    {
        hit.gameObject.SetActive(parameter);
        stand.gameObject.SetActive(parameter);
    }
    public void SetHitAndStandActive(bool isActive)
    {
        hit.interactable = isActive;
        stand.interactable = isActive;
    }

    public void gameOverToggle(bool value)
    {
        gameOver.gameObject.SetActive(value);
    }
    //create a method which makes hit and stand image's color alpha(transparency) to 50 in stead of hiding it completely
}
