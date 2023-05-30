using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer_Full : MonoBehaviour
{

    public Vector2 direction;
    public GameObject Head;
    public GameObject EyeBig;
    public GameObject EyeSmall;

    [SerializeField] private float VLimit;
    private float HLimit;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Transform>();
        HLimit = Mathf.Sqrt(1 - Mathf.Pow(VLimit, 2));
    }

    private Vector2 NonZero(Vector2 pre){
        Vector2 pos = pre;
        if(Mathf.Round(pos.x*100) == 0){
            pos = new Vector2(0.01f, pos.y);
        }
        if(Mathf.Round(pos.y*100) == 0){
            pos = new Vector2(pos.x, 0.01f);
        }
        return pos;
    }

    private Vector2 DirLimit(Vector2 pre){
        Vector2 pos = pre;
        if(pos.y > VLimit){
            pos = new Vector2(HLimit * (pos.x / Mathf.Abs(pos.x)), VLimit);
        }
        if(pos.y < -VLimit){
            pos = new Vector2(HLimit * (pos.x / Mathf.Abs(pos.x)), -VLimit);
        }
        pos = pos.normalized;
        return pos;
    }

    // Update is called once per frame
    void Update()
    {
        direction = (
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - 
            ((Vector2)transform.position)
        ).normalized;
        direction = DirLimit(NonZero(direction));
        
        Head.transform.right = new Vector2(direction.x, direction.y / 3f);
        //Debug.Log("Pointing to - " + direction);

        if(direction.x <= 0){
            transform.localScale = new Vector2(-1, transform.localScale.y);
            Head.transform.localScale = new Vector2(-1.2f, -1.2f);
        }
        else{
            transform.localScale = new Vector2(1, transform.localScale.y);
            Head.transform.localScale = new Vector2(1.2f, 1.2f);
        }
    }
}
