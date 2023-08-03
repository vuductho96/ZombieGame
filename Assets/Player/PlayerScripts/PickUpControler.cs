using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpControler : MonoBehaviour
{
    public GameObject AxeObject;
    public GameObject TargetAxe;
    private string axeTag = "Axe";
    public GameObject PickUpText;
    private GameObject currentWeapon; // Reference to the currently active weapon
    private bool hasPickedUpAxe = false; // Track if the player has already picked up the axe
    private bool weaponInRange;

    private void Start()
    {
        PickUpText.SetActive(false);
        TargetAxe.SetActive(false);
        currentWeapon = AxeObject; // Set the initial weapon as the default one
    }

    private void OutOfRange()
    {
        PickUpText.SetActive(false);
        weaponInRange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !hasPickedUpAxe && weaponInRange)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2.0f); // Adjust the radius as needed
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(axeTag))
                {
                    PickUp(collider.gameObject);
                    hasPickedUpAxe = true; // Set the flag to indicate that the player has picked up the axe
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && hasPickedUpAxe)
        {
            Drop();
            hasPickedUpAxe = false; // Reset the flag since the player has dropped the axe
        }
    }

    private void PickUp(GameObject axe)
    {
        currentWeapon.SetActive(false); // Deactivate the currently active weapon
        currentWeapon = axe; // Set the new weapon as the currently active one
        TargetAxe.SetActive(true);
        PickUpText.SetActive(false); // Deactivate the pick-up text when the axe is picked up
        Debug.Log("Picked up the axe");
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

    private void Drop()
    {
        TargetAxe.SetActive(false);
        currentWeapon.SetActive(true); // Activate the previously dropped weapon
        PickUpText.SetActive(false);

        // Calculate the drop position with an offset of 0.5f
        Vector3 dropPosition = transform.position + transform.forward * 2.0f; // Adjust the distance as needed
        dropPosition.y = GetGroundHeight(dropPosition) + 0.5f; // Add an offset of 0.5f

        currentWeapon.transform.position = dropPosition;

        Debug.Log("Dropped the axe");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag(axeTag) && !hasPickedUpAxe)
        {
            PickUpText.SetActive(true);
            weaponInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(axeTag))
        {
            OutOfRange();
        }
    }
}
