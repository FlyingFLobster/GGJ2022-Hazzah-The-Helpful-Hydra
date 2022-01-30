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
    [SerializeField] string[] chatter;
    private int currentChatter;

    private string text; // Text to be displayed.
    private Color textColor;

    // Enum of states talkers can be in, helps determine some behaviour.
    public enum State
    {
        Idle,
        IdleConversation,
        Talking
    };

    private State currentState;

    // Audio Source for speech.
    //private AudioSource[] speech;


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

        //speech = GetComponentsInChildren<AudioSource>();

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
    private IEnumerator TextOverTime(string inText, AudioSource voice)
    {
        voice.Play();
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
        voice.Stop();
        SetIdleConversation();
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

    // Starts a talk over time routine for the character.
    // Speaker will correspond to which voice to use (since some characters have multiple voices)
    public void TalkText(string inText, string voiceName)
    {
        if (currentState != State.Talking)
        {
            AudioSource voice = transform.Find(voiceName).gameObject.GetComponent<AudioSource>();
            SetTalking();
            text = "";
            StartCoroutine(TextOverTime(inText, voice));
        }
    }

    public void SetIdle()
    {
        //Debug.Log("SET TO IDLE");
        currentState = State.Idle;
    }

    public void SetIdleConversation()
    {
        currentState = State.IdleConversation;
    }

    public void SetTalking()
    {
        currentState = State.Talking;
    }

    public void SetTextColor(Color inColor)
    {
        textColor = inColor;
    }

    public State GetState()
    {
        return currentState;
    }

    public bool IsIdleConversation()
    {
        return currentState == State.IdleConversation;
    }
}
