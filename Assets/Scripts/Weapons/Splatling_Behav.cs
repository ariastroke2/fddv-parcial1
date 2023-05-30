using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splatling_Behav : MonoBehaviour
{
    [SerializeField]
    private GameObject shooterPrefab;
    public InkManager InkM;

    private AudioSource audio;

    [SerializeField]
    private AudioClip shot;

    [SerializeField]
    private AudioClip ChargeReticle;

    [SerializeField]
    private AudioClip ChargeStart;

    public MousePointer_Full DirectionImport;

    [SerializeField]
    private float OffsetY = 0;

    [SerializeField]
    private float OffsetX = 0;
    public int MaxCharge;
    public int Charge;

    [SerializeField]
    private int ChargeSpeed;
    private bool Release;
    private bool ReleasedCharge;

    [SerializeField]
    private int CustomCooldown = 420;

    [SerializeField]
    private int ChargeCooldown = 420;

    [SerializeField]
    private float PelletSpeed;

    [SerializeField]
    private float WeaponRNG;

    [SerializeField]
    private int MaxRange;

    private int Team;

    private int animRep = 0;
    private int state = 0;
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
        ReleasedCharge = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = DirectionImport.direction;
        if (Input.GetMouseButton(0))
        {
            if (InkM.getInk() > 1 && ReleasedCharge)
            {
                InkM.Cannot();
                if (Release)
                {
                    Release = false;
                    state = 0;
                    audio.pitch = 1f;
                    audio.PlayOneShot(ChargeStart);
                }
                else
                {
                    if (Charge < MaxCharge && cooldown < 1 && InkM.getInk() > Charge)
                    {
                        Charge += ChargeSpeed;
                        cooldown = ChargeCooldown;
                        if (Charge >= MaxCharge / 2 && state == 0)
                        {
                            state = 1;
                            audio.pitch = 2f;
                            audio.PlayOneShot(ChargeReticle);
                        }
                        if (Charge >= MaxCharge && state == 1)
                        {
                            audio.pitch = 5f;
                            state = 2;
                            audio.PlayOneShot(ChargeReticle);
                        }
                    }
                }
            }
        }
        else
        {
            if (Release && ReleasedCharge)
                InkM.Can();
            Release = true;
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

        if (Release && Charge > 0)
        {
            if (cooldown < 1)
            {
                Charge--;
                ReleasedCharge = false;
                animRep = 60;
                InkM.Consume(1);
                InkM.Cannot();
                cooldown = CustomCooldown;
                audio.pitch = 0.5f;
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
                obj.GetComponent<TeamManager>().UpdateTeam(Team);
                if (state > 0)
                {
                    obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
                    obj.GetComponent<Bullet_Behaviour>().size = 0.5f;
                    obj.GetComponent<Bullet_Behaviour>().maxDistance = MaxRange;
                    obj.GetComponent<Bullet_Behaviour>().dmg = 45;
                }
                else
                {
                    obj.GetComponent<Rigidbody2D>().velocity = newSpeed * 0.5f;
                    obj.GetComponent<Bullet_Behaviour>().size = 0.4f;
                    obj.GetComponent<Bullet_Behaviour>().maxDistance = MaxRange * 0.75f;
                    obj.GetComponent<Bullet_Behaviour>().dmg = 25;
                }
            }
            if (Charge < 1)
            {
                ReleasedCharge = true;
            }
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
