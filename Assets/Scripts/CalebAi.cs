using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
//using UnityEngine.AI.NavMesh.SamplePosition;

public class CalebAi : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of NPC movement
    public float fieldOfVision = 5f; // Field of vision radius
    public LayerMask playerLayer; // Layer where the player character is placed
    public float catchDistance = 1f; // Distance to catch the player
    public Collider2D safeRoomCollider; // Reference to the safe room Collider2D
    public GameObject[] patrolPointObjects; // Array to store patrol point GameObjects
    public float walkRadius;
    public float tiredSeconds = 5f;
    public SpriteRenderer sr;


    private Transform player; // Reference to the player's transform
    private Transform[] patrolPoints; // Array to store patrol points
    private int currentPatrolIndex = 0; // Index of current patrol point
    private SafeRoomManager safeRoomManager; // Reference to the SafeRoomManager
    NavMeshAgent agent;
    private float timer;
    private bool tired;
    private Rigidbody2D rb;
    private float sizeX;

    private void Start()
    {
        sizeX = transform.localScale.x;
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find player
        safeRoomManager = FindObjectOfType<SafeRoomManager>(); // Find the SafeRoomManager
        FindPatrolPoints(); // Find patrol points
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        tired = false;
        timer = tiredSeconds;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        handleFlip();
        if (player != null)
        {
            // Check if player is inside the safe room
            bool isPlayerInSafeRoom = IsPlayerInSafeRoom();

            if (!isPlayerInSafeRoom)
            {
                // Check if player is within field of vision
                bool isPlayerInRange = Physics2D.OverlapCircle(transform.position, fieldOfVision, playerLayer);

                if (isPlayerInRange && !tired)
                {
                    
                    if(timer>0)
                    {
                        timer -= Time.deltaTime;
                    }
                    else
                    {
                        sr.color = Color.red;
                        tired = true;
                        Patrol();
                    }
                    // Move towards the player
                    //transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                    agent.speed = 2.7f;
                    agent.SetDestination(player.transform.position);
                    // Check if NPC caught the player
                    float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                    if (distanceToPlayer < catchDistance)
                    {
                        // Handle player caught
                        PlayerCaught();
                    }
                }
                else
                {

                    // Roam around the halls
                    Patrol();
                    
                }
            }
            else
            {
                // Player is in the safe room, return to patrolling
                Patrol();
            }
            if(tired)
            {
                if(timer < tiredSeconds)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    sr.color = Color.white;
                    tired = false;
                }
            }
        }
    }
    void handleFlip()
    {
        float dotProduct = Vector2.Dot(Vector2.left, agent.velocity);
        //Debug.Log(rb.velocity.magnitude);
        if(dotProduct < 0)
        {
            gameObject.transform.localScale = new Vector3(-sizeX, gameObject.transform.localScale.y);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(sizeX, gameObject.transform.localScale.y);
        }
    }
    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        agent.speed = 1.5f;
        // Move towards the next patrol point
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        //transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        agent.SetDestination(targetPoint.position);
        //Debug.Log(Vector2.Distance(transform.position, targetPoint.position));
        // Check if reached the patrol point
        if (Vector2.Distance(transform.position, targetPoint.position) < 1f)
        {
            Debug.Log("Reach Destination");
            // Move to the next patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
        /*Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        
        NavMesh.SamplePosition (randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;
        agent.SetDestination(finalPosition);*/

    }

    private bool IsPlayerInSafeRoom()
    {
        // Check if player's collider is overlapping with the safe room collider
        if (safeRoomCollider != null && player != null)
        {
            return safeRoomCollider.OverlapPoint(player.position);
        }
        return false;
    }

    private void FindPatrolPoints()
    {
        // Convert patrol point GameObjects to their transforms
        patrolPoints = new Transform[patrolPointObjects.Length];
        for (int i = 0; i < patrolPointObjects.Length; i++)
        {
            patrolPoints[i] = patrolPointObjects[i].transform;
        }
    }

    private void PlayerCaught()
    {
        if (safeRoomManager.AreAllNpcsKilled())
        {
            GameOver();
        }
        else
        {
            safeRoomManager.KillNpc();

            // Update the number of killed NPCs in PlayerPrefs
            int killedNpcs = PlayerPrefs.GetInt("KilledNpcs", 0);
            PlayerPrefs.SetInt("KilledNpcs", killedNpcs + 1);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw field of vision radius in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfVision);
    }

    private void GameOver()
    {
        // Clear the killed NPCs count
        PlayerPrefs.DeleteKey("KilledNpcs");

        // You can implement your game over logic here
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOver"); // Load the game over scene
    }
}
