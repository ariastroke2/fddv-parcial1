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
    public Image BombType;
    public Image FadeAnimator;
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

    public Sprite[] Brellas;
    public Sprite[] BombsSprites;

    // Start is called before the first frame update

    void Start()
    {
        DisplayLifeOf = target.GetComponent<LifeManager>();
        DisplayInk = target.GetComponent<InkManager>();
        DisplayBomb = target.GetComponent<BombManager>();

        target.GetComponent<Character_Controller>().Enter(new Vector2(-80f, 40f));
            
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

        BombType.sprite = BombsSprites[DisplayBomb.bombtype];

        FadeAnimator.color = new Color(0,0,0,(130 - Time.time * 50) / 100);
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
            switch(MaxCharge){
                case 45:
                    Weapon.sprite = Brellas[0];
                    break;
                case 75:
                    Weapon.sprite = Brellas[1];
                    break;
                case 15:
                    Weapon.sprite = Brellas[2];
                    break;
            }
        }
        if(param == "Shooter"){
            Debug.Log("No special script not found");    
        }
        if(param != "Brella")
            Weapon.sprite = settings.transform.Find("Shooter").GetComponent<SpriteRenderer>().sprite;
        
        
    }
}
