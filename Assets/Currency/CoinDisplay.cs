using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    public CoinSystem coinSystem;
    public TextMeshProUGUI coinCountText;

    private void Start()
    {
        UpdateCoinCountUI();
    }

    public void UpdateCoinCountUI()
    {
        // Get the current coin count from the CoinSystem script
        int currentCoins = coinSystem.totalCoins;

        // Update the TMP Text to display the current coin count
        coinCountText.text = "Coins: " + currentCoins.ToString();
    }
}
