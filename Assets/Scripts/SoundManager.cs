using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] audioSources;


    public void PlaySound(int index)
    {
        if (index >= 0)
        {
            audioSources[index].Play();
        }
    }

    public void PauseSound(int index)
    {
        if (index >= 0)
        {
            audioSources[index].Pause();
        }
    }

    public void StopSound(int index)
    {
        if (index >= 0)
        {
            audioSources[index].Stop();
        }
    }

    public void SetVolume(int index, float volume)
    {
        if (index >= 0)
        {
            audioSources[index].volume = volume;
        }
    }
}
