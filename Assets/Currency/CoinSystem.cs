using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public bool TryPurchase(int cost)
    {
        if (totalCoins >= cost)
        {
            totalCoins -= cost;
            coinDisplay.UpdateCoinCountUI();
            return true; // Purchase successful
        }
        else
        {
            Debug.Log("Insufficient funds to purchase.");
            return false; // Purchase failed
        }
    }
}

public class HireNpc2 : MonoBehaviour
{
    public int npcHireCost = 50; // Set the cost to hire an NPC
    public CoinSystem coinSystem;
    public GameObject npcObject; // Reference to the NPC GameObject
    private NavMeshAgent navMeshAgent; // Reference to the NavMesh Agent component of the NPC

    private bool isInRange = false;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        navMeshAgent = npcObject.GetComponent<NavMeshAgent>(); // Get the NavMeshAgent from the NPC GameObject
        navMeshAgent.enabled = false; // Disable the NavMeshAgent initially
    }

    private void Update()
    {
        // Check for player input when in range of an NPC
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryHireNpc();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("NPC"))
        {
            isInRange = true;
            Debug.Log("Player entered NPC's interaction range. Press 'E' to hire the NPC.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isInRange = false;
            Debug.Log("Player exited NPC's interaction range.");
        }
    }

    public void TryHireNpc()
    {
        if (coinSystem.TryPurchase(npcHireCost))
        {
            // Activate the NavMeshAgent component when the NPC is hired
            navMeshAgent.enabled = true;

            // Perform additional actions to hire the NPC.
            Debug.Log("NPC hired!");
        }
        else
        {
            Debug.Log("Not enough coins to hire NPC.");
        }
    }
}
