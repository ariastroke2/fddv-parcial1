using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedObject : MonoBehaviour
{

    private GameObject Notify;

    public void AssignTracker(GameObject t){
        Notify = t;
    }

    public bool IsActive(){
        return (Notify != null);
    }

    public void SendSignal(){
        Notify.GetComponent<EnemyTracker>().EnemyKilled();
    }

}
