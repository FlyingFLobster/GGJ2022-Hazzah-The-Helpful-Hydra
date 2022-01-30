using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Ssylviss;

    // Called when the collider 'other' enters this trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.gameObject.GetComponent<PlayerController>().CheckSwitch("introwatched")) // If intro hasn't been watched yet.
            {
                other.gameObject.GetComponent<PlayerController>().IntroConversation(Ssylviss);
                other.gameObject.GetComponent<PlayerController>().ActivateSwitch("introwatched");
            }
        }
    }
}
