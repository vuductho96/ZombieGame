using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chase : MonoBehaviour
{
    [Header("------------------Chase and Patrol --------------------")]
    public Transform[] waypoints;
    public Transform target;
    public float chaseDistance = 10f;
    public float returnCooldown = 5f;
    private Animator anim;
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isChasing = false;
    private bool isCooldown = false;
    private float cooldownTimer = 0f;
    private bool isReturnCooldown = false;
    private float returnCooldownTimer = 0f;
    private bool isStop = false;
    private float waitingTime = 2f;
    private float ChaseStopDistance = 2f;// local variable;
    private float attackCooldown;
    private const float DefaultStopDistance = 0.1f;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        StartCoroutine(PatrolCoroutine());
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (!isCooldown && distanceToTarget <= chaseDistance)
        {
            isChasing = true;
            Chase();
        }
        else if (!isChasing && !isCooldown && !isReturnCooldown)
        {
            // Do nothing, let the coroutine handle patrol movement
        }

        if (isChasing && distanceToTarget > chaseDistance)
        {
            StartCooldown();
        }

        if (isReturnCooldown)
        {
            UpdateReturnCooldown();
        }
        else
        {
            UpdateCooldown();
        }
    }

    private void Chase()
    {
        float currentStopDistance = ChaseStopDistance;
        anim.SetFloat("BOSS", 1f);
        agent.SetDestination(target.position);
        agent.stoppingDistance = currentStopDistance;
        isStop = false;

        BossMelleAttack();
    }

    private void StartCooldown()
    {
        isChasing = false;
        isCooldown = true;
        cooldownTimer = returnCooldown;
        agent.ResetPath();
        anim.SetFloat("BOSS", 0f);
        isReturnCooldown = true;
        returnCooldownTimer = returnCooldown;
        isStop = true;
    }

    private void UpdateCooldown()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                isReturnCooldown = true;
                returnCooldownTimer = returnCooldown;
            }
        }
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void UpdateReturnCooldown()
    {
        returnCooldownTimer -= Time.deltaTime;
        if (returnCooldownTimer <= 0f)
        {
            isReturnCooldown = false;
            StartCoroutine(PatrolCoroutine()); // Start patrolling again after return cooldown
        }
    }

    private IEnumerator PatrolCoroutine()
    {
        // Set the stopping distance to the default value
        agent.stoppingDistance = DefaultStopDistance;

        while (true)
        {
            Vector3 waypointPosition = waypoints[currentWaypointIndex].position;
            Debug.Log("Moving to waypoint " + currentWaypointIndex + " at position " + waypointPosition);

            agent.SetDestination(waypointPosition);
            anim.SetFloat("BOSS", 0.5f);
            isStop = false;

            yield return new WaitForSeconds(0.1f);

            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.5f);

            Debug.Log("Reached waypoint " + currentWaypointIndex);

            // Add waiting time before moving to the next waypoint
            float waitingTime = 2.0f; // Adjust this value as needed
            Debug.Log("Waiting for " + waitingTime + " seconds before moving to the next waypoint");
            anim.SetFloat("BOSS", 0f, 0.00001f, Time.deltaTime); // Transition to idle state
            yield return new WaitForSeconds(waitingTime);

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void BossMelleAttack()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= ChaseStopDistance && attackCooldown <= 0f)
            {
                int randomAttack = Random.Range(1, 5); // Generate a random number between 1 and 4

                switch (randomAttack)
                {
                    case 1:
                        anim.SetTrigger("Melle1");
                        break;
                    case 2:
                        anim.SetTrigger("Melle2");
                        break;
                    case 3:
                        anim.SetTrigger("Melle3");
                        break;
                    case 4:
                        anim.SetTrigger("Melle4");
                        break;
                }

                attackCooldown = 2f; // Set the cooldown time
            }
        }




    }
}






