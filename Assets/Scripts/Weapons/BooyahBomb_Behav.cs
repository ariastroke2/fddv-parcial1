using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooyahBomb_Behav : MonoBehaviour
{
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

    private GameObject[] SphereAnim;

    private int cooldown;

    // Start is called before the first frame update
    void Start()
    {
        Team = GetComponent<TeamManager>().GetTeam();
        cooldown = 0;

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

        SphereAnim = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            SphereAnim[i] = transform.GetChild(1).transform.GetChild(i).gameObject;
            SphereAnim[i].GetComponent<Renderer>().material.color = tc;
        }
        transform.GetChild(1).gameObject.SetActive(false);

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
            if (onGround > 20 && onGround < 40)
            {
                if (onGround == 21)
                {
                    audio.PlayOneShot(bombInflate);
                }
                transform.GetChild(0).localScale += new Vector3(-0.025f, -0.025f, 0f);
            }
            if (onGround == 50)
            {
                //audio.PlayOneShot(bombInflate);
            }
            if (onGround >= 40)
            {
                if (onGround == 40)
                {
                    rb.isKinematic = true;
                    audio.PlayOneShot(bombBlast);
                    transform.GetChild(1).gameObject.SetActive(true);
                    GetComponent<Collider2D>().enabled = false;
                    transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
                transform.GetChild(1).localScale += new Vector3(
                    ((1f / onGround) - 39) / 110f,
                    ((1f / onGround) - 39) / 110f,
                    ((1f / onGround) - 39) / 200f
                );
                SphereAnim[0].transform.rotation = Quaternion.Euler(0, onGround * 10, 0);
                SphereAnim[1].transform.rotation = Quaternion.Euler(0, -onGround * 15, 0);
                SphereAnim[2].transform.rotation = Quaternion.Euler(0, onGround * 20, 0);
                SphereAnim[3].transform.rotation = Quaternion.Euler(0, -onGround * 20, 0);
                transform.GetChild(2).GetComponent<Light>().spotAngle += 0.15f;

                if (cooldown == 0 && onGround < 200)
                {
                    cooldown = 15;
                    Detonate();
                }
            }
            if (onGround > 200)
            {
                for (int i = 0; i < 4; i++)
                {
                    Color edit = SphereAnim[i].GetComponent<Renderer>().material.color;
                    edit = new Color(edit.r, edit.g, edit.b, ((300 - onGround) / 100f));
                    SphereAnim[i].GetComponent<Renderer>().material.color = edit;
                }
            }
            if (onGround > 300)
            {
                Debug.Log("Bomb entered deletion phase");
                Destroy(gameObject);
            }
            rb.velocity = new Vector2(rb.velocity.x * 0.9f, rb.velocity.y);
            rb.angularVelocity = rb.angularVelocity * 0.8f;
        }
        if (cooldown > 0)
        {
            cooldown--;
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
        blast.GetComponent<BombBlast>().size = transform.GetChild(1).localScale.x;
        blast.GetComponent<BombBlast>().dmg = 40;
        blast.GetComponent<BombBlast>().shield = true;
    }

    private Vector2 RandomDir()
    {
        Vector2 r = new Vector2(Random.Range(-10f, 10f), Random.Range(0f, 10f));
        r = r.normalized;
        return r;
    }
}
