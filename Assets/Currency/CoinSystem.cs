using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    public int totalCoins = 0;
    public CoinDisplay coinDisplay; // Reference to the CoinDisplay script

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        Debug.Log("Total Coins: " + totalCoins);

        // Call the UpdateCoinCountUI method in the CoinDisplay script to update the UI
        coinDisplay.UpdateCoinCountUI();
    }
}
