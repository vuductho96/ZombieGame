using UnityEngine;

public class SwordAttack2 : MonoBehaviour
{
    public float damageAmount = 20f;
    public float radius = 1.5f;
    private float knockBackForce = 5f;
    public Animator anim;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            HandleCollision(damageAmount);
        }
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
                    enemy.UpdateHealth(enemy.currentHealth);

                    // Apply knockback force to the enemy
                    Vector3 knockBackDirection = collider.transform.position - transform.position;
                    knockBackDirection.Normalize();
                    knockBackDirection.y = 0f; // Keep the knockback force horizontal
                    enemy.ApplyKnockBack(knockBackDirection * knockBackForce);

                    // Activate the sword attack animation
                    anim.SetTrigger("SwordAttck");

                    // Add debug output
                    Debug.Log("Player hit enemy!");
                }
            }
        }
    }
}
