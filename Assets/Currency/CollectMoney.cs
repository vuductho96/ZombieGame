using UnityEngine;

public class CollectMoney : MonoBehaviour
{
    // The amount of money to add when collected
    public int moneyAmount = 1;
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip coinSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the CoinSystem component from the player object
            CoinSystem playerManager = other.GetComponent<CoinSystem>();

            // Play the coin sound using the AudioSource component
            if (audioSource != null && coinSound != null)
            {
                audioSource.PlayOneShot(coinSound);
            }

            // If the CoinSystem component is found, add the moneyAmount to the player's total coins
            if (playerManager != null)
            {
                playerManager.AddCoins(moneyAmount);
            }

            // Deactivate the money object after the player collects it
            gameObject.SetActive(false);
        }
    }
}
