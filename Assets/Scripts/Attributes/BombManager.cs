using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] BombPrefab;

    [SerializeField]
    private int[] BombDelay;

    private int cooldown;
    private int Team;

    public int bombtype;

    // Start is called before the first frame update
    void Start()
    {
        Team = GetComponent<TeamManager>().GetTeam();
        cooldown = 0;
    }

    public int GetCurrentCooldownType()
    {
        return BombDelay[bombtype];
    }

    public int CurrentCooldownProgress()
    {
        return cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)
            - ((Vector2)transform.position)
        );

        if (bombtype == 2)
            if (direction.magnitude > 15)
            {
                float x = direction.magnitude / 15f;
                direction = direction / x;
            }

        if (cooldown > 0)
        {
            cooldown--;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (cooldown == 0)
            {
                cooldown = BombDelay[bombtype];
                Vector2 newSpeed = new Vector2(direction.x * 2, direction.y * 2);
                GameObject obj = (GameObject)Instantiate(
                    BombPrefab[bombtype],
                    (Vector2)transform.position + direction.normalized * 5,
                    transform.rotation
                );
                obj.GetComponent<Rigidbody2D>().velocity = newSpeed;
                obj.GetComponent<TeamManager>().UpdateTeam(Team);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && cooldown == 0)
        {
            ChangeBombType();
        }
    }

    private void ChangeBombType()
    {
        bombtype = (bombtype + 1) % BombPrefab.Length;
    }
}
