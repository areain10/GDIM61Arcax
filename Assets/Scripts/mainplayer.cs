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

    //Sprite REnderer
    public SpriteManager spriteManager;

    private PlayerStates playerState;

    float lastHInput;
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

        lastHInput = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player can move
        if (playerState == PlayerStates.Walking)
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

        if(moveHorizontal > 0) { spriteManager.toggleFlip(true); }
        else if(moveHorizontal < 0) { spriteManager.toggleFlip(false); }

        // Calculate movement direction
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        
        // Apply movement to the player's Rigidbody2D
        rb.velocity = movement * moveSpeed;
        

        lastHInput += moveHorizontal;
    }

    // Method to enable or disable player movement
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public void UpdatePlayerState(PlayerStates playState)
    {
        playerState = playState;
        rb.velocity = Vector2.zero;
    }
}
