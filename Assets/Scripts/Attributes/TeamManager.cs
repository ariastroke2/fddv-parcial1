using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{

    [SerializeField] private int team;
    
    public void UpdateTeam(int nTeam){
        team = nTeam;
    }

    public void ParentTeam(){
        team = transform.parent.GetComponent<TeamManager>().GetTeam();
    }

    public int GetTeam(){
        return team;
    }
}
