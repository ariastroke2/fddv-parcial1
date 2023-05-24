using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool Active = false;
    public GameObject Entity;
    public GameObject ParentTarget;
    public GameObject Tracker;
    public int randomRange = 0;
    public int FrameWait = 1000;

    private int progress;

    // Start is called before the first frame update
    void Start()
    {
        progress = Random.Range(0, randomRange);
    }

    // Update is called once per frame
    void Update()
    {
        if(Active){
            //Debug.Log("Generator activated");
            progress++;
            if(progress > FrameWait){
                Debug.Log("Octotrooper summoned");
                GameObject obj = (GameObject)Instantiate(Entity, transform.position, transform.rotation);
                obj.GetComponent<OctoTrooper>().Target = ParentTarget;
                obj.GetComponent<OctoTrooper>().Notify = Tracker;
                Active = false;
                progress = 0;
            }
        }
    }
}
