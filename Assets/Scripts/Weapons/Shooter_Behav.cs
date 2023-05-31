using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Behav : MonoBehaviour
{
    [SerializeField]
    private GameObject shooterPrefab;
    public InkManager InkM;

    private AudioSource audio;

    [SerializeField]
    private AudioClip shot;

    public MousePointer_Full DirectionImport;

    [SerializeField]
    private float OffsetY = 0;

    [SerializeField]
    private float OffsetX = 0;

    [SerializeField]
    private int CustomCooldown = 420;

    [SerializeField]
    private float PelletSpeed;

    [SerializeField]
    private float WeaponRNG;

    [SerializeField]
    private int MaxRange;

    private int Team;
    private int animRep = 0;
    private int cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (DirectionImport == null)
        {
            GameObject temp = GameObject.Find("Player");
            InkM = temp.GetComponent<InkManager>();
            DirectionImport = temp.GetComponent<MousePointer_Full>();
            Team = temp.GetComponent<TeamManager>().GetTeam();
        }
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = DirectionImport.direction;
        if (Input.GetMouseButton(0) && InkM.getInk() > 3)
        {
            if (cooldown < 1)
            {
                animRep = 60;
                InkM.Consume(2);
                InkM.Cannot();
                cooldown = CustomCooldown;
                audio.PlayOneShot(shot);
                // Bullet
                Vector2 newPos = new Vector2(
                    transform.position.x + (direction.x * 2.5f) + Random.Range(-0.5f, 0.5f),
                    transform.position.y + Random.Range(-0.5f, 0.5f) + (direction.y * 1f)
                );
                Vector2 newSpeed = new Vector2(
                    direction.x * PelletSpeed + Random.Range(-WeaponRNG, WeaponRNG),
                    direction.y * PelletSpeed + Random.Range(-WeaponRNG, WeaponRNG)
                );
                GameObject obj = (GameObject)Instantiate(shooterPrefab, newPos, transform.rotation);
                obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
                obj.GetComponent<Bullet_Behaviour>().maxDistance = MaxRange;
                obj.GetComponent<Bullet_Behaviour>().dmg = 30;
                obj.GetComponent<Bullet_Behaviour>().size = 0.8f;
                obj.GetComponent<TeamManager>().UpdateTeam(Team);
            }
        }
        else
        {
            InkM.Can();
        }
    }

    void FixedUpdate()
    {
        Vector2 direction = DirectionImport.direction;
        transform.right = direction;

        if (direction.x <= 0)
        {
            transform.localScale = new Vector2(-1, -1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }

        if (cooldown > 0)
            cooldown--;

        if (animRep > 1)
        {
            animRep -= 7;
            transform.position = new Vector2(
                transform.parent.transform.position.x
                    - (animRep * (direction.x) * 0.01f)
                    + OffsetX * (direction.x / Mathf.Abs(direction.x)),
                transform.parent.transform.position.y - (animRep * (direction.y) * 0.01f) + OffsetY
            );
        }
    }
}
