using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm_Manager : MonoBehaviour
{

    public GameObject forearm;
    public GameObject upperarm;

    public SpriteRenderer upperarmrend, forearmrend;

    public float HalfLength = 1.2f;

    public float ShoulderX = 0;
    public float ShoulderY = 0;

    public float HoldDistance = 2.2f;
    public float HoldOffsetX = 0f;
    public float HoldOffsetY = 0.5f;

    public string Arm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 PreHand = (
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - 
            ((Vector2)transform.parent.transform.position)
        ).normalized*HoldDistance + (Vector2)transform.parent.transform.position + new Vector2(HoldOffsetX, HoldOffsetY);

        Vector2 HandPosition = (Vector2)PreHand;
        Vector2 ShoulderPosition = new Vector2(transform.position.x + ShoulderX, transform.position.y + ShoulderY);

        Vector2 directionP = (
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - 
            ((Vector2)transform.parent.transform.position)
        ).normalized;

        Vector2 direction = (
            HandPosition - 
            ShoulderPosition
        ).normalized;

        //transform.right = direction;

        // Arm Hinges

        Vector2 shoulder = ShoulderPosition;
        Vector2 hand = HandPosition;

        if(
            (hand - 
            ShoulderPosition
            ).magnitude
                >
            HalfLength*2
        ){
            hand = ((direction * HalfLength * 2f) + ShoulderPosition);
        }

        /*if(direction.x < 0){
            if(Arm == "L"){
                hand = ((directionP * HalfLength * 1f) + ShoulderPosition);
            }
        }
        else{
            if(Arm == "R"){
                hand = ((directionP * HalfLength * 1f) + ShoulderPosition);
            }
        }*/

        forearm.transform.position = hand;
        upperarm.transform.position = shoulder;

        // Arm rotation

        //float ArmRotationG = 0;
        
        float ArmRotationG = Mathf.Acos(
            (hand - 
            ShoulderPosition
            ).magnitude
                /
            (HalfLength * 2)
        ) * Mathf.Rad2Deg;
        

        if(ArmRotationG == 0 || float.IsNaN(ArmRotationG))
            ArmRotationG = 0.1f;

        float AxisRotationG = Vector2.SignedAngle(new Vector2(ShoulderPosition.x + 2, ShoulderPosition.y) - ShoulderPosition, hand - ShoulderPosition) + 90;
        
        // ArmRotationG = 0;
        // AxisRotationG = 0;

        if(direction.x < 0){
            transform.localScale = new Vector2(transform.localScale.x, -1);
            ArmRotationG = 180 - ArmRotationG;
            if(Arm == "L"){
                upperarmrend.sortingOrder = 5;
                forearmrend.sortingOrder = 6;
            }else{
                upperarmrend.sortingOrder = 2;
                forearmrend.sortingOrder = 3;
            }
        }
        else{
            transform.localScale = new Vector2(transform.localScale.x, 1);
            if(Arm == "R"){
                upperarmrend.sortingOrder = 5;
                forearmrend.sortingOrder = 6;
            }else{
                upperarmrend.sortingOrder = 2;
                forearmrend.sortingOrder = 3;
            }
        }

        forearm.transform.rotation = Quaternion.Euler(0,0, ArmRotationG + 180 + AxisRotationG);
        upperarm.transform.rotation = Quaternion.Euler(0, 0, AxisRotationG - ArmRotationG);

        
    }
}
