using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class conversationHandler : MonoBehaviour
{
    GameObject interactObject;
    conversationData conversationData;
    bool inconvo;

    [SerializeField] private Canvas conversationCanvas;
    // Start is called before the first frame update
    void Start()
    {
        inconvo = false;
        interactObject = null;
        conversationCanvas.gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "NPC" && collision.gameObject.GetComponent<conversationData>() != null)
        {
            conversationData = collision.gameObject.GetComponent<conversationData>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NPC" && collision.gameObject.GetComponent<conversationData>() != null)
        {
            conversationData = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        handleInteract();
        exitConvo();
    }

    private bool handleInteract()
    {
        if (conversationData != null && Input.GetKeyDown(KeyCode.E) && inconvo == false)
        {
            conversationCanvas.gameObject.SetActive(true);
            mainplayer.instance.UpdatePlayerState(PlayerStates.Interacting);
            
            ConversationUI.instance.StartConvo(conversationData);
            ConversationUI.instance.inconvo = true;
            inconvo = true;
            return true;
        }
        return false;
    }

    private void exitConvo()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && conversationCanvas.gameObject.activeSelf == true)
        {
            //conversationCanvas.gameObject.SetActive(false);
            inconvo = false;
            ConversationUI.instance.exitConverstion();
            
        }
    }
}
