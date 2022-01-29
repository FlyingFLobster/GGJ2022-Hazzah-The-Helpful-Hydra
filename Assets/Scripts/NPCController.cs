// @author Zachary Kidd-Smith
// Script for controlling NPC Behaviour (Floating dialogue, conversation, etc).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{   
    private Talker talker;

    // Start is called before the first frame update
    void Start()
    {
        talker = GetComponent<Talker>();
        talker.ShowFloatingDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called by the player object to initiate a conversation.
    public void StartTalk()
    {
        talker.SetTalking();
        talker.ClearText();
        talker.SetText("Oh? You're approaching me?");
        //talker.ShowFloatingDialogue();
    }
}
