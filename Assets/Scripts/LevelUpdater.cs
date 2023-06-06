using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpdater : MonoBehaviour
{

    [SerializeField] private GameObject[] Templates;
    [SerializeField] private GameObject[] Platforms;

    private int State;

    [SerializeField] private List<Vector2> coords;

    void Start()
    {
        
    }

    public void PullPlatforms(){
        State = 1;
    }

    public void LoadLevel(int level){
        coords.Clear();
        GameObject Template = Instantiate(Templates[level], Vector3.zero, Quaternion.identity);
        coords = new List<Vector2>();
        for(int i = 0; i < Template.transform.childCount; i++){
                coords.Add(Template.transform.GetChild(i).position);
        }
        for(int i = 0; i < Platforms.Length; i++){
            int r = Random.Range(i, Platforms.Length);
            GameObject temp = Platforms[i];
            Platforms[i] = Platforms[r];
            Platforms[r] = temp;
            if(i < coords.Count)
                Platforms[i].transform.position = new Vector2(coords[i].x, -20f);
        }
        Destroy(Template);
    }

    public void PushPlatforms(){
        State = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(State == 1){
            bool done = true;
            for(int i = 0; i < Platforms.Length; i++){
                if(Vector2.Distance(Platforms[i].transform.position, new Vector2(Platforms[i].transform.position.x, -20f)) > 0.1f){
                    Platforms[i].transform.position = Vector2.MoveTowards(Platforms[i].transform.position, new Vector2(Platforms[i].transform.position.x, -20f), Time.deltaTime * 15f);
                    done = false;
                }
            }
            if(done)
                State = 0;
        }
        if(State == 2){
            bool done = true;
            for(int i = 0; i < coords.Count; i++){
                if(Vector2.Distance(Platforms[i].transform.position, coords[i]) > 0.1f){
                    Platforms[i].transform.position = Vector2.MoveTowards(Platforms[i].transform.position, coords[i], Time.deltaTime * 15f);
                    done = false;
                }
            }
            if(done)
                State = 0;
        }
    }
}
