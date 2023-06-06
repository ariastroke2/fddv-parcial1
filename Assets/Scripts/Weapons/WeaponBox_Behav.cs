using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox_Behav : MonoBehaviour
{

    private int onGround;

    public Animator animator;

    private AudioSource audio;

    [SerializeField]
    private AudioClip boxSpawn;

    [SerializeField]
    private AudioClip boxLand;

    [SerializeField]
    private AudioClip boxOpen;

    private Rigidbody2D rb;
    private Weapons_Menu wmenu;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        onGround = 0;
        audio.PlayOneShot(boxSpawn);
        animator = GetComponent<Animator>();
        wmenu = transform.Find("ChangeWeapon").GetComponent<Weapons_Menu>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("WithinRange", wmenu.canChangeWeapon);
    }
}
