using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
 CharacterController characterController;
    public float knockBackForce = 5f;
    public float punchDamageAmount = 10f; // Damage amount for a punch
    public float kickDamageAmount = 15f; // Damage amount for a kick
    public float radius;
    public AudioSource audioSource;
    public AudioClip punchSound;
    public AudioClip kickSound;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Kick();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Punch();
        }
     
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Punch()
    {
        anim.SetTrigger("Punch");
        audioSource.PlayOneShot(punchSound);
        // Rest of the punch logic...
        HandleCollision(punchDamageAmount);
    }
    
    public void Kick()
    {
        anim.SetTrigger("Kick");
        audioSource.PlayOneShot(kickSound);
        // Rest of the kick logic...
        HandleCollision(kickDamageAmount);
    }

    private void HandleCollision(float damageAmount)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            // Check if the collision is with an enemy
            if (collider.CompareTag("Enemy"))
            {
                // Get the enemy script component
                EnemyHealthBar enemy = collider.GetComponent<EnemyHealthBar>();

                // Check if the enemy script exists
                if (enemy != null)
                {
                    // Call the TakeDamageFromPlayer method on the enemy
                    enemy.TakeDamageFromPlayer(damageAmount);

                    // Update the enemy's health bar slider
                   

                    // Apply knockback force to the enemy
                    Vector3 knockBackDirection = collider.transform.position - transform.position;
                    knockBackDirection.Normalize();
                    knockBackDirection.y = 0f; // Keep the knockback force horizontal
                    enemy.ApplyKnockBack(knockBackDirection * knockBackForce);

                    // Add debug output
                    Debug.Log("Player hit enemy!");
                }
            }
        }
    }

}
