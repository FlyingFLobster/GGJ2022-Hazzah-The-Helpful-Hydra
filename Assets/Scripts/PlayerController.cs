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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
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
        if (Input.GetButtonDown(""))
        {

        }
    }

    private void FixedUpdate()
    {
        // Apply change in movement, currently uses the old unity input system, going to try to avoid physics.

    }
}
