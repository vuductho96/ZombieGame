using UnityEngine;
using UnityEngine.AI;

public class HireNpc : MonoBehaviour
{
    public int npcHireCost = 50; // Set the cost to hire an NPC
    public CoinSystem coinSystem;
    public GameObject npcObject; // Reference to the NPC GameObject
    private NavMeshAgent navMeshAgent; // Reference to the NavMesh Agent component of the NPC
    public GameObject hire;
    private bool isInRange = false;
    private CharacterController characterController;
    private bool isHiring;

    private bool hasHired = false; // Flag to track if the player has already hired the NPC

    private void Start()
    {
        hire.SetActive(false); // Initially, the hire object is turned off
        characterController = GetComponent<CharacterController>();
        navMeshAgent = npcObject.GetComponent<NavMeshAgent>(); // Get the NavMeshAgent from the NPC GameObject
        navMeshAgent.enabled = false; // Disable the NavMeshAgent initially
    }

    private void Update()
    {
        // Check for player input when in range of an NPC and not already hired
        if (isInRange && !hasHired && Input.GetKeyDown(KeyCode.E))
        {
            TryHireNpc(); // Attempt to hire the NPC when the player presses 'E'
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("NPC"))
        {
            isInRange = true; // Player is in range of an NPC
            if (!hasHired) hire.SetActive(true); // Turn on the hire object to indicate interaction if not already hired
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isInRange = false; // Player is no longer in range of an NPC
            hire.SetActive(false); // Turn off the hire object
            Debug.Log("Player exited NPC's interaction range.");
        }
    }

    public void TryHireNpc()
    {
        if (!hasHired && coinSystem.TryPurchase(npcHireCost))
        {
            // Activate the NavMeshAgent component when the NPC is hired
            navMeshAgent.enabled = true;
            hasHired = true; // Set the flag that the player has hired the NPC

            hire.SetActive(false); // Deactivate the hire object

            // Other logic related to hiring the NPC
        }
        else
        {
            Debug.Log("Not enough coins to hire NPC.");
            hire.SetActive(false); // Deactivate the hire object
        }
    }
}
