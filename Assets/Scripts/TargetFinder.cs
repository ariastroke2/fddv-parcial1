using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{

    public GameObject CustomDefault;
    private GameObject FoundTargetObj;

    private Collider2D Range;
    private List<GameObject> Inside;

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
            FoundTargetObj = CustomDefault;
        }
        foreach (GameObject item in Inside) {
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

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag != "Particle" && other.gameObject.tag != "Ground"){
            if(Team != other.gameObject.GetComponent<TeamManager>().GetTeam()){
                Inside.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag != "Particle" && other.gameObject.tag != "Ground"){
            if(Inside.Contains(other.gameObject))
                Inside.Remove(other.gameObject);
        }
    }
}
