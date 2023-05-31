using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit_spark_behav : MonoBehaviour
{

    private SpriteRenderer img;
    private AudioSource audio;

    [SerializeField] private Sprite[] sparks;
    [SerializeField] private AudioClip hitsound;

    private int step;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        transform.rotation = Quaternion.Euler(0,0, Random.Range(1, 360));
        if(Random.Range(1, 2) == 1){
            img.sprite = sparks[0];
        }else{
            img.sprite = sparks[1];
        }
        audio.PlayOneShot(hitsound);
        step = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = (new Vector2(0.8f, 0.5f * Mathf.Cos(step * Mathf.PI / 8f)));
        step++;
        if(step > 4)
            Destroy(gameObject);
    }
}
