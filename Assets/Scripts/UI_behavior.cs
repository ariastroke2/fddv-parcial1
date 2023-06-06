using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_behavior : MonoBehaviour
{

    public Image Life;
    public Image Ink;
    public Image Charge;
    public Image Bombs;
    public Image Weapon;
    public GameObject target;

    private LifeManager DisplayLifeOf;
    private InkManager DisplayInk;
    private Splatling_Behav DisplayCharge;
    private Brella_Behav DisplayCool;
    private BombManager DisplayBomb;

    public int MaxLife;
    public int MaxInk;
    public int MaxCharge;

    private int LifeAnim;
    private int Animation;

    public Sprite Brella;
    public Sprite Shooter;
    public Sprite Splatling;

    // Start is called before the first frame update

    void Start()
    {
        DisplayLifeOf = target.GetComponent<LifeManager>();
        DisplayInk = target.GetComponent<InkManager>();
        DisplayBomb = target.GetComponent<BombManager>();
            
        Bombs.fillAmount = 1f;
        MaxLife = DisplayLifeOf.Life();
        Life.fillAmount = 0.65f;
        LifeAnim = MaxLife;
        //Debug.Log("My max - " + MaxLife);
        MaxInk = DisplayInk.getInk();
    }

    // Update is called once per frame
    void Update()
    {
        Animation++;
        if(Animation > 15){
            if(LifeAnim < DisplayLifeOf.Life()){
                LifeAnim -= (int)Mathf.Round((LifeAnim - DisplayLifeOf.Life()) / 1.1f);
            }else{
                LifeAnim = DisplayLifeOf.Life();
            }
            Animation = 0;
        }
        Life.fillAmount = (LifeAnim * 1f / MaxLife )*0.65f;


        Ink.fillAmount = (DisplayInk.getInk() * 1f / MaxInk );


        if(DisplayCharge != null){
            Charge.fillAmount = (DisplayCharge.Charge * 1f / MaxCharge );
        }else{
            if(DisplayCool != null){
                Charge.fillAmount = ((MaxCharge - DisplayCool.GetCooldown()) * 1f / MaxCharge );
            }else{
                Charge.fillAmount = 1;
            }
        }

        //Bombs.fillAmount = (DisplayBomb.bombs * ((DisplayBomb.CustomCooldown - DisplayBomb.cooldown) / DisplayBomb.CustomCooldown)) / 5;
        Bombs.fillAmount =  (((DisplayBomb.GetCurrentCooldownType() - DisplayBomb.CurrentCooldownProgress()) * 1f / DisplayBomb.GetCurrentCooldownType()));
    }

    private GameObject Retrieve(string a){
        GameObject ret = null;

        if(target.transform.Find(a))
            ret = target.transform.Find(a).gameObject;
        else
            ret = null;
        
        return ret;
    }

    public void ChangeWeapon(string param = "", GameObject settings = null){

        DisplayCharge = null;
        DisplayCool = null;

        //GameObject temp = Retrieve("Splatling");
        if(param == "Splatling")
        {
            Debug.Log("Splatling settings");
            DisplayCharge = settings.GetComponent<Splatling_Behav>();
            MaxCharge = DisplayCharge.MaxCharge;
        }
        if(param == "Brella")
        {
            Debug.Log("Brella found");
            DisplayCool = settings.GetComponent<Brella_Behav>();                            
            MaxCharge = DisplayCool.CustomCooldown;
        }
        if(param == "Shooter"){
            Debug.Log("No special script not found");    
        }
        Weapon.sprite = settings.transform.Find("Shooter").GetComponent<SpriteRenderer>().sprite;
        
        
        /*else{
                temp = Retrieve("TentaBrella");                                                 
                if(temp != null)
                {
                    Debug.Log("TentaBrella found");
                    DisplayCool = temp.GetComponent<Brella_Behav>();
                    MaxCharge = DisplayCool.CustomCooldown;
                    Weapon.sprite = Brella;
                }else{
                    temp = Retrieve("UndercoverBrella");                                                 
                    if(temp != null)
                    {
                        Debug.Log("UndercoverBrella found");
                        DisplayCool = temp.GetComponent<Brella_Behav>();
                        MaxCharge = DisplayCool.CustomCooldown;
                        Weapon.sprite = Brella;
                    }else{
                        temp = Retrieve("HydraSplatling");                                                 
                    if(temp != null)
                    {
                        Debug.Log("HydraSplatling found");
                        DisplayCharge = temp.GetComponent<Splatling_Behav>();
                        MaxCharge = DisplayCharge.MaxCharge;
                        Weapon.sprite = Brella;
                    }else{
                        temp = Retrieve("MiniSplatling");                                                 
                    if(temp != null)
                    {
                        Debug.Log("MiniSplatling found");
                        DisplayCharge = temp.GetComponent<Splatling_Behav>();
                        MaxCharge = DisplayCharge.MaxCharge;
                        Weapon.sprite = Brella;
                    }else{
                        Debug.Log("No special script not found");    
                        Weapon.sprite = Shooter;            
                    }                 
                    }                  
                                     
                }           
            }
        }*/
    }
}
