using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{

    public GameObject target;
    public GameObject parallax;
    public float SensitivityX = 0;
    public float SensitivityY = 0;

    [SerializeField] private MousePointer_Full DirectionImport; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null){

            Vector2 direction = (
                (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - 
                ((Vector2)target.transform.position)
            ).normalized;

            float depsX = SensitivityX * Mathf.Sin(direction.x * 90 * Mathf.Deg2Rad);
            float depsY = SensitivityY * Mathf.Sin(direction.y * 90 * Mathf.Deg2Rad);

            //float depsX = 0f;
            //float depsY = 0f;

            // + (direction.x / Mathf.Abs(direction.x) * 15f)

            transform.position = new Vector3(target.transform.position.x + depsX,
            target.transform.position.y + 5 + depsY, -10);

            if(transform.position.y <= 7){
                transform.position = new Vector3(transform.position.x,
                7f, -10);
            }

            parallax.transform.position = new Vector3((transform.position.x) / 1.1f ,
            (transform.position.y) / 1.1f , 0);
        }
    }
}
