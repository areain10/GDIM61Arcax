using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalebAi : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of NPC movement
    public float fieldOfVision = 5f; // Field of vision radius
    public LayerMask playerLayer; // Layer where the player character is placed
    public float catchDistance = 1f; // Distance to catch the player
    public Collider2D safeRoomCollider; // Reference to the safe room Collider2D
    public GameObject[] patrolPointObjects; // Array to store patrol point GameObjects

    private Transform player; // Reference to the player's transform
    private Transform[] patrolPoints; // Array to store patrol points
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
            Debug.Log("Player in range: " + isPlayerInRange);

            if (isPlayerInRange)
            {
                // Check if player is inside the safe room
                bool isPlayerInSafeRoom = IsPlayerInSafeRoom();
                Debug.Log("Player in safe room: " + isPlayerInSafeRoom);

                if (!isPlayerInSafeRoom)
                {
                    // Move towards the player
                    transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                    Debug.Log("Moving towards player");

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
        if (patrolPoints.Length == 0) return;

        // Move towards the next patrol point
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        Debug.Log("Moving towards patrol point: " + targetPoint.name);

        // Check if reached the patrol point
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Move to the next patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
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
        Debug.Log("Patrol points found: " + patrolPoints.Length);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw field of vision radius in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfVision);
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOver");
    }
}
