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

    // This is called when the player presses the Haz or Zah buttons, which button is sent as a button Name.
    // Codes are: "Talk With Haz"
    //            "Talk With Zah"
    // If the current piece of dialogue isn't a choice, both buttons will just advance dialogue normally.
    public void AdvanceConversation(string buttonName)
    {

    }

    // This is called when the conversation has finished, should clean everything up to how it was before the,
    // conversation happened and destruct the Conversation Controller.
    private void EndConversation()
    {
        // Call EndConversation methods on Player and NPC.
        player.GetComponent<PlayerController>().EndConversation();
        npc.GetComponent<NPCController>().EndConversation();
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
