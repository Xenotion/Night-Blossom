


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public List<Transform> guardTargets;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Guarding
    public Transform currentGuardTarget;
    public float guardTime;
    float guardingSince;
    bool isGuarding;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Movement
    public float accelerationCoefficent;
    public float baseSpeed;

    private void Awake()
    {
        player = GameObject.Find("First Person Player").transform; 
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        // can do lazy updates with this
        agent.speed = Time.realtimeSinceStartup * accelerationCoefficent + baseSpeed;
        // TODO: update acceleration as well?

        if (Time.realtimeSinceStartup - guardingSince > guardTime )
        {
            isGuarding = false; // guard new target

        }
        if (playerInSightRange )
        {
            ChasePlayer(); // priority
        }
        else
        {
            Patroling();
        }
     
        //if (playerInAttackRange && playerInSightRange) AttackPlayer();

     
    }

    private void Patroling()
    {
        //Debug.Log("Patrolling!");
        if (!isGuarding) FindGuardTarget();
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet && isGuarding)
        {
            agent.SetDestination(walkPoint);
        }
            

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        Debug.Log("Search!");
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(currentGuardTarget.position.x + randomX, transform.position.y, currentGuardTarget.position.z + randomZ);
        Debug.Log("current:");
        Debug.Log(transform.position);
        Debug.Log("target:");
        Debug.Log(walkPoint);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void FindGuardTarget()
    {
        // TODO: can change to based on distance
    
        currentGuardTarget = guardTargets[Random.Range(0, guardTargets.Count)];
        isGuarding= true;
        walkPointSet = false; // reset walkPoint
        guardingSince = Time.realtimeSinceStartup;
    }

    private void ChasePlayer()
    {
        Debug.Log("Chase player!");
        //walkPoint = player.transform.position;
        walkPointSet= false;
        isGuarding= false;

        // look at player?
        //transform.LookAt(player);

        agent.SetDestination(player.transform.position);
    }

    // we might add attacks later
    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
