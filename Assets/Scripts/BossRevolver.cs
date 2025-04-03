using UnityEngine;

public class BossRevolver : MonoBehaviour
{
    private int loadedChamber; // Random chamber where the bullet is
    private int currentChamber = 0;
    private int totalChambers = 6;

    public void LoadGun()
    {
        loadedChamber = Random.Range(0, totalChambers); // 0 to 5
        currentChamber = 0;
        Debug.Log($"Bullet is in chamber {loadedChamber}");
    }

    public bool PullTrigger()
    {
        bool fired = currentChamber == loadedChamber;
        Debug.Log($"Boss fires the bullet: {fired}");
        currentChamber = (currentChamber + 1) % totalChambers; // Move to next chamber
        return fired;
    }
}


