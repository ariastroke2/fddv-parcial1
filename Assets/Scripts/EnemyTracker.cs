using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public int OSenemies;
    public GameObject s1, s2, s3, s4;
    public int Difficulty;

    public AudioClip levelup;
    private AudioSource audio;

    public TMPro.TextMeshProUGUI enemiesdisplay;

    private int TotalScore;

    private int ScreensClear = 0;

    private GameObject[] randomizer;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        TotalScore = -1;
        randomizer = new GameObject[4];
        randomizer[0] = s1;
        randomizer[1] = s2;
        randomizer[2] = s3;
        randomizer[3] = s4;
        Difficulty = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Enemies on screen - "+OSenemies);
        if(OSenemies == 0){
            TotalScore += Difficulty;
            enemiesdisplay.text = TotalScore + "";
            audio.PlayOneShot(levelup);
            Debug.Log("Generation Start");
            switch(ScreensClear){
                case 1:
                    Difficulty = 2;
                    break;
                case 5:
                    Difficulty = 3;
                    break;
                case 10:
                    Difficulty = 4;
                    break;
            }
            int[] chosen = {0, 1, 2, 3};

            // Shuffle
            for(int i = 0; i < 4; i++){
                
                int r = Random.Range(i, 4);
                Debug.Log(" shuffling " + i + " and " + r);
                int temp = chosen[i];
                chosen[i] = chosen[r];
                chosen[r] = temp;
            }

            for(int i = 0; i < 4; i++){
                
                if(i < Difficulty)
                    randomizer[chosen[i]].GetComponent<Spawner>().Active = true;
            }
            OSenemies = Difficulty;
            ScreensClear++;
        }
    }
}
