using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            Debug.Log("volume changed");
        }
    }

    public float GetVolume()
    {
        return audioSource != null ? audioSource.volume : 0f;
    }
}
