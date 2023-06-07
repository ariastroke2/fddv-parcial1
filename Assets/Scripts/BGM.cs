using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicStart;

    // Start is called before the first frame update
    void Start()
    {
        if(musicStart != null){
            musicSource.PlayOneShot(musicStart);
            musicSource.PlayScheduled(AudioSettings.dspTime + musicStart.length);
        }else{
            musicSource.PlayScheduled(AudioSettings.dspTime);
        }
    }
}
