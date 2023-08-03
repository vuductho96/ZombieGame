using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount = 20f;
    public float lifetime = 5f; // Lifetime of the projectile
    public float moveSpeed = 10f; // Speed at which the projectile moves
    public bool isHoming = true; // If true, the projectile will home in on a target (if set)
    // Optional hit effect (e.g., particle effect or explosion)
    private Transform target; // The target enemy's transform (if isHoming is true)
    private bool isLaunched; // Track if the projectile is already launched
   
    // Set the target enemy for the projectile to move towards (if isHoming is true)
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnEnable()
    {
        // Reset the isLaunched flag when the projectile is reused
        isLaunched = false;
        // Destroy the projectile after its lifetime expires
        Invoke("DisableProjectile", lifetime);
    }
    private void Start()
    {
      
    }
    private void Update()
    {
        if (isLaunched)
            return; // Exit if the projectile is already launched

        if (isHoming && target != null)
        {
            // Move the projectile towards the target's position
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Move the projectile in its current forward direction
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
      
        // Check if the collision is with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
           

            // Handle the collision with the enemy, apply damage, or perform other actions.
            EnemyHealthBar enemyHealth = collision.transform.GetComponent<EnemyHealthBar>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamageFromPlayer(damageAmount);
            }
        }

        // Disable the projectile after hitting a target or colliding with any object
        DisableProjectile();
    }

    private void DisableProjectile()
    {
        // Disable the projectile and reset its position and direction for reuse
        isLaunched = false;
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
