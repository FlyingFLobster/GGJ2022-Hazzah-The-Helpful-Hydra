//@author Zachary Kidd-Smith
// This script handles playing the background music of the game, probably keep this on an object attached to the player character
// so that we can easily check for them entering a new trigger zone to play a different song.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource mainSong;

    // Start is called before the first frame update
    void Start()
    {
        mainSong = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        mainSong.Play();
    }

    public void Stop()
    {
        mainSong.Stop();
    }

    public void SetSong(AudioSource inSong)
    {
        // If the song isn't new, don't do anything (If this check wasn't here it'd restart)
        if (!inSong.Equals(mainSong))
        {
            Stop(); // Make sure the original stops playing before the reference is discarded.
            mainSong = inSong;
            Play();
        }
    }
}
