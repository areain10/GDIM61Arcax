using System.Collections.Generic;
using UnityEngine;

public class SafeRoomManager : MonoBehaviour
{
    public List<GameObject> npcsInSafeRoom;
    public Collider2D safeRoomCollider; // Reference to the safe room Collider2D
    public Transform startLocation;

    private void Start()
    {
        // Initialize the list with NPCs present in the safe room at the start
        npcsInSafeRoom = new List<GameObject>();
        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
        {
            if (IsInSafeRoom(npc.transform.position))
            {
                npcsInSafeRoom.Add(npc);
            }
        }

        // Remove the NPCs that were killed in previous attempts
        /*int killedNpcs = PlayerPrefs.GetInt("KilledNpcs", 0);
        for (int i = 0; i < killedNpcs; i++)
        {
            KillNpc();
        }*/
    }

    public void KillNpc()
    {
        if (npcsInSafeRoom.Count > 0)
        {
            mainplayer.instance.gameObject.transform.position = startLocation.position;
            GameObject npcToKill = npcsInSafeRoom[0];
            npcsInSafeRoom.RemoveAt(0);
            Destroy(npcToKill);
        }
    }

    public bool AreAllNpcsKilled()
    {
        return npcsInSafeRoom.Count == 0;
    }

    private bool IsInSafeRoom(Vector2 position)
    {
        // Check if the position is within the safe room collider
        return safeRoomCollider.OverlapPoint(position);
    }
}
