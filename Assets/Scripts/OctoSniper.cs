using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoSniper : MonoBehaviour
{
	public int Cooldown = 500;
	public GameObject Bullet;
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

	private int progress = 0;
	private int dir = 0;

	private int Team;
	private int SpawnC = 0;
	private bool spawning = true;
	private SpriteRenderer MyRenderer;

	private Camera main;

	// Variable para el láser
	public LineRenderer laser;

	void Start(){
		Team = GetComponent<TeamManager>().GetTeam();
		finder = transform.Find("FinderRange").GetComponent<TargetFinder>();
		SpawnAnim = transform.Find("Spawner").gameObject;
		progress = Random.Range(0, Cooldown);

		MyRenderer = GetComponent<SpriteRenderer>();
		audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

		MyRenderer.enabled = false;
		SpawnAnim.SetActive(false);
		audio.PlayOneShot(SpawnSound);

		main = Camera.main;
		// Instantiate the laser object
   		GameObject laserObject = new GameObject("Laser");
    	laserObject.transform.parent = transform;
    	laser = laserObject.AddComponent<LineRenderer>();

		// Configure the line renderer properties
		laser.startWidth = 1f;
		laser.endWidth = 1f;
		laser.material = new Material(Shader.Find("Sprites/Default"));
		laser.startColor = Color.blue;
		laser.endColor = Color.blue;
		laser.enabled = false;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if(!spawning){

			Target = finder.FoundTarget();
			
			if(Target != null){
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
					laser.startWidth =  (1f/280)*(280-progress);
					laser.endWidth = (1f/280)*(280-progress);
					animator.SetInteger("Progress", progress);
					if(Cooldown-progress < 20){
						laser.startColor = Color.red;
						laser.endColor = Color.red;
						
					}
					if(progress < 100){
						laser.startWidth =  0f;
						laser.endWidth = 0f;
					}
					if(progress > Cooldown){
						Shoot(direction);
						audio.PlayOneShot(ShootSound);
						progress = 0;
						laser.startColor = Color.blue;
						laser.endColor = Color.blue;
						laser.startWidth = 1f;
						laser.endWidth = 1f;
					}
				}

				// Actualizar láser
            	laser.SetPosition(0, transform.position);
            	laser.SetPosition(1, Target.transform.position);
            	laser.enabled = true;
				

			}else{
				progress = 9;
				animator.SetInteger("Progress", 9);
				// Deshabilitar láser
            	laser.enabled = false;
			}
		}else{
			if(SpawnC == 0){
				SpawnAnim.SetActive(true);
			}
			SpawnAnimationUpd();
		}
	}

	// Methods

	private void Shoot(Vector2 direction) // Crea un proyectil nuevo
	{
		Vector2 newPos = new Vector2(transform.position.x + (dir*2f), transform.position.y-(1f));
						
    					
						Vector2 newSpeed = new Vector2(direction.x * BulletSpeed, direction.y * BulletSpeed);
						GameObject obj = (GameObject)Instantiate(Bullet, newPos, transform.rotation);
						obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
						obj.GetComponent<Bullet_Behaviour>().dmg = 50;
						obj.GetComponent<TeamManager>().UpdateTeam(Team);
						// Deshabilitar láser mientras dispara
    					laser.enabled = false;
	}

	private void SpawnAnimationUpd() // Actualiza la animación de spawn del enemigo
	{
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

	public void Kill() // Destruye al enemigo, es la "animacion" de muerte del enemigo.
	{
		if(GetComponent<TrackedObject>() != null)
			if(GetComponent<TrackedObject>().IsActive())
				GetComponent<TrackedObject>().SendSignal();
		AudioSource.PlayClipAtPoint(DieSound, main.transform.position);
		for(int i = 0; i < 10; i++){
			Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
			Vector2 newSpeed = new Vector2(Random.Range(-6f, 6f), Random.Range(5f, 8f));
			GameObject obj = (GameObject)Instantiate(Bullet, newPos, transform.rotation);
			obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
			obj.GetComponent<Rigidbody2D>().gravityScale = 3;
			obj.GetComponent<Bullet_Behaviour>().phasing = true;
			obj.GetComponent<TeamManager>().UpdateTeam(Team);
			obj.gameObject.transform.localScale = new Vector2(0.6f, 0.6f);
		}
	}

	
	public TargetFinder TargetSettings(){
		return finder;
	}
}
