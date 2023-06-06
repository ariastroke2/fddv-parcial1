using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Changer : MonoBehaviour
{
   //Usado para colocar sonidos
    [SerializeField] private AudioSource clickSoundEffect;

    
    public void Play(int index)
    {
        clickSoundEffect.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void Settings(int index)
    {
        clickSoundEffect.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void QuitGame(){
        Application.Quit();
    }

}

   