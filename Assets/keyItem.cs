using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyItem : MonoBehaviour
{
    public bool pickedUP;
    public Vector2 vel;
    public float smoothTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUP) { followPlayer(); }
    }

    void followPlayer()
    {
        Vector3 offset = new Vector3(0, 0.5f, 0);
        transform.position = Vector2.SmoothDamp(transform.position,mainplayer.instance.transform.position + offset,ref vel,smoothTime);
    }
}
