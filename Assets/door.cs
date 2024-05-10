using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public GameObject Key;
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
        if (collision.tag == "Player")
        {
            Debug.Log("On door");
            for (int i = 0;i < mainplayer.instance.keyItems.Count; i++)
            {
                Debug.Log(mainplayer.instance.keyItems[i].name.Substring(0, (mainplayer.instance.keyItems[i].name.Length - 7)) +  Key.name);
                if (mainplayer.instance.keyItems[i].name.Substring(0, (mainplayer.instance.keyItems[i].name.Length - 7)) == Key.name)
                {
                    Destroy(mainplayer.instance.keyItems[i]);
                    mainplayer.instance.keyItems.Remove(mainplayer.instance.keyItems[i]);

                    
                    Destroy(gameObject); break;
                }
            }
        }
    }
}
