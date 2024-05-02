using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalebAi : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of NPC movement
    public float fieldOfVision = 5f; // Field of vision radius
    public LayerMask playerLayer; // Layer where the player character is placed
    public float catchDistance = 1f; // Distance to catch the player
    public GameObject safeRoom; // Reference to the safe room GameObject

    private Transform player; // Reference to the player's transform
    [SerializeField] private Vector2[] patrolPoints; // Array to store patrol points
    private int currentPatrolIndex = 0; // Index of current patrol point

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find player
        FindPatrolPoints(); // Find patrol points
    }

    private void Update()
    {
        if (player != null)
        {
            // Check if player is within field of vision
            bool isPlayerInRange = Physics2D.OverlapCircle(transform.position, fieldOfVision, playerLayer);

            if (isPlayerInRange)
            {
                // Check if player is inside the safe room
                bool isPlayerInSafeRoom = IsPlayerInSafeRoom();

                if (!isPlayerInSafeRoom)
                {
                    // Move towards the player
                    transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                    // Check if NPC caught the player
                    float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                    if (distanceToPlayer < catchDistance)
                    {
                        // End the game or take appropriate action
                        GameOver();
                    }
                }
            }
            else
            {
                // Roam around the halls
                Patrol();
            }
        }
    }

    private void Patrol()
    {
        // Move towards the next patrol point
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolIndex], moveSpeed * Time.deltaTime);

        // Check if reached the patrol point
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolIndex]) < 0.1f)
        {
            // Move to the next patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private bool IsPlayerInSafeRoom()
    {
        // Check if player is inside the safe room
        if (safeRoom != null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(player.position, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject == safeRoom)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void FindPatrolPoints()
    {
        // You can set up patrol points manually or programmatically based on your level design
        // For simplicity, let's assume we have two patrol points
        patrolPoints = new Vector2[2];
        patrolPoints[0] = transform.position + new Vector3(5f, 0f); // Example patrol point 1
        patrolPoints[1] = transform.position - new Vector3(5f, 0f); // Example patrol point 2
    }

    private void OnDrawGizmosSelected()
    {
        // Draw field of vision radius in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfVision);
    }

    private void GameOver()
    {
        // You can implement your game over logic here
        Debug.Log("Game Over!");
        // For example, you can reload the scene or display a game over screen
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}