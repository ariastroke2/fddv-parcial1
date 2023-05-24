using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer_Full : MonoBehaviour
{

    public Vector2 direction;
    public GameObject Head;
    public GameObject EyeBig;
    public GameObject EyeSmall;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = (
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - 
            ((Vector2)transform.position)
        ).normalized;
        
        Head.transform.right = new Vector2(direction.x, direction.y / 3f);
        //Debug.Log("Pointing to - " + direction);

        if(direction.x < 0){
            transform.localScale = new Vector2(-1, transform.localScale.y);
            Head.transform.localScale = new Vector2(-1.2f, -1.2f);
        }
        else{
            transform.localScale = new Vector2(1, transform.localScale.y);
            Head.transform.localScale = new Vector2(1.2f, 1.2f);
        }
    }
}
