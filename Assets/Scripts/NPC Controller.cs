using UnityEngine;
using NavMeshPlus;

public class NPCController : MonoBehaviour
{
    // Flag to determine if the NPC is in the roaming phase
    private bool isRoaming = false;

    // Reference to the safe room collider
    public Collider2D safeRoomCollider;

    // Speed of the NPC's movement
    public float moveSpeed = 3f;

    // Target position for roaming
    private Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Called when another Collider2D enters this NPC's trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to the player
        if (collision.CompareTag("Player"))
        {
            // Trigger roaming behavior in the NPC
            StartRoaming();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the NPC is in the roaming phase
        if (isRoaming)
        {
            // Move towards the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if the NPC has reached the target position
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Choose a new random target position
                SetRandomTargetPosition();
            }
        }
    }

    // Function to trigger roaming behavior
    private void StartRoaming()
    {
        isRoaming = true;
        // Set the initial target position
        SetRandomTargetPosition();
    }

    // Function to set a random target position outside the safe room bounds
    private void SetRandomTargetPosition()
    {
        // Get the bounds of the safe room collider
        Bounds bounds = safeRoomCollider.bounds;

        // Find a random point outside the bounds of the safe room
        Vector2 randomPoint = Vector2.zero;
        do
        {
            randomPoint = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
        } while (safeRoomCollider.bounds.Contains(randomPoint));

        // Set the target position to the random point
        targetPosition = randomPoint;
    }
}
