using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{

    public AudioClip d;
    public int Team = 0;
    public AudioClip hit;

    private Camera main;

    void Start(){
        main = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other)
            {
                    if(other.gameObject.tag=="Ground"){
                        Debug.Log("Sploosh");
                        AudioSource.PlayClipAtPoint(d, transform.position);
                        gameObject.SetActive(false);
                        Destroy(gameObject);
                        
                    }
                    if(Team == 1){
                        if (other.gameObject.tag == "Player")
                        {   
                            //Debug.Log("Player Hit");
                            other.gameObject.GetComponent<LifeManager>().Damage(45);

                            AudioSource.PlayClipAtPoint(d, main.transform.position);
                            AudioSource.PlayClipAtPoint(hit, main.transform.position);
                            gameObject.SetActive(false);
                            Destroy(gameObject);
                        }
                        else if(other.gameObject.tag == "Brella")
                        {   
                            if(other.gameObject.GetComponent<LifeManager>().Life() > 0){
                                //Debug.Log("Blocked");
                                other.gameObject.GetComponent<LifeManager>().Damage(45);
    
                                AudioSource.PlayClipAtPoint(d, main.transform.position);
                                gameObject.SetActive(false);
                                Destroy(gameObject);
                            }
                        }
                    }
                    if(Team == 2){
                        if (other.gameObject.tag == "Enemy")
                        {   
                            //Debug.Log("Enemy hit");
                            other.gameObject.GetComponent<LifeManager>().Damage(6);

                            AudioSource.PlayClipAtPoint(hit, main.transform.position);
                            gameObject.SetActive(false);
                            Destroy(gameObject);
                        }
                    }

            }
}
