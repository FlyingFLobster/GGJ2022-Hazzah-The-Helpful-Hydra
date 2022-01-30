// @author Zachary Kidd-Smith
// Script for controlling NPC Behaviour (Floating dialogue, conversation, etc).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private string actorCode;
    [SerializeField] private string initialLineCode;

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

    /*
    // Called by the player object to initiate a conversation.
    public void StartTalk()
    {
        //talker.TalkText("Oh? You're approaching me?");
        //talker.ShowFloatingDialogue();
    }
    */

    // Called by the conversation controller as cleanup after the conversation has finished.
    public void EndConversation()
    {
        talker.SetIdle();
    }

    public string GetActorCode()
    {
        return actorCode;
    }

    public string GetInitialLineCode()
    {
        return initialLineCode;
    }
}
