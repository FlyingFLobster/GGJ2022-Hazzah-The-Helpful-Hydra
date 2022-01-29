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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        talker = GetComponent<Talker>();
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

        // Interaction.
        /*
         * Talking with NPCs should start with a button press that broadcasts to a nearby npc, initiating a dialogue sequence with them.
         */
        // Talk with Haz
        if (Input.GetButtonDown("Talk With Haz") || Input.GetButtonDown("Talk With Zah"))
        {
            if (talker.GetState() == Talker.State.Idle) // If not in conversation, initialize one.
            {
                // Cast a collider to check for a nearby NPC to interact with.
                List<Collider> hits = new List<Collider>(Physics.OverlapSphere(gameObject.transform.position, 3));
                hits.RemoveAll(hit => !hit.tag.Equals("NPC")); // Filter all non-NPC tagged objects out.


                // Just take the first one to interact with, since none of them should be super closeby eachother,
                // and all non-NPC tagged objects have been filtered out.
                if (hits.Count > 0)
                {
                    GameObject interactionTarget = hits[0].gameObject;

                    // Setup conversation Controller.
                    ConversationController conv = Instantiate(ConversationControllerPrefab, transform.position, Quaternion.identity).GetComponent<ConversationController>();

                    conv.SetPlayer(gameObject);
                    conv.SetNPC(interactionTarget);

                    //interactionTarget.GetComponent<NPCController>().StartTalk();
                    //talker.SetIdleConversation();
                }
            }
            else // In a conversation
            {

            }
        }
    }

    private void FixedUpdate()
    {
    }
}
