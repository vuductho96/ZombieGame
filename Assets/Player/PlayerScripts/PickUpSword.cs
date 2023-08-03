using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSword : MonoBehaviour
{
    public GameObject SwordObject;
    public GameObject TargetSword;
    private string swordTag = "Sword";
    public GameObject PickUpText;
    private GameObject currentWeapon; // Reference to the currently active weapon
    private bool hasPickedUpSword = false; // Track if the player has already picked up the sword
    private bool weaponInRange;

    private void Start()
    {
        PickUpText.SetActive(false);
        TargetSword.SetActive(false);
        currentWeapon = SwordObject; // Set the initial weapon as the default one
    }

    private void OutOfRange()
    {
        PickUpText.SetActive(false);
        weaponInRange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && weaponInRange)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2.0f); // Adjust the radius as needed
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(swordTag))
                {
                    if (hasPickedUpSword)
                    {
                        Drop(); // Drop the current sword before picking up the new one
                    }
                    PickUp(collider.gameObject);
                    hasPickedUpSword = true; // Set the flag to indicate that the player has picked up the sword
                    break;
                }
              

                
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && hasPickedUpSword)
        {
            Drop();
            hasPickedUpSword = false; // Reset the flag since the player has dropped the sword
        }
    }

    private void PickUp(GameObject sword)
    {
        if (!sword.CompareTag(swordTag))
        {
            Debug.Log("Cannot pick up object. It is not a sword.");
            return;
        }

        currentWeapon.SetActive(false); // Deactivate the currently active weapon
        currentWeapon = sword; // Set the new weapon as the currently active one
        TargetSword.SetActive(true);
        PickUpText.SetActive(false); // Deactivate the pick-up text when the sword is picked up
        Debug.Log("Picked up the sword");

        // Disable the collider of the picked up sword
        Collider swordCollider = sword.GetComponent<Collider>();
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }

    private void Drop()
    {
        TargetSword.SetActive(false);
        currentWeapon.SetActive(true); // Activate the previously dropped weapon
        PickUpText.SetActive(false);

        // Calculate the drop position with an offset of 0.5f
        Vector3 dropPosition = transform.position + transform.forward * 2.0f; // Adjust the distance as needed
        dropPosition.y = GetGroundHeight(dropPosition) + 0.5f; // Add an offset of 0.5f

        currentWeapon.transform.position = dropPosition;

        // Enable the collider of the dropped sword
        Collider swordCollider = currentWeapon.GetComponent<Collider>();
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }

        Debug.Log("Dropped the sword");
    }

    private float GetGroundHeight(Vector3 position)
    {
        RaycastHit hit;
        float groundHeight = 0f;
        if (Physics.Raycast(position, Vector3.down, out hit))
        {
            groundHeight = hit.point.y;
        }
        return groundHeight;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag(swordTag) && !hasPickedUpSword)
        {
            PickUpText.SetActive(true);
            weaponInRange = true;
        }
        else if (hit.collider.CompareTag(swordTag))
        {
            PickUpText.SetActive(false);
            weaponInRange = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(swordTag))
        {
            OutOfRange();
        }
    }
}
