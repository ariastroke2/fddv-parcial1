using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstBomb_Behav : MonoBehaviour
{
    [SerializeField]
    private GameObject shooterPrefab;

    [SerializeField]
    private GameObject BlastPrefab;

    private int Team;

    [SerializeField]
    private int dmgRad;

    [SerializeField]
    private int lethalRad;

    private AudioSource audio;

    [SerializeField]
    private AudioClip bombSpawn;

    [SerializeField]
    private AudioClip bombBlast;

    private Rigidbody2D rb;
    private int timer;

    // Start is called before the first frame update
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
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = tc;

        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        timer = 0;
        audio.PlayOneShot(bombSpawn);
        rb.AddTorque(-10f, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.isKinematic)
        {
            timer++;
            if (timer > 40)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Detonate()
    {
        for (int i = 0; i < 25; i++)
        {
            Vector2 direction = RandomDir();
            Vector2 newSpeed = new Vector2(
                direction.x * dmgRad * Random.Range(0.5f, 1f),
                direction.y * dmgRad * Random.Range(0.5f, 1f)
            );
            GameObject obj = (GameObject)Instantiate(
                shooterPrefab,
                new Vector2(
                    transform.position.x + 2f * direction.x,
                    transform.position.y + 2f * direction.y
                ),
                transform.rotation
            );
            obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
            obj.GetComponent<Bullet_Behaviour>().maxDistance = 1000;
            obj.GetComponent<Bullet_Behaviour>().dmg = 7;
            obj.GetComponent<Bullet_Behaviour>().size = 0.8f;
        }
        audio.PlayOneShot(bombBlast);
        //gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        Destroy(transform.GetChild(0).gameObject);
        rb.isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.gameObject.tag != "Particle"
            && other.gameObject.tag != "Brella"
            && other.gameObject.tag != "BrellaClose"
        )
        {
            Detonate();
        }
    }

    private Vector2 RandomDir()
    {
        Vector2 r = new Vector2(Random.Range(-10f, 10f), Random.Range(0f, 10f));
        r = r.normalized;
        return r;
    }
}
