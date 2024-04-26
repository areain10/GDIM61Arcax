using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Conversation", order = 1)]
public class ConversationSO : ScriptableObject
{
    //conversationData npcData;

    public int start;
    public string[] PlayerDialogue;

    public string[] NPCDialogue;
}
