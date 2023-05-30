using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Spawned;
    private GameObject MasterObjective;
    private GameObject Tracker;

    private int SpawnType;

    public void SetTracker(GameObject t){
        Tracker = t;
    }

    public void SetMasterObjective(GameObject m){
        MasterObjective = m;
    }

    public void FeedInput(int Input){
        SpawnType = Input - 1;
        if(SpawnType >= Spawned.Length)
            SpawnType = Spawned.Length - 1;
        if(SpawnType != -1)
            Spawn();
    }

    private void Spawn(){
        Debug.Log("Octotrooper summoned, SpawnType - "+SpawnType);
        GameObject obj = (GameObject)Instantiate(Spawned[SpawnType], transform.position, transform.rotation);
        obj.GetComponent<OctoTrooper>().finder.CustomDefault = MasterObjective;
        obj.GetComponent<OctoTrooper>().Notify = Tracker;
    }
}
