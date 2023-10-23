


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
    private Transform currentGuardTarget;
    public float guardTime;
    float guardingSince;
    bool isGuarding;


    // Sight
    public float sightRange, attackRange, viewAngle;
    public bool playerInSightRange, playerInAttackRange;

    // Movement
    public float accelerationCoefficent; // how much the speed increases per second
    public float baseSpeed; // speed at t0
    public float secondsToFullSpeed; // how fast the bot can accelerate to full speed
    public float startAccelerationTime;


    // Effects
    private float stunnedSince;
    private float stunDuration = 0;
    private bool isStunned;

    public GameObject stunParticles;
    public GameObject PortalEffect;

    // spawning
    public Transform spawnPoint;
    bool spawned;

    // teleport
    private Vector3 teleportPoint;

    // rage mode
    private bool enraged = false;
    public float enrageSpeed = 15; // speed at which the enemy will become enraged

    private void Awake()
    {
        player = GameObject.Find("First Person Player").transform; 
        agent = GetComponent<NavMeshAgent>();
        stunParticles.SetActive(false);
        ResetSpeed();
        spawned = false;
    }

    private void Update()
    {
        UpdateEffects();
        // handle stun
        if (isStunned || !spawned)
        {
            return; //skip all actions
            
        }

        // Check for sight and attack range
        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // TODO: maybe make it chase player based on both sight & sound?
        playerInSightRange = isPlayerInSight();

        // can do lazy updates with this ?

        agent.speed = (Time.realtimeSinceStartup - startAccelerationTime) * accelerationCoefficent + baseSpeed;
        // match acceleration with speed
        agent.acceleration = agent.speed / secondsToFullSpeed;

        if(agent.speed > enrageSpeed && !enraged){
            Enrage();
        }
       

        if(enraged){
            // rage mode, always chase player
            ChasePlayer();

        }
        else{
            // normal mode, patrol and chase player on sight
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

        }
        
     
        //if (playerInAttackRange && playerInSightRange) AttackPlayer();


    }

    // teleports to the point set
    private void Teleport(){
        agent.Warp(teleportPoint);
        PortalEffect.transform.position = new Vector3 (0,0,0); // reset portal position
    }

    // in {seconds} seconds, teleport to the player's current position
    public void TeleportBehindPlayer(float seconds){
        
        if(spawned){
            PortalEffect.transform.position = player.position;
            teleportPoint = player.position;
            Invoke("Teleport", seconds);
             
        };
        
    }

    public void Enrage(){
        if(!enraged){
            enraged = true;
            TeleportBehindPlayer(2);
        }
        
    }

    public void Spawn(){
        if(!spawned){
            Debug.Log(agent.Warp(spawnPoint.position));
            spawned = true;
            Debug.Log("Enemy Spawned!");

            //TeleportBehindPlayer(5);
        }
    }

    // Stuns the enemy, allows extending 
    public void Stun(float seconds)
    {
        if (!isStunned)
        {
            isStunned = true;
            stunnedSince = Time.realtimeSinceStartup;
        }

        enraged = false;
        stunDuration += seconds;
    }

    // resets the speed to the initial value
    public void ResetSpeed(){
        startAccelerationTime = Time.realtimeSinceStartup;
    }

    // handle all potential effects
    private void UpdateEffects()
    {
        if (isStunned) {
            if (Time.realtimeSinceStartup - stunnedSince > stunDuration)
            {
                isStunned = false;
                stunDuration = 0;
                stunParticles.SetActive(false);
            }
            else
            {
                agent.speed = 0;
                stunParticles.SetActive(true);
                stunParticles.transform.position = transform.position;
            }

        }
        
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
    
        Transform target = guardTargets[Random.Range(0, guardTargets.Count)];
        SetGuardTarget(target);
        
    }

    public void SetGuardTarget(Transform target){
        currentGuardTarget = target;
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

            Stun(5.0f); // this is not as elegant, but more flexible?
            /* 
            if (canMove)
            {
                canMove ＝ false；
                agent.isStopped = true; // Stops the NavMeshAgent from moving
                Invoke("EnableMovement", 2.0f); // Re-enables movement after 5 seconds
            }
            */
        }
    }

    private void EnableMovement()
    {
        agent.isStopped = false; // Allows the NavMeshAgent to move again
        canMove = true;
    }

}
