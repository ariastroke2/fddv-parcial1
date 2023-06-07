using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{

    public GameObject CustomDefault;
    public GameObject FoundTargetObj;

    private Collider2D Range;
    public List<GameObject> Inside;

    private int Team;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TeamManager>().ParentTeam();
        Team = GetComponent<TeamManager>().GetTeam();
        Range = GetComponent<Collider2D>();
        Inside = new List<GameObject>();
    }

    public GameObject FoundTarget(){
        return FoundTargetObj;
    }

    // Update is called once per frame
    void Update()
    {
        if(Inside.Count == 0){
            if(CustomDefault != null)
                FoundTargetObj = CustomDefault;
            else
                FoundTargetObj = null;
        }
        foreach (GameObject item in Inside) {
            if(item.CompareTag("Particle")){
                if(FoundTargetObj == item)
                    FoundTargetObj = null;
                Debug.Log("Forgot " + item.name + " because it was tagged as particle");
                Inside.Remove(item);
            }else{
                if(FoundTargetObj == null){
                    FoundTargetObj = item;
                }else{
                    float distanceOld = (
                                (Vector2)FoundTargetObj.transform.position - 
                                (Vector2)transform.position
                            ).magnitude;
                    float distance = (
                                (Vector2)item.transform.position - 
                                (Vector2)transform.position
                            ).magnitude;
                    if(distanceOld > distance){
                        FoundTargetObj = item;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag != "Particle" && other.gameObject.tag != "Ground"){
            if(Team != other.gameObject.GetComponent<TeamManager>().GetTeam()){
                Inside.Add(other.gameObject);
                Debug.Log("Found player");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(Inside.Contains(other.gameObject)){
            Debug.Log("Forgot " + other.gameObject.name + " because it exited range");
            Inside.Remove(other.gameObject);
            if(FoundTargetObj == other.gameObject)
                FoundTargetObj = null;
        }
    }
}
