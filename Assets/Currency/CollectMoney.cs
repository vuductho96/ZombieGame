using UnityEngine;

public class CollectMoney : MonoBehaviour
{
    // The amount of money to add when collected
    public int moneyAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the PlayerManager component from the player object
          CoinSystem playerManager = other.GetComponent<CoinSystem>();

            // If the PlayerManager component is found, add the moneyAmount to the player's total coins
            if (playerManager != null)
            {
                playerManager.AddCoins(moneyAmount);
            }

            // Deactivate the money object after the player collects it
            gameObject.SetActive(false);
        }
    }
}
