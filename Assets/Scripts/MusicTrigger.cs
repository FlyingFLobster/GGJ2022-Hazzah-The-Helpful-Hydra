using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    private AudioSource music;

    private void Start()
    {
        music = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Get Player's music player
            GameObject musicPlayer = other.gameObject.transform.Find("MusicPlayer").gameObject;
            musicPlayer.GetComponent<MusicController>().SetSong(music);
        }
    }
}
