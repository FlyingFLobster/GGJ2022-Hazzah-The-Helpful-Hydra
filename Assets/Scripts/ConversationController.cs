//@Author Zachary Kidd-Smith
// This script is spawned on an empty object at the start of a conversation, it's responsibility is to
// keep track of the script and command the conversation participants.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

public class ConversationController : MonoBehaviour
{
    private GameObject player;
    private GameObject npc;
    private Dictionary<string, Dictionary<string, LineData>> lines;
    private LineData currentLine;

    // Start is called before the first frame update
    void Start()
    {
        // Load all the dialogue in this zone (currently there is only one zone and all actor codes are hardcoded)
        lines = GetZoneLines("zone1");

        // TODO: how does the ConversationController know which player and npc objects are involved?

        // Set the first line of the conversation - how is this known?


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This is called when the player presses the Haz or Zah buttons, which button is sent as a button Name.
    // Codes are: "Talk With Haz"
    //            "Talk With Zah"
    // If the current piece of dialogue isn't a choice, both buttons will just advance dialogue normally.
    public void AdvanceConversation(string playerChoice)
    {
        string switch_code;
        LineData next_line;

        // Find the next line of dialogue based on the switches, targets, and Haz/Zah player choice
        // The final switch is always a dummy empty string ""
        for (int i = 0; i < currentLine.switches_read.Count; i++)
        {
            switch_code = currentLine.switches_read[i];

            // If switch code is genuine, get its target if it is set to true, otherwise continue
            if (switch_code != "")
            {
                if (GlobalSwitches[switch_code])
                {
                    next_line = ChooseWhichHead(playerChoice, i);
                    break;
                }
            }

            // If the fake switch ("") is reached, just get the target
            next_line = ChooseWhichHead(playerChoice, i);
        }


        // Update switches due to the current line
        foreach (string switch_code in currentLine.switches_set_true)
        {
            GlobalSwitches[switch_code] = true;
        }
        foreach (string switch_code in currentLine.switches_set_false)
        {
            GlobalSwitches[switch_code] = false;
        }

        // Update the current line
        currentLine = next_line;

        // Where to send the text of the new line?
    }

    private LineData ChooseWhichHead(string playerChoice, int switchIdx)
    {
        string next_line_code;
        LineData next_line;

        // If there is only one dialogue option for this switch, that is the next line
        if (currentLine.targets2[switchIdx] == "")
        {
            next_line_code = currentLine.targets1[switchIdx];
            next_line = lines[next_line_code];
        }
        // If there are two, consider the player's input choice
        else
        {
            next_line_code = (playerChoice == "Talk With Haz") ? currentLine.targets1[switchIdx] : currentLine.targets2[switchIdx];
            next_line = lines[next_line_code];
        }

        return next_line;
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
