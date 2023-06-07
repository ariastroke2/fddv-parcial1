using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TutorialScript : MonoBehaviour
{

    public LifeManager reactorHp;
    public Image FadeAnimator;
    // Start is called before the first frame update

    private float StartTime;
    private int FadePer;
    private int timer;

    private float seconds;

    void Start()
    {
        FadePer = 100;
        seconds = 2.0f;
        timer = 120;
        //GameObject.Find("Player").GetComponent<Character_Controller>().Enter(new Vector2(-80f, 40f));
    }

    // Update is called once per frame
    void Update()
    {
        if(reactorHp.Life() < 1){
            timer--;
            if(timer == 0){
                GameObject.Find("Player").GetComponent<Character_Controller>().Exit(new Vector2(500f, 120f));
                StartTime = Time.time;
            }
            if(timer < 0)
                FadeAnimator.color = new Color(0,0,0,((Time.time - StartTime) * 50) / 100);
            if(timer < -150)
                SceneManager.LoadScene(2);
        }else{
            //FadeAnimator.color = new Color(0,0,0,(130 - Time.time * 50) / 100);
        }
    }
}
