using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoBomber : MonoBehaviour
{
    public int Cooldown = 500;
    public GameObject Bullet;
    public GameObject Drop;
    private TargetFinder finder;
    private GameObject SpawnAnim;
    private GameObject Target;
    private Animator animator;

    public float BulletSpeed;
    private bool Cont = true;

    private AudioSource audio;
    public AudioClip SpawnSound;
    public AudioClip ShootSound;
    public AudioClip DieSound;

    public float Offshoot;

    public float VoicePitch;

    private int progress = 0;
    private int dir = 0;

    private int Team;
    private int SpawnC = 0;
    private bool spawning = true;
    private SpriteRenderer MyRenderer;

    private Camera main;

    void Start()
    {
        Team = GetComponent<TeamManager>().GetTeam();
        progress = Random.Range(0, Cooldown);

        MyRenderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        finder = transform.Find("FinderRange").GetComponent<TargetFinder>();
        SpawnAnim = transform.Find("Spawner").gameObject;

        MyRenderer.enabled = false;
        SpawnAnim.SetActive(false);
        audio.pitch = VoicePitch;
        audio.PlayOneShot(SpawnSound);
        

        main = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!spawning)
        {
            Target = finder.FoundTarget();

            if (Target != null)
            {
                Vector2 direction = (
                    (Vector2)Target.transform.position - (Vector2)transform.position
                ).normalized;

                if (direction.x < 0)
                {
                    transform.localScale = new Vector2(-1.5f, transform.localScale.y);
                    dir = -1;
                }
                else
                {
                    transform.localScale = new Vector2(1.5f, transform.localScale.y);
                    dir = 1;
                }

                if (Cont)
                {
                    progress += 1;
                    animator.SetInteger("Progress", progress);
                    if (progress > Cooldown)
                    {
                        audio.PlayOneShot(ShootSound);
                        Shoot(Target.transform.position);
                        progress = 0;
                    }
                }
            }
            else
            {
                progress = 9;
                animator.SetInteger("Progress", 9);
            }
        }
        else
        {
            if (SpawnC == 0)
            {
                SpawnAnim.SetActive(true);
            }
            SpawnAnimationUpd();
        }
    }

    // Methods

    private void Shoot(Vector2 position) // Crea un proyectil nuevo
    {
        GameObject obj = (GameObject)Instantiate(Bullet, transform.position, transform.rotation);
        obj.GetComponent<Rigidbody2D>().velocity = Trajectory(position, Offshoot);
        obj.GetComponent<TeamManager>().UpdateTeam(Team);
    }

    private void SpawnAnimationUpd() // Actualiza la animación de spawn del enemigo
    {
        SpawnC += 15;
        SpawnAnim.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, SpawnC * 0.5f);
        SpawnAnim.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, SpawnC * -0.7f);
        SpawnAnim.transform.GetChild(2).transform.rotation = Quaternion.Euler(0, 0, SpawnC * 1f);
        SpawnAnim.transform.GetChild(0).transform.localScale = new Vector2(
            1 - (SpawnC * Mathf.Abs(Mathf.Cos(SpawnC * Mathf.Deg2Rad)) / 1000f),
            1 - (SpawnC / 1000f)
        );
        SpawnAnim.transform.GetChild(1).transform.localScale = new Vector2(
            1 - (SpawnC / 1000f),
            1 - (SpawnC * Mathf.Abs(Mathf.Sin(SpawnC * Mathf.Deg2Rad)) / 1000f)
        );
        SpawnAnim.transform.GetChild(2).transform.localScale = new Vector2(
            (SpawnC / 500f),
            (SpawnC * Mathf.Sin(SpawnC * Mathf.Deg2Rad) / 500f)
        );

        foreach (Transform child in SpawnAnim.transform.GetChild(1).transform)
        {
            child.gameObject.transform.localScale = new Vector2((SpawnC / 500f), (SpawnC / 500f));
        }
        foreach (Transform child in SpawnAnim.transform.GetChild(0).transform)
        {
            child.gameObject.transform.localScale = new Vector2((SpawnC / 500f), (SpawnC / 500f));
        }
        if (SpawnC > 1000)
        {
            Destroy(SpawnAnim);
            spawning = false;
            MyRenderer.enabled = true;
        }
    }

    public void Kill() // Destruye al enemigo, es la "animacion" de muerte del enemigo.
    {
        if(GetComponent<TrackedObject>() != null)
			if(GetComponent<TrackedObject>().IsActive())
				GetComponent<TrackedObject>().SendSignal();
        AudioSource.PlayClipAtPoint(DieSound, main.transform.position);
        for (int i = 0; i < 10; i++)
        {
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 newSpeed = new Vector2(Random.Range(-6f, 6f), Random.Range(5f, 8f));
            GameObject obj = (GameObject)Instantiate(Drop, newPos, transform.rotation);
            obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
            obj.GetComponent<Rigidbody2D>().gravityScale = 3;
            if(obj.GetComponent<Bullet_Behaviour>()!= null){
                obj.gameObject.transform.localScale = new Vector2(0.6f, 0.6f);
                obj.GetComponent<Bullet_Behaviour>().phasing = true;
            }
            else{
                i = 11;
            }
            obj.GetComponent<TeamManager>().UpdateTeam(Team);
            
        }
    }

    private Vector2 Trajectory(Vector2 TargetPosition, float Overshoot){
        //TargetPosition = TargetPosition - (Vector2)transform.position;

        float TargetDistanceX = TargetPosition.x - transform.position.x;
        float TargetDistanceY = (TargetPosition.y - transform.position.y);
        //if(TargetDistanceY < 0)
            //TargetDistanceY = 0;

        float ComponentY = 0f;
        float FirstHalfTime = 0f;
        float SecondHalfTime = 0f;
        float ComponentX = 0f;

        if(TargetDistanceY <= -Overshoot ){
            ComponentY = Mathf.Sqrt((9.81f * 3) * (Overshoot));
            FirstHalfTime = -ComponentY / (-9.81f * 3);
            SecondHalfTime = Mathf.Sqrt(((Overshoot + TargetDistanceY) * 2) / (((-9.81f * 3))));

            float TotalTime = FirstHalfTime + SecondHalfTime;

            if(VoicePitch == 1f)
                ComponentX = (TargetDistanceX * 0.5f) / (TotalTime);
            if(VoicePitch == 0.5f)
                ComponentX = (TargetDistanceX * 1f) / (TotalTime);
        }else{
            ComponentY = Mathf.Sqrt(88.29f * ((TargetDistanceY) + Overshoot));
            FirstHalfTime = -ComponentY / (-9.81f * 3);
            SecondHalfTime = Overshoot / (((9.81f * 3)) / 2);

            float TotalTime = FirstHalfTime + SecondHalfTime;

            if(VoicePitch == 1f)
                ComponentX = (TargetDistanceX * 0.65f) / (TotalTime);
            if(VoicePitch == 0.5f)
                ComponentX = (TargetDistanceX * 1f) / (TotalTime);
        }

        


        return new Vector2(ComponentX, ComponentY);
    }

    public TargetFinder TargetSettings()
    {
        return finder;
    }
}
