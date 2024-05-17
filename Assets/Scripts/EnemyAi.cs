using System.Collections;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform safeRoom;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseDistance = 3f;
    public float dragDuration = 3f;
    public float dragGap = 0.5f; // Gap between player and NPC during dragging
    public LayerMask obstacleMask;
    public float avoidanceDistance = 1f; // Distance to check for obstacles

    private Transform target;
    private bool isDragging = false;

    void Start()
    {
        target = GetNextPatrolPoint();
    }

    void Update()
    {
        if (!isDragging)
        {
            if (Vector2.Distance(transform.position, target.position) <= chaseDistance)
            {
                // Chase the player
                target = GameObject.FindGameObjectWithTag("Player").transform;
                transform.position = Vector2.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                // Patrol behavior with obstacle avoidance
                PatrolWithAvoidance();
            }
        }
    }

    void PatrolWithAvoidance()
    {
        // Calculate a movement direction
        Vector2 moveDirection = (target.position - transform.position).normalized;

        // Check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, avoidanceDistance, obstacleMask);

        if (hit.collider != null)
        {
            // If an obstacle is detected, change the movement direction
            moveDirection = Vector2.Reflect(moveDirection, hit.normal);
        }

        // Move the NPC
        transform.Translate(moveDirection * patrolSpeed * Time.deltaTime);

        // Update target if close to the current target point
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            target = GetNextPatrolPoint();
        }
    }

    Transform GetNextPatrolPoint()
    {
        return patrolPoints[Random.Range(0, patrolPoints.Length)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Disable player movement temporarily
            collision.GetComponent<mainplayer>().enabled = false;

            // Start dragging the player to the safe room
            StartCoroutine(DragPlayerToSafeRoom(collision.transform));
        }
    }

    private IEnumerator DragPlayerToSafeRoom(Transform playerTransform)
    {
        isDragging = true;

        Vector3 playerStartPosition = playerTransform.position;
        Vector3 npcStartPosition = transform.position;

        Vector3 dragDirection = (safeRoom.position - npcStartPosition).normalized;
        Vector3 playerTargetPosition = safeRoom.position - dragDirection * dragGap;
        Vector3 npcTargetPosition = safeRoom.position;

        float elapsedTime = 0f;

        while (elapsedTime < dragDuration)
        {
            float t = elapsedTime / dragDuration;

            playerTransform.position = Vector3.Lerp(playerStartPosition, playerTargetPosition, t);
            transform.position = Vector3.Lerp(npcStartPosition, npcTargetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure player and NPC reach their target positions exactly after the drag duration
        playerTransform.position = playerTargetPosition;
        transform.position = npcTargetPosition;

        // Re-enable player movement
        playerTransform.GetComponent<mainplayer>().enabled = true;

        // Resume patrolling after dragging the player
        target = GetNextPatrolPoint();

        isDragging = false;
    }
}