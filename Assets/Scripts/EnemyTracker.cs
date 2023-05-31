using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    private int OnScreenEnemies;
    public GameObject[] Spawners;

    public GameObject MasterObjective;

    private AudioSource audio;
    [SerializeField] private AudioClip Success;

    private int Wave;
    private int WaveLength;
    private int WaveStep;
    private int State;

    private int Timer;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        //TotalScore = -1;

        for(int i = 0; i < Spawners.Length; i++){
            Spawners[i].GetComponent<Spawner>().SetTracker(gameObject);
            Spawners[i].GetComponent<Spawner>().SetMasterObjective(MasterObjective);
        }
        
        WaveLength = 3;
        WaveStep = WaveLength;
        OnScreenEnemies = 0;
        Wave = 6;
        State = 1;
        Timer = 0;
    }

    void Update()
    {
        if(State == 0){
            if(OnScreenEnemies == 0){
                if(WaveStep > 0){
                
                    int[] WaveData = WaveGenerator(Wave);

                    string msg = "";
                    for(int i = 0; i < WaveData.Length; i++){
                        msg += WaveData[i] + ", ";
                    }
                    Debug.Log("Wave " + Wave + " +-+ " + msg);

                    
                    Debug.Log("Feeding spawners, enemies on screen - "+OnScreenEnemies);
                    for(int i = 0; i < Spawners.Length; i++){
                        Spawners[i].GetComponent<Spawner>().FeedInput(WaveData[i]);
                    }
                WaveStep--;
                }else{
                    Wave++;
                    Timer = 0;
                    State = 1;
                    WaveStep = WaveLength;
                    audio.PlayOneShot(Success);
                }
            }
        }else{
            Timer++;
            if(Timer > 60){
                State--;
                Timer = 0;
            }
        }
    }

    private int[] WaveGenerator(int wavegen){
        int MaxDiff = (int)(Mathf.Round(wavegen / 2)) - 2;
        int WaveScore = wavegen - 3;
        int GeneratedEnemiesCounter = 0;

        int[] Chances = new int[((MaxDiff * (MaxDiff + 1))/2) + 1];
        int[] Generated = new int[Spawners.Length];

        Chances[0] = 0;
        for(int i = 1; i <= MaxDiff; i++){
            for(int j = 0; j < i; j++){
                Chances[i + j] = i;
            }
        }

        int counter = 0;
        while(counter < Spawners.Length && WaveScore > 0){

            int Chosen = Chances[Random.Range(0, Chances.Length)];
            WaveScore -= Chosen;
            Generated[counter] = Chosen;
            if(Chosen != 0)
                GeneratedEnemiesCounter++;

            counter++;
        }
        if(counter < Spawners.Length){
            while(counter < Spawners.Length){
                Generated[counter] = 0;
                counter++;
            }
        }

        for(int i = 0; i < Spawners.Length; i++){
            int r = Random.Range(i, Spawners.Length);
            int temp = Generated[i];
            Generated[i] = Generated[r];
            Generated[r] = temp;
        }

        OnScreenEnemies = GeneratedEnemiesCounter;

        return Generated;
    }

    public int GetWave(){
        return Wave - 5;
    }

    public int GetState(){
        return State;
    }

    public int GetWaveStep(){
        return WaveStep;
    }

    public int GetWaveLength(){
        return WaveLength;
    }

    public void EnemyKilled(){
        Debug.Log("Counter has gone down by one");
        OnScreenEnemies--;
    }
}
