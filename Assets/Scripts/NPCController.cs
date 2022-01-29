// @author Zachary Kidd-Smith
// Script for controlling NPC Behaviour (Floating dialogue, conversation, etc).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private GameObject floatingTextPrefab;

    private GameObject floatingText;
    private string text;

    // Dummy dialogue list, can slap in the system that Nick comes up with in here.
    private string[] chatter = new string[] {"Blah blah", "Did you know?", "Some cats have thumbs. . .", "Crazy right?"};
    private int currentChatter;

    // Start is called before the first frame update
    void Start()
    {
        currentChatter = 0;
        text = chatter[currentChatter];
        ShowFloatingDialogue();

        Invoke("CycleChatter", 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        // Update the position of the floating text to above the character.
        Vector3 floatingTextPosition = transform.position;
        floatingTextPosition.y = GetComponent<Collider>().bounds.size.y;
        floatingText.transform.position = floatingTextPosition;
    }

    void ShowFloatingDialogue()
    {
        if (floatingText == null)
        {
            if (floatingTextPrefab)
            {
                floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
                floatingText.GetComponentInChildren<TextMesh>().text = text;
            }
        }
        else
        {
            floatingText.GetComponentInChildren<TextMesh>().text = text;
        }
    }

    // Cycle's through the NPC's idle chatter and displays it in their floating text box.
    void CycleChatter()
    {
        currentChatter = currentChatter + 1;

        if (currentChatter >= chatter.Length)
        {
            currentChatter = 0;
        }

        text = chatter[currentChatter];

        ShowFloatingDialogue();

        Invoke("CycleChatter", 4.0f); // Invokes itself in 4 seconds time.
    }
}
