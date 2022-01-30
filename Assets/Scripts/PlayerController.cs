//@author Zachary Kidd-Smith
// Script for the main player character (Hazzah!), should handle all player controlled input during the gameplay (Eg: Walking around, interaction buttons, etc).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject ConversationControllerPrefab;
    public int playerSpeed = 0;

    private Rigidbody rb;
    private Transform tr;
    private Talker talker;

    // Boolean dictionary, Keys are names for dialogue switches, values are booleans, just gonna store on 
    // the player for now for ease of access since they will always be in the scene and part of the conversation.
    Dictionary<string, bool> dialogueSwitches = new Dictionary<string, bool>();

    /*
     * 
    {
        {"SSYLVISSTALKED", false},
        {"RHASTTALKED", false},
        {"RHASTTIP", false}
    };
     */

    private ConversationController conv;

    // The two interact prompts.
    private GameObject promptHaz;
    private GameObject promptZah;
    private GameObject promptDual;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        talker = GetComponent<Talker>();
        conv = null;

        // Get References to interact prompts.
        promptHaz = gameObject.transform.Find("Haz Prompt").gameObject;
        promptZah = gameObject.transform.Find("Zah Prompt").gameObject;
        promptDual = gameObject.transform.Find("Dual Prompt").gameObject;

        // Disable the interact prompts (should only be visible when in a situation where they can be pressed).
        HidePrompts();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = 0;
        float verticalInput = 0;

        // Check inputs.
        // Movement (Can only move when not in a conversation).
        if (talker.GetState() == Talker.State.Idle)
        {
            if (Input.GetButton("Horizontal")) // Negative = left, Positive = right
            {
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    horizontalInput += playerSpeed;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    horizontalInput -= playerSpeed;
                }
            }

            if (Input.GetButton("Vertical")) // Negative = down, Positive = up
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    verticalInput += playerSpeed;
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    verticalInput -= playerSpeed;
                }
            }
        }


        tr.position = tr.position + new Vector3(horizontalInput * Time.deltaTime, 0.0f, verticalInput * Time.deltaTime);


        // Cast a collider to check for a nearby NPC to interact with.
        List<Collider> hits = new List<Collider>(Physics.OverlapSphere(gameObject.transform.position, 3));
        hits.RemoveAll(hit => !hit.tag.Equals("NPC")); // Filter all non-NPC tagged objects out.

        // If there is an NPC nearby then display the two talk prompts.
        if (talker.GetState() == Talker.State.Idle && hits.Count >0)
        {
            ShowPrompts();
        }
        else if (talker.GetState() == Talker.State.Idle)
        {
            HidePrompts();
            HideDualPrompt();
        }

        // Interaction.
        /*
         * Talking with NPCs should start with a button press that broadcasts to a nearby npc, initiating a dialogue sequence with them.
         */
        // Talk with Haz
        bool hazTalk = Input.GetButtonDown("Talk With Haz");
        bool zahTalk = Input.GetButtonDown("Talk With Zah");
        if (hazTalk || zahTalk)
        {
            if (talker.GetState() == Talker.State.Idle) // If not in conversation, initialize one.
            {
                // Just take the first one to interact with, since none of them should be super closeby eachother,
                // and all non-NPC tagged objects have been filtered out.
                if (hits.Count > 0)
                {
                    GameObject interactionTarget = hits[0].gameObject;

                    // Setup conversation Controller.
                    conv = Instantiate(ConversationControllerPrefab, transform.position, Quaternion.identity).GetComponent<ConversationController>();

                    conv.SetPlayer(gameObject);
                    conv.SetNPC(interactionTarget);

                    //interactionTarget.GetComponent<NPCController>().StartTalk();
                    //talker.SetIdleConversation();

                    // Get the codes from the NPC for their dialogue initiating line.
                    string actorCode = interactionTarget.GetComponent<NPCController>().GetActorCode();
                    string lineCode = interactionTarget.GetComponent<NPCController>().GetInitialLineCode();

                    // Call initial dialogue to start the conversation with Haz or Zah.
                    if (hazTalk)
                    {
                        conv.InitiateConversation("Talk With Haz", actorCode, lineCode);
                    }
                    else
                    {
                        conv.InitiateConversation("Talk With Zah", actorCode, lineCode);
                    }
                }
            }
            else // In a conversation
            {
                if (conv != null)
                {
                    if (hazTalk)
                    {
                        conv.AdvanceConversation("Talk With Haz");
                    }
                    else
                    {
                        conv.AdvanceConversation("Talk With Zah");
                    }
                }
            }
        }
    }

    // Called by the conversation controller as cleanup after the conversation has finished.
    public void EndConversation()
    {
        talker.SetIdle();
        conv = null;
        HidePrompts();
        HideDualPrompt();
    }

    // Takes in a key for the dictionary of dialogue switches and sets it to true.
    public void ActivateSwitch(string key)
    {
        bool value;
        if (dialogueSwitches.TryGetValue(key, out value))
        {
            dialogueSwitches[key] = true;
        }
        else
        {
            dialogueSwitches.Add(key, true);
        }
    }

    public void DeactivateSwitch(string key)
    {
        bool value;
        if (dialogueSwitches.TryGetValue(key, out value))
        {
            dialogueSwitches[key] = false;
        }
        else
        {
            dialogueSwitches.Add(key, false);
        }
    }

    public bool CheckSwitch(string key)
    {
        bool value;
        bool result;
        if (dialogueSwitches.TryGetValue(key, out value))
        {
            result = dialogueSwitches[key];
        }
        else
        {
            dialogueSwitches.Add(key, false);
            result = false;
        }
        return result;
    }

    // These two hide and display the Haz/Zah prompts above the player's head, indicating that they can do something.
    public void ShowPrompts()
    {
        HideDualPrompt();
        promptHaz.SetActive(true);
        promptZah.SetActive(true);
    }

    public void ShowDualPrompt()
    {
        HidePrompts();
        promptDual.SetActive(true);
    }

    public void HidePrompts()
    {
        promptHaz.SetActive(false);
        promptZah.SetActive(false);
    }

    public void HideDualPrompt()
    {
        promptDual.SetActive(false);
    }

    // Special case since the character is technically talking to themself, this is activated by a trigger.
    public void IntroConversation(GameObject Ssylviss)
    {
        conv = Instantiate(ConversationControllerPrefab, transform.position, Quaternion.identity).GetComponent<ConversationController>();

        conv.SetPlayer(gameObject);

        conv.SetNPC(Ssylviss);
        conv.InitiateConversation("Talk With Haz", "player_haz", "intro0");
    }
}
