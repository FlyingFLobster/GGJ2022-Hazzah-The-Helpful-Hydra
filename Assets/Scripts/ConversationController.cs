//@Author Zachary Kidd-Smith
// This script is spawned on an empty object at the start of a conversation, it's responsibility is to
// keep track of the script and command the conversation participants.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    private GameObject player;
    private GameObject npc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // These should be called as soon as the conversation controller is instantiated, to prevent unruly behaviour.
    public void SetPlayer(GameObject inPlayer)
    {
        player = inPlayer;
        player.GetComponent<Talker>().SetIdleConversation();
    }

    public void SetNPC(GameObject inNPC)
    {
        npc = inNPC;
        npc.GetComponent<Talker>().SetIdleConversation();
    }
}
