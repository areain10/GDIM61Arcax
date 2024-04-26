using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationUI : MonoBehaviour
{
    public static ConversationUI instance;
    public Image npcSprite;
    public TextMeshProUGUI speech;
    private conversationData npcData;
    private ConversationSO convoS;
    private int[] convoStep = { -2,-2};
    private bool isPlayerTalking;
    private bool[] convoDone;
    private bool inconvo;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inconvo) { checkForUpdate(); }
    }

    public void StartConvo(conversationData data)
    {
        convoDone = new bool[2];
        convoDone[0] = false; convoDone[1] = false;
        inconvo = false;
        convoStep = new int[2] { 0,0};
        convoDone = new bool[2];
        npcSprite.sprite = data.sprite;
        convoS = data.so;
        if(convoS.start == 0) { isPlayerTalking = true; } else { isPlayerTalking = false; }
        inconvo = true;
        updateConvo();
       

    }

    void checkForUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
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
        switch (isPlayerTalking)
        {
            case true:speech.text = convoS.PlayerDialogue[convoStep[0]];
                if ((convoStep[0] + 1) == convoS.PlayerDialogue.Length)
                {
                    convoDone[0] = true;
                }
                else { convoStep[0] += 1;}


                break;
            case false:speech.text = convoS.NPCDialogue[convoStep[1]];
                if ((convoStep[1] + 1) == convoS.NPCDialogue.Length)
                {
                    convoDone[1] = true;
                }
                else { convoStep[1] += 1; }
                break;
        }
        isPlayerTalking = !isPlayerTalking;
        
        
    }
    public void exitConverstion()
    {
        gameObject.SetActive(false);
        mainplayer.instance.UpdatePlayerState(PlayerStates.Walking);
    }
}
