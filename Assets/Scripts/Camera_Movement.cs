using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{

    public GameObject target;
    public GameObject parallax;
    public float SensitivityX = 0;
    public float SensitivityY = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - 
            ((Vector2)target.transform.position)
        ).normalized;
        
        float depsX = SensitivityX * Mathf.Sin(direction.x * 90 * Mathf.Deg2Rad);
        float depsY = SensitivityY * Mathf.Sin(direction.y * 90 * Mathf.Deg2Rad);

        //float depsX = 0f;
        //float depsY = 0f;

        transform.position = new Vector3(target.transform.position.x + depsX,
         target.transform.position.y + 4 + depsY, -10);

         parallax.transform.position = new Vector3((target.transform.position.x + depsX) / 1.1f ,
         (target.transform.position.y + 4 + depsY) / 1.1f , 0);
         

    }
}
