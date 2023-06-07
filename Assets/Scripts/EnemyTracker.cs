using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    private int OnScreenEnemies;
    public GameObject[] Spawners;

    public GameObject MasterObjective;

    [SerializeField] private GameObject WeaponBoxTemplate;
    private GameObject CurrentWeaponBox;

    private LevelUpdater lvlUpd;
    private NonLoopingBGM BGM;

    private AudioSource audio;
    [SerializeField] private AudioClip Success;

    private int Wave;
    private int WaveLength;
    private int WaveStep;
    private int State;

    private int Timer;

    void Start()
    {
        BGM = GameObject.Find("BGM").GetComponent<NonLoopingBGM>();
        audio = GetComponent<AudioSource>();
        lvlUpd = GetComponent<LevelUpdater>();
        //TotalScore = -1;

        for(int i = 0; i < Spawners.Length; i++){
            Spawners[i].GetComponent<Spawner>().SetTracker(gameObject);
            Spawners[i].GetComponent<Spawner>().SetMasterObjective(MasterObjective);
        }
        
        WaveLength = 3;
        WaveStep = WaveLength;
        OnScreenEnemies = 0;
        Wave = 6;
        State = 10;
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
                    State = 10;
                    WaveStep = WaveLength;
                    audio.PlayOneShot(Success);
                }
            }
        }else{
            Timer++;
            if(Timer > 60){
                State--;
                Timer = 0;

                if(State == 0){
                    if(BGM != null){
                        BGM.Play();
                    }
                }

                if((Wave + 6)%4 == 0 && Wave != 6){
                    if(State == 9){
                        lvlUpd.PullPlatforms();
                    }
                    if(State == 6){
                        lvlUpd.LoadLevel(Random.Range(0, 5));
                    }
                    if(State == 5){
                        lvlUpd.PushPlatforms();
                    }
                }
                if(State == 8)
                    if(Wave%2 == 1){
                        if(CurrentWeaponBox != null)
                            CurrentWeaponBox.transform.Find("ChangeWeapon").gameObject.GetComponent<Weapons_Menu>().DestroyBox();
                        CurrentWeaponBox = Instantiate(WeaponBoxTemplate, new Vector2(0, 35f), transform.rotation);
                    }
            }
        }
        
    }

    private int[] WaveGenerator(int wavegen){
        int MaxDiff = (int)(Mathf.Round(wavegen / 2)) - 2;
        int MinDefault;
        int WaveScore = wavegen - 3;
        int GeneratedEnemiesCounter = 0;
        int[] Chances;

        switch(MaxDiff){
            case 1:
                Chances = new int[] {1};
                break;
            case 2:
                Chances = new int[] {1,2};
                break;
            case 3:
                Chances = new int[] {1,2,3,3};
                break;
            case 4:
                Chances = new int[] {1,2,3,3,4,4};
                break;
            default:
                Chances = new int[] {1,2,3,3,4,4,5,5,5};
                break;
        }

        if(wavegen < 16)
            MinDefault = 0;
        else if(wavegen < 20)
            MinDefault = 1;
        else
            MinDefault = 2;

        //int[] Chances = new int[((MaxDiff * (MaxDiff + 1))/2)];
        int[] Generated = new int[Spawners.Length];

        /*Chances[0] = MaxDiff / 2;
        for(int i = 1; i <= MaxDiff; i++){
            for(int j = 0; j < i; j++){
                Chances[i + j] = i;
            }
        }*/

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
                Generated[counter] = MinDefault;
                if(MinDefault != 0)
                    GeneratedEnemiesCounter++;
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
