using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPointer : MonoBehaviour
{

    [SerializeField] private GameObject shooterPrefab;
    [SerializeField] private GameObject Shield;
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip ShieldOpen;
    [SerializeField] private AudioClip ShieldClose;
    [SerializeField] private AudioClip ShieldBreak;
    [SerializeField] private SpriteRenderer ShieldState;
    [SerializeField] private InkManager InkM;

    [SerializeField] private Sprite BrellaUP;
    [SerializeField] private Sprite BrellaDOWN;

    [SerializeField] private MousePointer_Full DirectionImport;

    [SerializeField] private float OffsetY = 0;
    [SerializeField] private float OffsetX = 0;
    [SerializeField] private int Pellets = 10;
    [SerializeField] private int CustomCooldown = 420;
    [SerializeField] private float PelletSpeed;
    [SerializeField] private float WeaponRNG;
    [SerializeField] private int ShieldRecover;
    [SerializeField] private int MaxRange;

    private AudioSource audio;
    private LifeManager BrellaHP;

    private int animRep = 0;
    private int cooldown = 0;
    private int shieldup = 0;
    private int animShield = 0;
    private int ShieldCooldown = 0;
    private bool ShieldStateUpdated = false;

    void Start()
    {
        GetComponent<Transform>();
        BrellaHP = Shield.GetComponent<LifeManager>();
        audio = GetComponent<AudioSource>();
        Shield.transform.localScale = new Vector2(2f, 1f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = DirectionImport.direction;
        transform.right = direction;
        
        int dir;

        if(direction.x <= 0){
            transform.localScale = new Vector2(-1, -1);
            dir = -1;
        }
        else{
            transform.localScale = new Vector2(1, 1);
            dir = 1;
        }

        if (Input.GetMouseButton(0) && InkM.getInk() > 10){
            if(cooldown < 1){
                animRep = 60; 
                InkM.Consume(7);
                InkM.Cannot();
                cooldown = CustomCooldown;
                audio.PlayOneShot(shot);
                for(int i = 0; i < Pellets; i++){
                    Vector2 newPos = new Vector2(transform.position.x + (direction.x*2.5f)+Random.Range(-0.5f, 0.5f), transform.position.y+Random.Range(-0.5f, 0.5f) + (direction.y*1f));
                    Vector2 newSpeed = new Vector2(direction.x*PelletSpeed+Random.Range(-WeaponRNG, WeaponRNG), direction.y*PelletSpeed+Random.Range(-WeaponRNG, WeaponRNG));
                    GameObject obj = (GameObject)Instantiate(shooterPrefab, newPos, transform.rotation);
                    obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
                    obj.GetComponent<Bullet_Behaviour>().maxDistance = MaxRange;
                }
                if(shieldup == 0){
                    audio.PlayOneShot(ShieldOpen);
                    shieldup = 1;
                    animShield = 90;
                    Shield.tag = "Brella";
                }
            }
        }else{
            if(shieldup == 1){
                InkM.Can();
                audio.PlayOneShot(ShieldClose);
                shieldup = 0;
                animShield = 90;
                Shield.tag = "Untagged";
            }
        }

        
        if(BrellaHP.Life() < 1){
            if(!ShieldStateUpdated){
                    audio.PlayOneShot(ShieldBreak);
                    ShieldState.sprite = BrellaDOWN;
                    ShieldStateUpdated = true;
                    BrellaHP.Set(-150);
                }
            }
        if(BrellaHP.Life() == -150){
            ShieldCooldown++;
            if(ShieldCooldown > ShieldRecover){
                BrellaHP.Set(50);
                ShieldCooldown = 0;
                ShieldState.sprite = BrellaUP;
                ShieldStateUpdated = false;
            }
        }

        if(animShield > 15){
            if(shieldup == 1){
                Shield.transform.localScale = new Vector2(Mathf.Sin((animShield)*Mathf.Deg2Rad)*2f, Mathf.Cos((animShield+15)*Mathf.Deg2Rad)*2f);
            }else{
                Shield.transform.localScale = new Vector2(Mathf.Cos((animShield+15)*Mathf.Deg2Rad)*2f, Mathf.Sin(animShield*Mathf.Deg2Rad)*2f);
            }
            animShield-=10;
        }

        if(cooldown > 0)
            cooldown--;

        if(animRep > 1){
            animRep-=7;
            transform.position = new Vector2(transform.parent.transform.position.x - (animRep*dir*0.01f) + OffsetX*dir, transform.parent.transform.position.y + OffsetY);
        }

        

    }
}
