using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_behavior : MonoBehaviour
{

    public Image Life;
    public Image Ink;
    public LifeManager DisplayLifeOf;
    public InkManager DisplayInk;

    public int MaxLife;
    public int MaxInk;

    private int LifeAnim;
    private int Animation;
    // Start is called before the first frame update

    void Start()
    {
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
        //Debug.Log("Life displayed - " + (LifeAnim));
        Ink.fillAmount = (DisplayInk.getInk() * 1f / MaxInk );
    }
}
