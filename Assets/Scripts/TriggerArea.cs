using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{

    public GameObject[] spawners;

    private bool Used;

    // Start is called before the first frame update
    void Start()
    {
        Used = false;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player"))
        {
            if(!Used){
            Used = true;
            for(int i = 0; i < spawners.Length; i++){
                spawners[i].GetComponent<Spawner>().FeedInput(1);
            }
        }
        }
        
    }
}
