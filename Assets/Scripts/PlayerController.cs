//@author Zachary Kidd-Smith
// Script for the main player character (Hazzah!), should handle all player controlled input during the gameplay (Eg: Walking around, interaction buttons, etc).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
        // Movement.
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

        tr.position = tr.position + new Vector3(horizontalInput * Time.deltaTime, 0.0f, verticalInput * Time.deltaTime);

        // Interaction.
        /*
         * Talking with NPCs should start with a button press that broadcasts to a nearby npc, initiating a dialogue sequence with them.
         */
        // Talk with Haz
        if (Input.GetButtonDown("Talk With Haz") || Input.GetButtonDown("Talk With Zah"))
        {
            // Cast a collider to check for a nearby NPC to interact with.
            List<Collider> hits = new List<Collider>(Physics.OverlapSphere(gameObject.transform.position, 3));
            hits.RemoveAll(hit => !hit.tag.Equals("NPC")); // Filter all non-NPC tagged objects out.


            // Just take the first one to interact with, since none of them should be super closeby eachother,
            // and all non-NPC tagged objects have been filtered out.
            if (hits.Count > 0)
            {
                GameObject interactionTarget = hits[0].gameObject;
                interactionTarget.GetComponent<NPCController>().StartTalk();
                
            }
        }
    }

    private void FixedUpdate()
    {
        // Apply change in movement, currently uses the old unity input system, going to try to avoid physics.

    }
}
