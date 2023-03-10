using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoTrooper : MonoBehaviour
{
    public int Cooldown = 500;
    public GameObject Bullet;
    public GameObject Target;
    public GameObject Notify;
    public Animator animator;
    public GameObject SpawnAnim;
    public float BulletSpeed;
    public bool Cont = true;

    private AudioSource audio;
    public AudioClip SpawnS;
    public AudioClip DieS;

    private int progress = 0;
    private int dir = 0;

    private int SpawnC = 0;
    private bool spawning = true;
    private SpriteRenderer MyRenderer;

    private Camera main;

    void Start(){
        progress = Random.Range(0, Cooldown);
        MyRenderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        MyRenderer.enabled = false;
        audio.PlayOneShot(SpawnS);
        main = Camera.main;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!spawning){
            Vector2 direction = (
                    (Vector2)Target.transform.position - 
                    (Vector2)transform.position
                ).normalized;

                if(direction.x < 0){
                    transform.localScale = new Vector2(-1.5f, transform.localScale.y);
                    dir = -1;
                }
                else{
                    transform.localScale = new Vector2(1.5f, transform.localScale.y);
                    dir = 1;
                }

            if(Cont)
            {
                progress += 1;
                animator.SetInteger("Progress", progress);
                if(progress > Cooldown){
                    Vector2 newPos = new Vector2(transform.position.x + (dir*2f), transform.position.y-(1f));
                    Vector2 newSpeed = new Vector2(direction.x * BulletSpeed, direction.y * BulletSpeed);
                    GameObject obj = (GameObject)Instantiate(Bullet, newPos, transform.rotation);
                    obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
                    progress = 0;
                }
            }
        }else{
            SpawnC += 15;
            SpawnAnim.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, SpawnC * 0.5f);
            SpawnAnim.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, SpawnC * -0.7f);
            SpawnAnim.transform.GetChild(2).transform.rotation = Quaternion.Euler(0, 0, SpawnC * 1f);
            SpawnAnim.transform.GetChild(0).transform.localScale = new Vector2(1-(SpawnC*Mathf.Abs(Mathf.Cos(SpawnC * Mathf.Deg2Rad))/1000f), 1-(SpawnC/1000f));
            SpawnAnim.transform.GetChild(1).transform.localScale = new Vector2(1-(SpawnC/1000f), 1-(SpawnC*Mathf.Abs(Mathf.Sin(SpawnC * Mathf.Deg2Rad))/1000f));
            SpawnAnim.transform.GetChild(2).transform.localScale = new Vector2((SpawnC/500f), (SpawnC*Mathf.Sin(SpawnC * Mathf.Deg2Rad)/500f));

            foreach (Transform child in SpawnAnim.transform.GetChild(1).transform)
            {
                child.gameObject.transform.localScale = new Vector2((SpawnC/500f), (SpawnC/500f));
            }
            foreach (Transform child in SpawnAnim.transform.GetChild(0).transform)
            {
                child.gameObject.transform.localScale = new Vector2((SpawnC/500f), (SpawnC/500f));
            }
            if(SpawnC > 1000){
                Destroy(SpawnAnim);
                spawning = false;
                MyRenderer.enabled = true;
            }
        }
    }

    public void Kill(){
        AudioSource.PlayClipAtPoint(DieS, main.transform.position);
        for(int i = 0; i < 10; i++){
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 newSpeed = new Vector2(Random.Range(-6f, 6f), Random.Range(5f, 8f));
            GameObject obj = (GameObject)Instantiate(Bullet, newPos, transform.rotation);
            obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
            obj.GetComponent<Rigidbody2D>().gravityScale = 3;
            obj.GetComponent<Bullet_Behaviour>().Team = 3;
            obj.gameObject.transform.localScale = new Vector2(0.6f, 0.6f);
        }
    }
}
