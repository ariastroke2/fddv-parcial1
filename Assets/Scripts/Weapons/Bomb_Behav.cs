using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Behav : MonoBehaviour
{
    [SerializeField]
    private GameObject shooterPrefab;

    [SerializeField]
    private GameObject BlastPrefab;

    private int onGround;

    private int Team;

    [SerializeField]
    private int dmgRad;

    [SerializeField]
    private int lethalRad;

    private AudioSource audio;

    [SerializeField]
    private AudioClip bombSpawn;

    [SerializeField]
    private AudioClip bombInflate;

    [SerializeField]
    private AudioClip bombBlast;

    private Rigidbody2D rb;

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
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = tc;

        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        onGround = 0;
        audio.PlayOneShot(bombSpawn);
        rb.AddTorque(-3f, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.05f)
        {
            onGround++;
            Debug.Log("Bomb counter / " + onGround);
            if (onGround > 50 && onGround < 80)
            {
                transform.GetChild(0).localScale += new Vector3(0.025f, 0.025f, 0f);
            }
            if (onGround == 50)
            {
                audio.PlayOneShot(bombInflate);
            }
            if (onGround == 80)
            {
                Detonate();
            }
            if (onGround > 120)
            {
                Debug.Log("Bomb entered deletion phase");
                Destroy(gameObject);
            }
            rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
            rb.angularVelocity = rb.angularVelocity * 0.8f;
        }
    }

    private void Detonate()
    {
        GameObject blast = (GameObject)Instantiate(
            BlastPrefab,
            transform.position,
            transform.rotation
        );
        blast.GetComponent<BombBlast>().Team = Team;
        blast.GetComponent<BombBlast>().size = lethalRad;
        blast.GetComponent<BombBlast>().dmg = 120;
        for (int i = 0; i < 15; i++)
        {
            Vector2 direction = RandomDir();
            Vector2 newSpeed = new Vector2(
                direction.x * dmgRad * Random.Range(0.5f, 1f),
                direction.y * dmgRad * Random.Range(0.5f, 1f)
            );
            GameObject obj = (GameObject)Instantiate(
                shooterPrefab,
                new Vector2(transform.position.x, transform.position.y + 1f),
                transform.rotation
            );
            obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
            obj.GetComponent<Bullet_Behaviour>().maxDistance = 1000;
            obj.GetComponent<Bullet_Behaviour>().dmg = 30;
            obj.GetComponent<Bullet_Behaviour>().size = 0.8f;
            obj.GetComponent<TeamManager>().UpdateTeam(Team);
        }
        audio.PlayOneShot(bombBlast);
        //gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        rb.isKinematic = true;
        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(1).gameObject);
    }

    private Vector2 RandomDir()
    {
        Vector2 r = new Vector2(Random.Range(-10f, 10f), Random.Range(0f, 10f));
        r = r.normalized;
        return r;
    }
}
