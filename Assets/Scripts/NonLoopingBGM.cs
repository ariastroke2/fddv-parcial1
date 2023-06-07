using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLoopingBGM : MonoBehaviour
{

    public AudioSource musicSource;
    public AudioClip[] Music;

    public void Play()
    {
        if(!musicSource.isPlaying){
            musicSource.clip = Music[Random.Range(0, Music.Length)];
            musicSource.Play();
        }
            
    }
}
