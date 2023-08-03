using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class NpcCODE : MonoBehaviour
{
    public AudioClip projectileFiringSound;
    public ParticleSystem Fire;
    private string enemyTag = "Enemy"; // Tag of the enemy GameObjects
    public float stopDistance = 2.0f; // The distance at which the NPC will stop moving
    public float attackCooldown = 5.0f; // The time between consecutive attacks
    public Transform attackPoint; // The point from which the NPC will fire the ranged attack
    public GameObject Projectile; // Reference to the projectile prefab to be spawned
    private float projectileSpeed = 10f;
    public float attackRange = 5f;
    public float detectionRange = 10f; // The range within which the NPC detects enemies
    public Transform player; // Reference to the player
    public float playerStopDistance = 2.5f;
    private Transform target; // Reference to the target enemy
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    private Animator animator;
    private float lastAttackTime;
    private bool isHired = false;
    private enum NpcState
    {
        FollowPlayer,
        AttackEnemy
    }

    private NpcState currentState;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Projectile.SetActive(false);
        Fire.Stop();

        currentState = NpcState.FollowPlayer; // Start by following the player
    }

    private void Update()
    {
        if (isHired)
        {
            // Stay idle when the NPC is hired
            animator.SetFloat("NPC", 0f, 0.1f, Time.deltaTime);
            agent.SetDestination(transform.position); // Stop the NavMeshAgent
            return;
        }

        if (currentState == NpcState.FollowPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > playerStopDistance)
            {
                // Move towards the player
                agent.SetDestination(player.position);
                animator.SetFloat("NPC", 1f, 0.1f, Time.deltaTime);
            }
            else
            {
                // Stop moving and idle when close to the player
                agent.SetDestination(transform.position);
                animator.SetFloat("NPC", 0f, 0.1f, Time.deltaTime);
            }

            // Check for enemy detection within the detection range
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask(enemyTag));
            if (enemiesInRange.Length > 0)
            {
                currentState = NpcState.AttackEnemy; // Switch to attacking enemy state
                target = enemiesInRange[0].transform; // Set the enemy as the target
            }
        }
        else if (currentState == NpcState.AttackEnemy)
        {
            // Check if we have a valid target
            if (target != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // Handle movement and attack
                if (distanceToTarget > stopDistance)
                {
                    // Move towards the target enemy
                    agent.SetDestination(target.position);
                    animator.SetFloat("NPC", 1f);
                }
                else
                {
                    // Stop moving when close enough to the target
                    agent.SetDestination(transform.position);
                    animator.SetFloat("NPC", 0f);

                    // Check if enough time has passed since the last attack
                    if (Time.time - lastAttackTime >= attackCooldown)
                    {
                        RangedAttack();

                        // Update the last attack time
                        lastAttackTime = Time.time;
                    }
                }
            }
            else
            {
                currentState = NpcState.FollowPlayer; // Switch back to following player state
            }
        }
    }

    private void RangedAttack()
    {
        Fire.Play();
        // Implement the ranged attack logic here
        Projectile.SetActive(true);
        animator.SetTrigger("Kamezoko");

        // Play the projectile firing sound
        if (projectileFiringSound != null)
        {
            AudioSource.PlayClipAtPoint(projectileFiringSound, attackPoint.position);
        }

        // Rotate towards the target before attacking
        Vector3 direction = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Instantiate the projectile at the attack point position with the same rotation as the NPC
        GameObject newProjectile = Instantiate(Projectile, attackPoint.position, transform.rotation);

        // Access the Rigidbody component of the new projectile
        Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();

        // Apply force to the projectile to shoot it
        projectileRigidbody.AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);

        // Debug: Draw a line to visualize the projectile's path (from attackPoint to the position the projectile will hit)
        Vector3 hitPosition = attackPoint.position + direction.normalized * attackRange;
        Debug.DrawLine(attackPoint.position, hitPosition, Color.red, 2f);
    }
}
