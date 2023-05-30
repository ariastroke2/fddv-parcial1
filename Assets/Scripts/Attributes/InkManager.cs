using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    [SerializeField] int Ink = 100;

    private int regint;
    private int limit;
    private bool regallow;

    // Start is called before the first frame update
    void Start()
    {
        limit = Ink;
        regint = 0;
    }   

    public void Consume(int used){
        Ink -= used;
        //Debug.Log("Reg Damage by "+ dmg);
    }

    public void Add(int extra){
        Ink += extra;
        //Debug.Log("Reg Damage by "+ dmg);
    }

    public void Set(int Refill){
        Ink = Refill;
        //Debug.Log("HP set to "+ NewHP);
    }

    public void Can(){
        regallow = true;
        //Debug.Log("HP set to "+ NewHP);
    }

    public void Cannot(){
        regallow = false;
        //Debug.Log("HP set to "+ NewHP);
    }

    public int getInk(){
        return Ink;
    }

    void FixedUpdate()
    {
        if(regallow){
            regint++;
            if(regint > 6){
                if(Ink < limit){
                    Ink += 1;
                }
                regint = 0;
            }
        }
    }


}
