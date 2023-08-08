using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AXEATTACK : MonoBehaviour
{
    public float damageAmount = 10f; // Adjust the damage value as needed
    public LayerMask playerLayer;    // Set this in the Inspector to the player's layer
    public Vector3 raycastDirection = Vector3.forward; // Manual raycast direction

    private void Update()
    {
        // Convert the local raycastDirection to world space based on the axe's rotation
        Vector3 worldRaycastDirection = transform.TransformDirection(raycastDirection);

        // Create a ray from the axe's position in the specified direction
        Ray ray = new Ray(transform.position, worldRaycastDirection);

        RaycastHit hitInfo;

        // Check if the ray hits something within a specified distance
        if (Physics.Raycast(ray, out hitInfo, 1f, playerLayer))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                Debug.Log("Axe hit the player using raycast!");
                PlayerHealthBar1 playerHealthBar = hitInfo.collider.GetComponent<PlayerHealthBar1>();
                if (playerHealthBar != null)
                {
                    playerHealthBar.TakeDamageFromEnemy(damageAmount);
                }
            }
        }

        // Visualize the ray in the scene view
        Debug.DrawRay(ray.origin, ray.direction * 1f, Color.red);
    }
}
