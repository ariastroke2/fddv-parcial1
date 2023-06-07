using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{

	public Animator animator;

	public float movementSpeed = 1f;
	public float maxSpeed = 5f;
	public float jumpForce = 1f;

	public float RespawnX;
	public float RespawnY;
	public float RespawnExit;

	public GameObject particle;

	private AudioSource audio;

	public AudioClip death;
	public AudioClip respawnJump;
	public AudioClip respawnJumpLand;


	private float xSpeed;
	private int state;
	private int timer;

	private bool EnterAnimation = false;

	private Camera main;

	private Rigidbody2D rigidBody;
	private MousePointer_Full MPointer;
	private LifeManager life;

	private int DownTime;
	private bool DownPhase;
	private List<GameObject> Phased;

	private GameObject bodysprite;

	// Start is called before the first frame update
	void Start()
	{
		gameObject.tag = "Player";		
		state = 0;
		timer = 0;
		audio = GetComponent<AudioSource>();
		Phased = new List<GameObject>();
		Application.targetFrameRate = 60;
		rigidBody = GetComponent<Rigidbody2D>();
		bodysprite = transform.Find("BodySprite").gameObject;
		life = GetComponent<LifeManager>();
		MPointer = GetComponent<MousePointer_Full>();
		main = Camera.main;
		if(EnterAnimation)
			EnterLaunch();
	}

	// Update is called once per frame
	void Update()
	{
		
		if(state == 0){

			xSpeed = Input.GetAxisRaw("Horizontal");

			if (Mathf.Abs(rigidBody.velocity.y) < 0.02)
			{
				if(Input.GetButtonDown("Jump"))
					rigidBody.velocity = new Vector2 (rigidBody.velocity.x, jumpForce);
			}else{
				xSpeed = xSpeed * 0.4f;
			}

			if(xSpeed != 0){
				rigidBody.velocity = new Vector2(rigidBody.velocity.x + xSpeed * movementSpeed, rigidBody.velocity.y);
				if(rigidBody.velocity.x > maxSpeed)
					rigidBody.velocity = new Vector2(maxSpeed, rigidBody.velocity.y);
				if(rigidBody.velocity.x < -maxSpeed)
					rigidBody.velocity = new Vector2(-maxSpeed, rigidBody.velocity.y);
			}else if(Mathf.Abs(rigidBody.velocity.y) < 0.02){
				rigidBody.velocity = new Vector2(rigidBody.velocity.x * 0.9f, rigidBody.velocity.y);
			}

			animator.SetFloat("SpeedX", MPointer.direction.x*(rigidBody.velocity.x));
			animator.SetFloat("SpeedY", rigidBody.velocity.y);
			
			if (Input.GetButton("Jump"))
				rigidBody.gravityScale = 4;
			else
				rigidBody.gravityScale = 10;

			if (Input.GetAxisRaw("Vertical") < 0){
				if(DownTime < 15){
					DownTime++;
				}else{
					if(!DownPhase)
						DownPhase = true;
				}
			}else{
				if(DownPhase){
					DownTime = 0;
					DownPhase = false;
					foreach (GameObject item in Phased) {
						Physics2D.IgnoreCollision(item.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
					}
					Phased.Clear();
				}
			}

			if(transform.position.y < -30){
				life.Set(0);
				Debug.Log("Out of bounds kill");
			}

			if(life.Life() < 1){
				Kill();
			}

		}else{
			timer++;
			if(timer == 240){
				RegenAnim();
			}else if(timer > 240){
				bodysprite.transform.right = rotate(rigidBody.velocity.normalized, Mathf.PI * 3 / 2);
				if(timer % 4 == 0){
					FewParticles(2);
				}
			}
			if(transform.position.x < RespawnX + RespawnExit && transform.position.x > RespawnX - RespawnExit){
				if(transform.position.y < RespawnY + RespawnExit && transform.position.y > RespawnY - RespawnExit){
					if(rigidBody.velocity.y < 0){
						if(state == 1)
							Regenerate();
						else if(state == 2)
							Destroy(gameObject);
						else if(state == 3)
							JumpLand();
					}
				}
			}
		}

	}

	private void FewParticles(int howMany){
		animator.SetInteger("State", 1);
		for(int i = 0; i < howMany; i++){
			GameObject obj = (GameObject)Instantiate(particle, transform.position, transform.rotation);
			obj.GetComponent<Rigidbody2D>().velocity = rotate(-rigidBody.velocity * 0.8f, Random.Range(-Mathf.PI / 4, Mathf.PI / 4));
			obj.GetComponent<Rigidbody2D>().gravityScale = 1;
			obj.GetComponent<TeamManager>().UpdateTeam(2);
			obj.GetComponent<Bullet_Behaviour>().maxDistance = 1000;
			obj.GetComponent<Bullet_Behaviour>().size = 1f;
			obj.GetComponent<Bullet_Behaviour>().phasing = true;
		}
	}

	public void Exit(Vector2 ExitJump){

		gameObject.tag = "Particle";
		state = 2;
		timer = 250;
		GetComponent<Collider2D>().enabled = false;
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
		transform.Find("BodySprite").gameObject.SetActive(true);

		animator.SetInteger("State", 1);
		animator.SetFloat("SpeedX", 0);
		animator.SetFloat("SpeedY", 0);

		audio.PlayOneShot(respawnJump);

		rigidBody.velocity = Trajectory(ExitJump, 20f);

		RespawnX = ExitJump.x;
		RespawnY = ExitJump.y;

		rigidBody.isKinematic = false;
	}

	public void Enter(Vector2 EnterJump){
		
		EnterAnimation = true;
		transform.position = EnterJump;

	}

	private void EnterLaunch(){
		gameObject.tag = "Particle";
		timer = 250;
		state = 3;
		GetComponent<Collider2D>().enabled = false;
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}
		transform.Find("BodySprite").gameObject.SetActive(true);


		animator.SetInteger("State", 1);

		audio.PlayOneShot(respawnJump);
		

		rigidBody.velocity = Trajectory(new Vector2(RespawnX, RespawnY), 20f);
		rigidBody.isKinematic = false;
	}

	public void Regenerate(){
		animator.SetInteger("State", 0);
		audio.PlayOneShot(respawnJumpLand);
		state = 0;
		gameObject.tag = "Player";
		life.Set(30);
		bodysprite.transform.right = new Vector2(0, 0);
		GetComponent<Collider2D>().enabled = true;
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	public void JumpLand(){
		animator.SetInteger("State", 0);
		audio.PlayOneShot(respawnJumpLand);
		state = 0;
		gameObject.tag = "Player";
		bodysprite.transform.right = new Vector2(0, 0);
		GetComponent<Collider2D>().enabled = true;
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(true);
		}
	}

	private void RegenAnim(){
		animator.SetInteger("State", 1);
		animator.SetFloat("SpeedX", 0);
		animator.SetFloat("SpeedY", 0);
		audio.PlayOneShot(respawnJump);
		rigidBody.velocity = Trajectory(new Vector2(RespawnX, RespawnY), 20f);
		rigidBody.isKinematic = false;
		life.CanRegenerate();
		transform.Find("BodySprite").gameObject.SetActive(true);
		
	}

	private Vector2 Trajectory(Vector2 TargetPosition, float Overshoot){

        float TargetDistanceX = TargetPosition.x - transform.position.x;
        float TargetDistanceY = TargetPosition.y - transform.position.y;

		float MaxPointY = TargetDistanceY + Overshoot;
		if(TargetDistanceY < 0)
			MaxPointY = transform.position.y + Overshoot;

        float ComponentY = 0f;
		float ComponentX = 0f;
        float Time = 0f;

		ComponentY = Mathf.Sqrt(2 * (9.81f * rigidBody.gravityScale) * (MaxPointY));

		Time = Mathf.Sqrt(2 * (9.81f * rigidBody.gravityScale) * (MaxPointY - TargetDistanceY)) + ComponentY;
		Time = Time / (9.81f * rigidBody.gravityScale);

		ComponentX = (TargetDistanceX * 1f) / (Time);

        return new Vector2(ComponentX, ComponentY);
    }

	public void Kill(){
		audio.PlayOneShot(death);
		gameObject.tag = "Particle";
		timer = 0;
		state = 1;
		GetComponent<Collider2D>().enabled = false;
		rigidBody.isKinematic = true;
		rigidBody.velocity = new Vector2(0, 0);
		life.CannotRegenerate();
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}

		for(int i = 0; i < 10; i++){
			Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
			Vector2 newSpeed = new Vector2(Random.Range(-6f, 6f), Random.Range(5f, 8f));
			GameObject obj = (GameObject)Instantiate(particle, newPos, transform.rotation);
			obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
			obj.GetComponent<Rigidbody2D>().gravityScale = 3;
			obj.GetComponent<TeamManager>().UpdateTeam(2);
			obj.GetComponent<Bullet_Behaviour>().maxDistance = 1000;
            obj.GetComponent<Bullet_Behaviour>().size = 0.8f;
			obj.GetComponent<Bullet_Behaviour>().phasing = true;
		}
	}

	public void KillPermanent(){
		AudioSource.PlayClipAtPoint(death, transform.position);
		state = 1;
		GetComponent<Collider2D>().enabled = false;
		rigidBody.isKinematic = true;
		rigidBody.velocity = new Vector2(0, 0);
		life.CannotRegenerate();
		for(int i = 0; i < transform.childCount; i++){
			transform.GetChild(i).gameObject.SetActive(false);
		}

		for(int i = 0; i < 20; i++){
			Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
			Vector2 newSpeed = new Vector2(Random.Range(-6f, 6f), Random.Range(5f, 8f));
			GameObject obj = (GameObject)Instantiate(particle, newPos, transform.rotation);
			obj.GetComponent<Rigidbody2D>().velocity = newSpeed * 2;
			obj.GetComponent<Rigidbody2D>().gravityScale = 3;
			obj.GetComponent<TeamManager>().UpdateTeam(2);
			obj.GetComponent<Bullet_Behaviour>().maxDistance = 1000;
            obj.GetComponent<Bullet_Behaviour>().size = 0.8f;
			obj.GetComponent<Bullet_Behaviour>().phasing = true;
		}

		Destroy(gameObject);
	}

	public Vector2 rotate(Vector2 v, float delta) {
		return new Vector2(
			v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
			v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
		);
	}


	private void OnCollisionStay2D(Collision2D other){
		if(other.gameObject.GetComponent<PlatformEffector2D>() != null){
			if(DownPhase){
				Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
				Phased.Add(other.gameObject);
			}
		}
	}
}
