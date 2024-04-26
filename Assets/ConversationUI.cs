using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationUI : MonoBehaviour
{
    //singleton
    public static ConversationUI instance;

    //npc data
    public Image npcSprite;
    private conversationData npcData;
    private ConversationSO convoS;

    //convo variables
    private int[] convoStep = { -2,-2};
    private bool isPlayerTalking;
    private bool[] convoDone;
    public bool inconvo;
    private Vector3 ogSize = new Vector3(1,1,1);
    private Vector3 idleSize = new Vector3(1,1,1) * 0.85f;

    //uielements
    public TextMeshProUGUI speech;
    public Image playerSprite;
    private void Awake()
    {
        instance = this;
        inconvo = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
        Debug.Log(inconvo);
    }

    // Update is called once per frame
    void Update()
    {
        if(inconvo) { checkForUpdate(); }
    }

    public void StartConvo(conversationData data)
    {
        //initialize variables, (should be simplified later
        convoStep = new int[2] { 0,0};
        convoDone = new bool[2];
        npcSprite.sprite = data.sprite;
        convoS = data.so;

        //Check who is starting the conversation
        if(convoS.start == 0) { isPlayerTalking = true; } else { isPlayerTalking = false; }
        inconvo = true;
        Debug.Log(inconvo);

        //update convo
        updateConvo();
       

    }

    //update convo text when the user presses the left mouse button
    void checkForUpdate()
    {
        inconvo = true;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(!convoDone.All(x => x))
            {
                updateConvo();
            }
            else
            {
                exitConverstion();
            }
            
        }
    }

    private void updateConvo()
    {
        inconvo = true;
        //check who is talking
        switch (isPlayerTalking)
        {
            //if player is talking, show player dialogue
            case true:playerSprite.color = Color.white; playerSprite.gameObject.transform.localScale = ogSize; npcSprite.gameObject.transform.localScale = idleSize; npcSprite.color = Color.gray; speech.text = convoS.PlayerDialogue[convoStep[0]];
                if ((convoStep[0] + 1) == convoS.PlayerDialogue.Length)
                {
                    //if player dialogue ended then change done
                    convoDone[0] = true;
                }
                else { convoStep[0] += 1;}


                break;
            //same thing but with npc dialogue
            case false:
                npcSprite.color = Color.white; npcSprite.gameObject.transform.localScale = ogSize; playerSprite.gameObject.transform.localScale = idleSize; playerSprite.color = Color.gray; speech.text = convoS.NPCDialogue[convoStep[1]];
                if ((convoStep[1] + 1) == convoS.NPCDialogue.Length)
                {
                    convoDone[1] = true;
                }
                else { convoStep[1] += 1; }
                break;
        }
        //change player is talking
        isPlayerTalking = !isPlayerTalking;
        
        
    }

    //exit conversation
    public void exitConverstion()
    {
        gameObject.SetActive(false);
        mainplayer.instance.UpdatePlayerState(PlayerStates.Walking);
    }
}
