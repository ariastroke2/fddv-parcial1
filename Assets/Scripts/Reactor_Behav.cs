using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor_Behav : MonoBehaviour
{

    public GameObject ExplodePrefab;

    private Transform circle;
    private Transform sphere;
    private Transform outercircle;
    private Transform outersphere;

    private bool KillAnim;
    private LifeManager lf;

    private AudioSource audio;
    public AudioClip charge;
    public AudioClip detonate;

    public AudioClip[] HitSound;

    private int OldHP;

    private int Team;

    private float ReactorScaleOG;

    private bool DetonteOnce;

    // Start is called before the first frame update
    void Start()
    {
        lf = GetComponent<LifeManager>();
        audio = GetComponent<AudioSource>();
        Team = GetComponent<TeamManager>().GetTeam();
        ReactorScaleOG = transform.localScale.x;
        DetonteOnce = false;
        circle = transform.Find("Circle");
        outercircle = transform.Find("OuterCircle");
        sphere = transform.Find("Sphere");
        outersphere = transform.Find("OuterSphere");
        sphere.gameObject.GetComponent<Renderer>().material.color = new Color(0f, 0.903f, 0.95f, 1f);
        outersphere.gameObject.GetComponent<Renderer>().material.color = new Color(0f, 0.903f, 0.95f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!KillAnim){
            sphere.rotation = Quaternion.Euler(0, Time.time * 90f, 0);
            outersphere.rotation = Quaternion.Euler(0, Time.time * -120f, 0);
            circle.rotation = Quaternion.Euler(0, 0, Time.time * 90f);
            outercircle.rotation = Quaternion.Euler(0, 0, Time.time * -160f);
            if(lf.Life() < 1){
                Kill();
                audio.PlayOneShot(charge);
            }
        }else{
            sphere.rotation = Quaternion.Euler(0, Time.time * 120f, 0);
            outersphere.rotation = Quaternion.Euler(0, Time.time * -150f, 0);
            circle.rotation = Quaternion.Euler(0, 0, Time.time * 120f);
            outercircle.rotation = Quaternion.Euler(0, 0, Time.time * -190f);
            sphere.localScale = sphere.localScale / 1.05f;
            outersphere.localScale -= new Vector3(0.03f, 0.03f, 0.03f);
            circle.localScale = circle.localScale/1.05f;
            outercircle.localScale = outercircle.localScale/1.05f;
            if(outersphere.localScale.x < 0.1){
                if(!DetonteOnce)
                    Detonate();
            }
            if(outersphere.localScale.x < -5){
                Destroy(gameObject);
            }
        }

        if(OldHP!=lf.Life()){
            if(OldHP > lf.Life()){
                audio.PlayOneShot(HitSound[Random.Range(0, HitSound.Length)]);
                transform.localScale -= new Vector3(0.8f, 0.8f, 0.8f);
            }
            OldHP = lf.Life();
        }
        float value = ReactorScaleOG + (transform.localScale.x - ReactorScaleOG) / 2;
        transform.localScale = new Vector3(value, value, value);
    }

    public void Kill(){
        KillAnim = true;
    }

    public void Detonate()
    {
        DetonteOnce = true;
        audio.PlayOneShot(detonate);
        for (int i = 0; i < 50; i++)
        {
            Vector2 direction = RandomDir();
            Vector2 newSpeed = new Vector2(
                direction.x * Random.Range(15f, 20f),
                direction.y * Random.Range(15f, 20f)
            );
            GameObject obj = (GameObject)Instantiate(
                ExplodePrefab,
                new Vector2(
                    transform.position.x,
                    transform.position.y
                ),
                transform.rotation
            );
            obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
            obj.GetComponent<Bullet_Behaviour>().maxDistance = 1000;
            obj.GetComponent<Bullet_Behaviour>().phasing = true;
            obj.GetComponent<Bullet_Behaviour>().size = 1.4f;
            obj.GetComponent<TeamManager>().UpdateTeam(Team);
        }
        //audio.PlayOneShot(bombBlast);
        //gameObject.SetActive(false);
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(false);
            //cont.GetChild(i).gameObject.SetActive(false);
        }
        
        //rb.isKinematic = true;
    }

    private Vector2 RandomDir()
    {
        Vector2 r = new Vector2(Random.Range(-10f, 10f), Random.Range(0f, 10f));
        r = r.normalized;
        return r;
    }
}
