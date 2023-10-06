


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

    // Sight
    public float sightRange, attackRange, viewAngle;
    public bool playerInSightRange, playerInAttackRange;

    // Movement
    public float accelerationCoefficent; // how much the speed increases per second
    public float baseSpeed; // speed at t0
    public float secondsToFullSpeed; // how fast the bot can accelerate to full speed

    

   

    private void Awake()
    {
        player = GameObject.Find("First Person Player").transform; 
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        // Check for sight and attack range
        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // TODO: maybe make it chase player based on both sight & sound?
        playerInSightRange = isPlayerInSight();

        // can do lazy updates with this ?

        agent.speed = Time.realtimeSinceStartup * accelerationCoefficent + baseSpeed;
        // match acceleration with speed
        agent.acceleration = agent.speed / secondsToFullSpeed;
       
      

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
        // Debug.Log("Search!");
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(currentGuardTarget.position.x + randomX, transform.position.y, currentGuardTarget.position.z + randomZ);
        // Debug.Log("current:");
        // Debug.Log(transform.position);
        // Debug.Log("target:");
        // Debug.Log(walkPoint);
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
        // Debug.Log("Chase player!");
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

    // checks if the player is in the enemy's FOV
    // function adapted from https://discussions.unity.com/t/how-to-check-if-player-is-inside-enemy-field-of-view-without-rendertexture/244249/2
    private bool isPlayerInSight()
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, toPlayer) <= viewAngle)
        {
            if (Physics.Raycast(transform.position, toPlayer, out RaycastHit hit, sightRange))
            {
                if (hit.transform.root == player.transform)
                {
                    return true;
                }
                    
            }
        }

        return false;

    }

    private bool canMove = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Assuming your player has the tag "Player"
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, transform.position);
            
            if (canMove)
            {
                canMove = false;
                agent.isStopped = true; // Stops the NavMeshAgent from moving
                Invoke("EnableMovement", 2.0f); // Re-enables movement after 5 seconds
            }
        }
    }

    private void EnableMovement()
    {
        agent.isStopped = false; // Allows the NavMeshAgent to move again
        canMove = true;
    }
}
