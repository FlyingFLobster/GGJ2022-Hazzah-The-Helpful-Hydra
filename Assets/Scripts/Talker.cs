//@author Zachary Kidd-Smith
// This script handles the talking for the character it is attached to, playing a sound clip while writing out dialogue.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : MonoBehaviour
{
    [SerializeField] private GameObject floatingTextPrefab;

    private GameObject floatingText;

    // Dummy dialogue list, can slap in the system that Nick comes up with in here.
    private string[] chatter = new string[] { "Blah blah", "Did you know?", "Some cats have thumbs. . .", "Crazy right?" };
    private int currentChatter;

    private string text; // Text to be displayed.

    // Enum of states talkers can be in, helps determine some behaviour.
    public enum State
    {
        Idle,
        Talking
    };

    private State currentState;


    // Start is called before the first frame update
    void Start()
    {
        if (floatingTextPrefab)
        {
            floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            floatingText.GetComponentInChildren<TextMesh>().text = text;
        }

        currentState = State.Idle;
        currentChatter = 0;
        text = chatter[currentChatter];

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

        ShowFloatingDialogue();
    }

    public void ShowFloatingDialogue()
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

    // Shows text in the text box over time.
    private IEnumerator TextOverTime(string inText)
    {
        Debug.Log(inText);
        string originalString = inText;
        int charCounter = 0;
        while (charCounter < originalString.Length)
        {

            while (originalString[charCounter] == ' ') // Skip over spaces.
            {
                charCounter = charCounter + 1;
            }

            charCounter = charCounter + 1;

            text = originalString.Substring(0, charCounter);

            yield return new WaitForSeconds(0.07f);
        }
    }

    // Cycle's through the NPC's idle chatter and displays it in their floating text box.
    private void CycleChatter()
    {
        if (currentState == State.Idle)
        {
            currentChatter = currentChatter + 1;

            if (currentChatter >= chatter.Length)
            {
                currentChatter = 0;
            }

            text = chatter[currentChatter];
        }

        Invoke("CycleChatter", 4.0f); // Invokes itself in 4 seconds time, runs for basically the NPC's entire life.
    }

    public void ClearText()
    {
        text = "";
    }

    public void SetText(string inText)
    {
        text = "";
        StartCoroutine(TextOverTime(inText));
    }

    public void SetIdle()
    {
        currentState = State.Idle;
    }

    public void SetTalking()
    {
        currentState = State.Talking;
    }
}
