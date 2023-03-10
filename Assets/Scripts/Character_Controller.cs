using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{

    public Animator animator;

    public float movementSpeed = 1f;
    public float maxSpeed = 5f;
    public float jumpForce = 1f;

    public GameObject particle;

    public AudioClip death;

    public GameObject GameOverCanva;

    float xSpeed;

    private Camera main;

    private Rigidbody2D rigidBody;
    private MousePointer_Full MPointer;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        GameOverCanva.SetActive(false);
        rigidBody = GetComponent<Rigidbody2D>();
        MPointer = GetComponent<MousePointer_Full>();
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        xSpeed = Input.GetAxisRaw("Horizontal");
        if(xSpeed != 0){
            rigidBody.AddForce(new Vector2(xSpeed, 0) * movementSpeed, ForceMode2D.Impulse);
            if(rigidBody.velocity.x > maxSpeed)
                rigidBody.velocity = new Vector2(maxSpeed, rigidBody.velocity.y);
            if(rigidBody.velocity.x < -maxSpeed)
                rigidBody.velocity = new Vector2(-maxSpeed, rigidBody.velocity.y);
        }else if(Mathf.Abs(rigidBody.velocity.y) < 0.02){
            rigidBody.velocity = new Vector2(rigidBody.velocity.x * 0.9f, rigidBody.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rigidBody.velocity.y) < 0.02)
        {
            rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        animator.SetFloat("SpeedX", MPointer.direction.x*(rigidBody.velocity.x));
        animator.SetFloat("SpeedY", rigidBody.velocity.y);
        
        if (Input.GetButton("Jump"))
            rigidBody.gravityScale = 5;
        else
            rigidBody.gravityScale = 6;

        if(transform.position.y < -30){
            LifeManager oops = GetComponent<LifeManager>();
            oops.Set(0);
            Debug.Log("Out of bounds kill");
        }

    }

    public void Kill(){
        AudioSource.PlayClipAtPoint(death, main.transform.position);
        for(int i = 0; i < 10; i++){
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 newSpeed = new Vector2(Random.Range(-6f, 6f), Random.Range(5f, 8f));
            GameObject obj = (GameObject)Instantiate(particle, newPos, transform.rotation);
            obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
            obj.GetComponent<Rigidbody2D>().gravityScale = 3;
            obj.GetComponent<Bullet_Behaviour>().Team = 3;
            obj.gameObject.transform.localScale = new Vector2(0.6f, 0.6f);
        }
    }
}
