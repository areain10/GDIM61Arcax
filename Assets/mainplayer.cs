using UnityEngine;

public class mainplayer : MonoBehaviour
{
    private float speednum = 5.0f; // Speed of player movement
    private Rigidbody2D rb; // Rigidbody2D component instance

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Getting the Rigidbody2D component
    }

    void Update()
    {
        // this for the  horizontal and vertical input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Creating movement vector 
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // this is for moving the player
        rb.velocity = movement * speednum;
    }
}
