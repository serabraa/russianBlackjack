using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
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
    [SerializeField] public TMP_Text dealerHitsUI;
    [SerializeField] public TMP_Text dealerMissesUI;
    [SerializeField] public TMP_Text jokerQuantityUI;
    [SerializeField] public TMP_Text chillPillQuantityUI;
    [SerializeField] public TMP_Text peekQuantityUI;
    [SerializeField] public Button hit;
    [SerializeField] public Button stand;
    [SerializeField] public GameObject gameOver;
    [SerializeField] public GameObject gameWin;
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
    public void UpdateChilPillQuantity(int quant)
    {
        chillPillQuantityUI.SetText("" + quant);
    }
        public void UpdatePeekQuantity(int quant)
    {
        peekQuantityUI.SetText("" + quant);
    }
        public void UpdateJokerQuantity(int quant)
    {
        jokerQuantityUI.SetText("" + quant);
    }

    public void UpdateScore(int score)
    {
        userCountUI.SetText("Count: " + score);
    }
    public void UpdatePoints(int points)
    {
        pointsUI.SetText("Points: " + points);
    }
    public void ShowGameOver()
    {
        gameOver.SetActive(true);
        dealerHitsUI.gameObject.SetActive(false);
    }
    public void DealerShoots(bool hitOrMiss)
    {
        if(hitOrMiss == true)
        {
            gameOver.SetActive(true);
            dealerHitsUI.SetText("Game is OVER! Angry dealer shot you!!!");
        }
        // dealerMissesUI.SetText("Lucky you! Dealer missed his shot!");
    }

    public void ToggleDealerMisses(bool state)
{
    dealerMissesUI.gameObject.SetActive(true);

    // Optional: set text content if needed
    dealerMissesUI.SetText("Lucky you! Dealer missed his shot!");

    // Hide it after 2 seconds
    StartCoroutine(HideDealerMissTextAfterDelay(3f));
}

    private IEnumerator HideDealerMissTextAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    dealerMissesUI.gameObject.SetActive(false);
}
    public void ShowGameWin()
    {
        gameWin.SetActive(true);
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

    // public void HitStandActivity(bool parameter)
    // {
    //     hit.gameObject.SetActive(parameter);
    //     stand.gameObject.SetActive(parameter);
    // }
    public void SetHitAndStandActive(bool isActive)
    {
        hit.interactable = isActive;
        stand.interactable = isActive;
    }

    // public void gameOverToggle(bool value)
    // {
    //     gameOver.gameObject.SetActive(value);
    // }
    //create a method which makes hit and stand image's color alpha(transparency) to 50 in stead of hiding it completely
}
