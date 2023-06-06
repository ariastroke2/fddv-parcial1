using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapons_Menu : MonoBehaviour
{
    public SpriteRenderer BoxImage; // Reference to the Image component where the sprites will be displayed
    public Sprite[] sprites; // Array of sprites to cycle through
    private int currentIndex = 0; // Current index of the active sprite

    public GameObject playerObject; // Reference to the player object
    public GameObject[] weaponPrefabs; // Array of weapon prefabs
    private GameObject currentWeapon; // Reference to the current weapon instance
    public GameObject DestroyExplosion;

    private int[] ChosenWeapons;

    private Transform cont;

    public bool canChangeWeapon = false; // Flag to determine if weapon change is allowed

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger Enter");
            canChangeWeapon = true; // Set the flag to true when in contact with the weapon changer
            /*for(int i = 0; i < 3; i++){
                transform.GetChild(i).gameObject.SetActive(true);
            }*/
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger Exit");
            canChangeWeapon = false; // Set the flag to false when no longer in contact with the weapon changer
            /*for(int i = 0; i < 3; i++){
                transform.GetChild(i).gameObject.SetActive(false);
            }*/
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        cont = transform.Find("DisplayContainer");
        if (currentWeapon == null)
        {
            // Instantiate the initial weapon prefab as a child of the player object
            //int weaponIndex = currentIndex % weaponPrefabs.Length;
            //currentWeapon = Instantiate(weaponPrefabs[weaponIndex], playerObject.transform);

            // Adjust the position and rotation of the weapon based on your needs
            //currentWeapon.transform.localPosition = Vector3.zero;
            //currentWeapon.transform.localRotation = Quaternion.identity;
        }
        else
        {
            // Set the position and rotation of the existing weapon based on your needs
            //currentWeapon.transform.localPosition = Vector3.zero;
            //currentWeapon.transform.localRotation = Quaternion.identity;
        }

        ChosenWeapons = new int[3];
        for(int i = 0; i < 3; i++){
            int x = Random.Range(0, weaponPrefabs.Length);
            ChosenWeapons[i] = x;
            cont.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprites[x];
            //cont.GetChild(i).gameObject.SetActive(false);
        }

        // Set the initial sprite
        //imageComponent.sprite = sprites[currentIndex];
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the "Q" key is pressed and weapon change is allowed
        if (Input.GetKeyDown(KeyCode.Q) && canChangeWeapon)
        {
            ChangeSprite();
        }
    }

    public void ChangeSprite()
    {
        // Destroy all previous weapon instances
        foreach (Transform child in playerObject.transform)
        {
            if (child.CompareTag("Weapon"))
            {
                Destroy(child.gameObject);
            }
        }

        // Increment the index and wrap around to 0 if it exceeds the array length

        currentWeapon = Instantiate(weaponPrefabs[ChosenWeapons[currentIndex]], playerObject.transform);

        for(int i = 0; i < 3; i++){
            Debug.Log((currentIndex)+" comparing "+i);
            if(currentIndex == i)
                cont.GetChild(i).localScale = new Vector2(0.07f, 0.07f);
            else
                cont.GetChild(i).localScale = new Vector2(0.05f, 0.05f);
        }

        currentIndex = (currentIndex + 1) % ChosenWeapons.Length;
        // Update the Image component with the new sprite
        //imageComponent.sprite = sprites[currentIndex];
        

        // Adjust the position and rotation of the weapon based on your needs
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        
        //DestroyBox();
    }

    public void DestroyBox()
    {

        currentWeapon = Instantiate(DestroyExplosion, transform.position, transform.rotation);
        currentWeapon.GetComponent<TeamManager>().UpdateTeam(2);
        currentWeapon.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -50f);


        Destroy(transform.parent.gameObject);

    }
}