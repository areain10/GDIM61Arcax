using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Conversation", order = 1)]
public class ConversationSO : ScriptableObject
{
    //conversationData npcData;

    // who starts convo
    public int start;

    //play9ers dialogue in order
    public string[] PlayerDialogue;


    // npc dialogue
    public string[] NPCDialogue;
}
