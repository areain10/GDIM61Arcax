using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates { Walking, Interacting }
public class mainplayer : MonoBehaviour
{
    // Singleton instance of the player controller
    public static mainplayer instance;

    // Movement speed of the player
    public float moveSpeed = 5f;

    // Whether the player can currently move
    private bool canMove = true;

    // Reference to the Rigidbody2D component of the player
    private Rigidbody2D rb;

    // Sprite Manager
    public SpriteManager spriteManager;

    private PlayerStates playerState;

    public List<GameObject> keyItems = new List<GameObject>();

    // Called when the script instance is being loaded
    void Awake()
    {
        // Set up the singleton instance
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player can move and if the player is in the walking state
        if (canMove && playerState == PlayerStates.Walking)
        {
            // Handle player movement
            MovePlayer();
        }
    }

    // Method to handle player movement
    void MovePlayer()
    {
        // Get input from horizontal and vertical axes
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Flip sprite based on horizontal movement
        if (moveHorizontal > 0) { spriteManager.toggleFlip(true); }
        else if (moveHorizontal < 0) { spriteManager.toggleFlip(false); }

        Vector2 movement = Vector2.zero;

        // Determine the dominant direction of movement based on the input magnitude
        if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical))
        {
            // Horizontal movement is dominant
            movement = new Vector2(moveHorizontal, 0);
        }
        else if (Mathf.Abs(moveVertical) > Mathf.Abs(moveHorizontal))
        {
            // Vertical movement is dominant
            movement = new Vector2(0, moveVertical);
        }

        // Apply movement to the player's Rigidbody2D if movement is allowed
        if (canMove)
        {
            rb.velocity = movement * moveSpeed;
        }
    }

    // Method to enable or disable player movement
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;

        // If movement is disabled, set velocity to zero
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void UpdatePlayerState(PlayerStates playState)
    {
        playerState = playState;

        // Whenever the player state is updated, reset the velocity
        rb.velocity = Vector2.zero;
    }

    
}
