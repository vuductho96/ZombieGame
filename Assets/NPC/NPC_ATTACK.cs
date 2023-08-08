using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_ATTACK : MonoBehaviour
{
    [Header("------------ATTACK----------")]
    public string stringtag = "Enemy";
    public float DetectionRange = 20f;
    public float StopDistaneWithEnemy = 3f;
    public float AttackCoolDown = 3f;
    public Transform AttackPoint;
    public float AttackRange = 100f;
    Animator anim;
    public float DameAmount = 50f;
    private float lastAttackTime;

    [Header("-----------FollowPlayer---------")]

    public Transform playerTransform; // Reference to the player GameObject
    private NavMeshAgent agent;
    public float StopBeforePlayerDistance = 2f;
    public float RotateSpeed = 20f;

    [Header("--------Effect----------")]
    public AudioClip Attack;
    AudioSource audio;
    public ParticleSystem FireBreath;
    private void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        FireBreath.Stop();
       
    }

    private void Update()
    {
       

        FollowPlayer(); // Always move towards the player
        RangeATTACK(); // Attack enemies in range
       
    }
   
    private void FollowPlayer()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("Player transform reference is missing!");
            return;
        }

        // Calculate the distance between the character and the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // If the distance to the player is greater than the stopping distance, move towards the player
        if (distanceToPlayer > StopBeforePlayerDistance)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // Move the character towards the player
            agent.SetDestination(playerTransform.transform.position);

            // Trigger the animation (assuming anim references the Animator component)
            anim.SetFloat("NPC", 1f);
        }
        else
        {
            //reset the animation parameter
            anim.SetFloat("NPC", 0f);
        }
    }

    private void RangeATTACK()
    {
        audio.PlayOneShot(Attack);
        FireBreath.Play();
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, DetectionRange);

        foreach (Collider enemy in hitEnemies)
        {
            if (!enemy.CompareTag(stringtag))
                continue;

            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            float distanceToEnemy = directionToEnemy.magnitude;
            if (distanceToEnemy > StopDistaneWithEnemy || Time.time < lastAttackTime + AttackCoolDown)
            {
                agent.isStopped = false;
                continue;
            }

            agent.isStopped = true;
            anim.SetTrigger("Kamezoko");
            lastAttackTime = Time.time;

            // Adjust the raycast origin point and direction
            Vector3 raycastOrigin = AttackPoint.position;
            Vector3 raycastDirection = directionToEnemy.normalized;

            RaycastHit hitInfo;
            if (Physics.Raycast(raycastOrigin, raycastDirection, out hitInfo, AttackRange))
            {
                if (hitInfo.collider.CompareTag(stringtag))
                {
                    Debug.Log("Raycast hit enemy: " + hitInfo.collider.name);

                    EnemyHealthBar enemyHealth = hitInfo.collider.GetComponent<EnemyHealthBar>();
                    enemyHealth?.TakeDamageFromPlayer(DameAmount);
                }
            }

            Debug.DrawRay(raycastOrigin, raycastDirection * AttackRange, Color.red, 1f);
        }
    }


}
