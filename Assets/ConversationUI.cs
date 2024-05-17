using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
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
    public TextMeshProUGUI NPCName;
    StreamReader convoText;
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
        //convoS = data.so;
        NPCName.text = data.NPCName;
        Time.timeScale = 0;
        convoText = new StreamReader("Assets/Conversations/"+data.NPCName+".txt");
        
        //Check who is starting the conversation
        if ((int)convoText.ReadLine()[0] == 0) { isPlayerTalking = true; } else { isPlayerTalking = false; }
        inconvo = true;
        Debug.Log(inconvo);

        //update convo
        updateConvo();
       

    }

    //update convo text when the user presses the left mouse button
    void checkForUpdate()
    {
        inconvo = true;
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E))
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
            
            case true:
                if (convoText.EndOfStream)
                {
                    //if player dialogue ended then change done
                    
                    convoDone[0] = true;
                    convoDone[1] = true;
                    exitConverstion();
                    Debug.Log("EndConvo");
                    break;
                }
                else 
                { convoStep[0] += 1;
                    speech.text = convoText.ReadLine();
                    playerSprite.color = Color.white; playerSprite.gameObject.transform.localScale = ogSize; npcSprite.gameObject.transform.localScale = idleSize; npcSprite.color = Color.gray;
                }


                break;
            //same thing but with npc dialogue
            case false:
                
                if (convoText.EndOfStream)
                {
                    convoDone[0] = true;
                    convoDone[1] = true;
                    Debug.Log("EndConvo");
                    exitConverstion();
                    break;
                }
                else { convoStep[1] += 1; speech.text = convoText.ReadLine(); npcSprite.color = Color.white; npcSprite.gameObject.transform.localScale = ogSize; playerSprite.gameObject.transform.localScale = idleSize; playerSprite.color = Color.gray; }
                break;
        }
        //change player is talking
        isPlayerTalking = !isPlayerTalking;
        
        
    }

    //exit conversation
    public void exitConverstion()
    {

        convoText.Close();
        convoText = null;
        Time.timeScale = 1;
        gameObject.SetActive(false);
        mainplayer.instance.UpdatePlayerState(PlayerStates.Walking);
        mainplayer.instance.GetComponent<conversationHandler>().inconvo = false;
    }
}
