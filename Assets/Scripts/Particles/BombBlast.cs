using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlast : MonoBehaviour
{
    [SerializeField]
    private GameObject Spark;

    private List<GameObject> list;
    public int Team = 0;
    public float size;
    public int dmg;

    private bool active;
    private int duration;

    public bool shield = false;

    // Update is called once per frame
    void Start()
    {
        list = new List<GameObject>();
        transform.localScale = new Vector2(size, size);
        active = true;
        duration = 0;
    }

    private void Detonate()
    {
        active = true;
    }

    void Update()
    {
        duration++;
        if (duration > 15)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (active == true)
        {
            if (other.gameObject.GetComponent<TeamManager>() != null){
                if (Team != other.gameObject.GetComponent<TeamManager>().GetTeam())
                {
                    if (other.gameObject.tag == "Particle")
                    {
                        if (shield)
                        {
                            Destroy(other.gameObject);
                        }
                    }
                    else
                    {
                        if(other.gameObject.GetComponent<LifeManager>() != null){
                            if (!list.Contains(other.gameObject))
                            {
                                other.gameObject.GetComponent<LifeManager>().Damage(dmg);
                                GameObject obj = (GameObject)Instantiate(
                                    Spark,
                                    other.gameObject.transform.position,
                                    transform.rotation
                                );
                                list.Add(other.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }
}
