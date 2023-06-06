using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{
    public AudioClip d;
    private int Team;
    public AudioClip hit;

    public float maxDistance;
    public int dmg;
    public bool phasing = false;
    public float size;
    private Vector2 OriginalPosition;
    private bool falloffDmg;

    [SerializeField]
    private int behav = 0;

    [SerializeField]
    private GameObject CustomSpark;

    private Camera main;

    private Rigidbody2D rb;

    void Start()
    {
        Team = GetComponent<TeamManager>().GetTeam();

		Color tc;
        if (Team == 1)
        {
            tc = new Color(0.973f, 0f, 1f, 1f);
        }
        else if (Team == 2)
        {
            tc = new Color(1f, 0.95294f, 0f, 1f);
        }
        else
        {
            tc = new Color(1f, 1f, 1f, 1f);
        }

        GetComponent<SpriteRenderer>().color = tc;

        main = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        falloffDmg = false;
        transform.localScale = new Vector2(size, size);
        OriginalPosition = (Vector2)transform.position;
    }

    void Update()
    {
        if (behav == 0)
        {
            // Bullets that have no fall-off
        }
        if (behav == 1)
        {
            // Bullets that have massive fall-off (Useful for limiting weapon ranges, i guess)
            if (((Vector2)transform.position - OriginalPosition).magnitude > maxDistance)
            {
                rb.velocity = new Vector2(rb.velocity.x * 0.7f, Mathf.Abs(rb.velocity.y) * -1.2f);
                if(!falloffDmg){
                    falloffDmg = true;
                    dmg = dmg / 4;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground" && other.gameObject.GetComponent<PlatformEffector2D>() == null)
        {
            AudioSource.PlayClipAtPoint(d, transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (!phasing)
        {
			if(other.gameObject.tag != "Particle" && Team != 0 && other.gameObject.GetComponent<LifeManager>() != null){
				if(Team != other.gameObject.GetComponent<TeamManager>().GetTeam()){

					other.gameObject.GetComponent<LifeManager>().Damage(dmg);

						GameObject obj = (GameObject)Instantiate(
							CustomSpark,
							transform.position,
							transform.rotation
						);
						gameObject.SetActive(false);
						Destroy(gameObject);
				}
			}
        }
    }
}
