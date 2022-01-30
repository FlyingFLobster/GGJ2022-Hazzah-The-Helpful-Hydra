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

    private GameObject camera; // For tracking the camera to the currently speaker character.

    private Dictionary<string, Dictionary<string, LineData>> lines;
    private LineData currentLine;


    // Start is called before the first frame update
    void Start()
    {
        // Load all the dialogue in this zone (currently there is only one zone and all actor codes are hardcoded)
        lines = LineLoader.GetZoneLines("zone1");
        //Debug.Log("STARTED");

        // TODO: how does the ConversationController know which player and npc objects are involved?

        // Set the first line of the conversation - how is this known?

        // Obtain reference to camera.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called upon first starting a conversation, retrieves the initial line of dialogue.
    public void InitiateConversation(string playerChoice, string actorCode, string lineCode)
    {
        Debug.Log("INITIATED");
        if (lines == null)
        {
            lines = LineLoader.GetZoneLines("zone1");
        }
        currentLine = lines[actorCode][lineCode];

        player.GetComponent<Talker>().SetIdleConversation();
        npc.GetComponent<Talker>().SetIdleConversation();

        AdvanceConversation(playerChoice);
    }

    // This is called when the player presses the Haz or Zah buttons, which button is sent as a button Name.
    // Codes are: "Talk With Haz"
    //            "Talk With Zah"
    // If the current piece of dialogue isn't a choice, both buttons will just advance dialogue normally.
    public void AdvanceConversation(string playerChoice)
    {
        string switch_code;
        LineData next_line = null;
        //Debug.Log(currentLine.switches_set_true.ToString());
        

        if (player.GetComponent<Talker>().IsIdleConversation() && npc.GetComponent<Talker>().IsIdleConversation())
        {
            //Debug.Log(currentLine.ToString());
            if (currentLine.targets1[0] == "") // If no next line, conversation ends.
            {
                EndConversation();
            }

            // Find the next line of dialogue based on the switches, targets, and Haz/Zah player choice
            // The final switch is always a dummy empty string ""
            for (int i = 0; i < currentLine.switches_read.Count; i++)
            {
                switch_code = currentLine.switches_read[i];

                // If switch code is genuine, get its target if it is set to true, otherwise continue
                if (switch_code != "")
                {
                    if (player.GetComponent<PlayerController>().CheckSwitch(switch_code))
                    {
                        next_line = ChooseWhichHead(playerChoice, i);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                //Debug.Log("Switch Code: " + switch_code + ", CheckSwitch Result: " + player.GetComponent<PlayerController>().CheckSwitch(switch_code));

                //Debug.Log("HERE");
                // If the fake switch ("") is reached, just get the target
                next_line = ChooseWhichHead(playerChoice, i);
            }

            if (next_line == null)
            {
                //Debug.Log("No next line found when advancing conversation.");

                // Probably just kill the conversation for safety
                EndConversation();
            }
            else
            {
                // Update the current line
                currentLine = next_line;

                // Where to send the text of the new line?
                // This will depend on who is speaking, each character has a talker component that it needs to get sent to.
                // Probably going to need to look at the actor code to figure that out.

                // Clear the text from both characters (Since we should probably only have one talking at a time)
                player.GetComponent<Talker>().ClearText();
                npc.GetComponent<Talker>().ClearText();

                // Check if there is a decision to be made and if so, display the button prompts.
                if (currentLine.targets2[0] != "")
                {
                    player.GetComponent<PlayerController>().ShowPrompts();
                }
                else // Else, display the generic dual prompt.
                {
                    player.GetComponent<PlayerController>().ShowDualPrompt();
                }

                // Figure out who the current line belongs to and send it to them.
                // New part of json, speaker field will define the character who is speaking the current line,
                // Can use that to switch based on the character (need special cases for double characters).

                // Can probably remove the cases for Haz and Floop since they should default to VoiceHolder1 anyway, but I'll keep them just in case.
                if (currentLine.speaker == "haz")
                {
                    //player.GetComponent<Talker>().SetTextColor(new Color(247, 101, 120)); F76578 
                    player.GetComponent<Talker>().TalkText(currentLine.text, "VoiceHolder1");
                    camera.GetComponent<CameraController>().ChangeFocus(player, -1);
                }
                else if (currentLine.speaker == "zah")
                {
                    //player.GetComponent<Talker>().SetTextColor(new Color(170, 225, 255)); #AAE1FF
                    player.GetComponent<Talker>().TalkText(currentLine.text, "VoiceHolder2");
                    camera.GetComponent<CameraController>().ChangeFocus(player, 1);
                }
                else if (currentLine.speaker == "floop")
                {
                    npc.GetComponent<Talker>().TalkText(currentLine.text, "VoiceHolder1");
                    camera.GetComponent<CameraController>().ChangeFocus(npc);
                }
                else if (currentLine.speaker == "robert")
                {
                    npc.GetComponent<Talker>().TalkText(currentLine.text, "VoiceHolder2");
                    camera.GetComponent<CameraController>().ChangeFocus(npc);
                }
                else // Any other NPC
                {
                    npc.GetComponent<Talker>().TalkText(currentLine.text, "VoiceHolder1");
                    camera.GetComponent<CameraController>().ChangeFocus(npc);
                }
            }
        }
    }

    private LineData ChooseWhichHead(string playerChoice, int switchIdx)
    {
        string next_line_code;
        LineData next_line;

        // If there is only one dialogue option for this switch, that is the next line
        if (currentLine.targets2[switchIdx] == "")
        {
            next_line_code = currentLine.targets1[switchIdx];
            // ^ This is in the form of "actor_code/linecode" right now I think, so need to split it into two strings.
            
        }
        // If there are two, consider the player's input choice
        else
        {
            next_line_code = (playerChoice == "Talk With Haz") ? currentLine.targets1[switchIdx] : currentLine.targets2[switchIdx];
            
        }

        string[] codes = next_line_code.Split('/');
        if (next_line_code != "")
        {
            string actorCode = codes[0];
            string lineCode = codes[1];
            try
            {
                next_line = lines[actorCode][lineCode];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("Key not found in lines: " + actorCode + "/" + lineCode);
            }
        }
        else
        {
            next_line = null;
            Debug.Log("Next line code: " + next_line_code);
            // Update switches due to the current line
            foreach (string code in currentLine.switches_set_true)
            {
                //Debug.Log("HERE");
                player.GetComponent<PlayerController>().ActivateSwitch(code);
            }
            foreach (string code in currentLine.switches_set_false)
            {
                player.GetComponent<PlayerController>().DeactivateSwitch(code);
            }
            EndConversation();
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
        //camera.GetComponent<CameraController>().ChangeFocus(player);
        Destroy(gameObject);
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

    public void SetCamera(GameObject inCamera)
    {
        camera = inCamera;
    }
}
