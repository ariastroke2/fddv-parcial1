using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPointer : MonoBehaviour
{

    public GameObject dragPrefab;
    public GameObject Shield;
    public AudioClip shot;
    public AudioClip ShieldOpen;
    public AudioClip ShieldClose;
    public AudioClip ShieldBreak;
    public SpriteRenderer ShieldState;
    public InkManager InkM;

    public Sprite BrellaUP;
    public Sprite BrellaDOWN;

    public float OffsetY = 0;
    public float OffsetX = 0;
    public int Pellets = 10;
    public int CustomCooldown = 420;
    public float MaxPelletRange = 30f;
    public float MinPelletRange = 25f;
    public int ShieldRecover = 1000;

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
        Vector2 direction = (
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - 
            ((Vector2)transform.position)
        ).normalized;
        transform.right = direction;

        int dir;

        if(direction.x < 0){
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
                    Vector2 newSpeed = new Vector2(direction.x*Random.Range(MinPelletRange, MaxPelletRange), direction.y*Random.Range(MinPelletRange, MaxPelletRange)+0.1f);
                    GameObject obj = (GameObject)Instantiate(dragPrefab, newPos, transform.rotation);
                    obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
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
