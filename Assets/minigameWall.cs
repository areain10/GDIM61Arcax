using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigameWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collide");
        if (collision.gameObject.tag == "MinigameBox")
        {
            collision.gameObject.GetComponent<hamsterMinigame>().Reset();
        }
    }
}
