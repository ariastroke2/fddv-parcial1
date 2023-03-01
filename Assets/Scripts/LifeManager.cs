using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int HP = 100;
    public bool reg = false;
    public bool DestroyWhenKilled = false;

    private int regint;
    private int limit;
    
    void Start(){
        limit = HP;
        regint = 0;
    }

    public void Damage(int dmg){
        HP -= dmg;
        //Debug.Log("Reg Damage by "+ dmg);
    }

    public int Life(){
        return HP;
    }
    
    public void Set(int NewHP){
        HP = NewHP;
        //Debug.Log("HP set to "+ NewHP);
    }

    void FixedUpdate()
    {
        if(reg){
            regint++;
            if(regint > 10){
                if(HP < limit){
                    HP += 1;
                }
                regint = 0;
            }
        }
        if(DestroyWhenKilled){
            if(HP < 1){
                gameObject.SetActive(false);
                if(gameObject.GetComponent<OctoTrooper>() != null){
                    gameObject.GetComponent<OctoTrooper>().Notify.GetComponent<EnemyTracker>().OSenemies -= 1;
                    gameObject.GetComponent<OctoTrooper>().Kill();
                }
                if(gameObject.GetComponent<Character_Controller>() != null){
                    gameObject.GetComponent<Character_Controller>().GameOverCanva.SetActive(true);
                    gameObject.GetComponent<Character_Controller>().Kill();
                }else
                    Destroy(gameObject);
            }
        }
    }
}
