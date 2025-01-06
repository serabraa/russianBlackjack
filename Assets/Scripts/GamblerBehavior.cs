using UnityEngine;
using UnityEngine.UI;


public class GamblerBehavior : IBossBehavior
{
    private bool hasBluffed = false;
    Card firstCard; // first card of a dealer,(SHOWN ONE)
    
    public void OnDefeated()
    {
        Debug.Log("GAMBLER HAS BEEN DEFEATED");
    }

    public void TakeTurn(GameController gameController)
    {
            if (!hasBluffed)
        {
            Debug.Log("The Gambler bluffs with a fake card!");
            Bluff(gameController);
            hasBluffed = true;
        }
        while(gameController.dealerScore < 17)
        {
            Debug.Log("The Gambler draws card normally");
            gameController.DrawCardForBehavior();
        }
        gameController.CheckState();
    }

    private void Bluff(GameController gameController)
    {
        Debug.Log("The Gambler bluffed!");
        firstCard = gameController.GetShownCard();      // first card
        Card newCard = gameController.JustDrawACard();  // newCard
        Transform firstCardGO = gameController.GetShownCardGO();    //getting first card's GO to change it's sprite to the new one
        Image firstCardImage =firstCardGO.GetComponent<Image>();
        firstCardImage.sprite = newCard.cardImage;  //sprite changed
        Debug.Log(newCard.rank);
        gameController.RecalculateScore(firstCard, newCard);    // and the score is recalculated
    }

}
