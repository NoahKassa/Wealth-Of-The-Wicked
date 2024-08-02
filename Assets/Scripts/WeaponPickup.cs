using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite droppedbook;
    public Sprite closedbook;
    public Sprite openbook;
    public int weaponnumber;
    private Moving moving;
    private gamemanager gamemanager;
    private attack _attack;
    // Start is called before the first frame update
    void Start()
    {
        _attack =  GameObject.Find("saitama").GetComponent<attack>();
        gamemanager = GameObject.Find("gamemanager").GetComponent<gamemanager>();
        moving = GameObject.Find("saitama").GetComponent<Moving>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //if its the sword
            if (!moving.holding)
            {
                if (gameObject.CompareTag("Sword"))
                {
                    Destroy(gamemanager.weaponlist[weaponnumber].GetComponent<Rigidbody2D>());
                    Destroy(gamemanager.weaponlist[weaponnumber].GetComponent<BoxCollider2D>());
                    gamemanager.weaponlist[weaponnumber].transform.parent = moving.player.transform;
                    gamemanager.weaponlist[weaponnumber].transform.localPosition = new Vector3(-8, 1, moving.stickchangez);
                    gamemanager.weaponlist[weaponnumber].transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    moving.currentweapon = weaponnumber;
                    moving.isstickup = true;
                    moving.holding = true;
                }
                if (gameObject.CompareTag("Book"))
                {
                    Destroy(gamemanager.weaponlist[weaponnumber].GetComponent<Rigidbody2D>());
                    Destroy(gamemanager.weaponlist[weaponnumber].GetComponent<BoxCollider2D>());
                    gamemanager.weaponlist[weaponnumber].transform.parent = moving.player.transform;
                    gamemanager.weaponlist[weaponnumber].transform.localPosition = new Vector3(-moving.bookchangex,1,moving.stickchangez);
                    gamemanager.weaponlist[weaponnumber].transform.localRotation = new Quaternion(0f,0f,0f,0f);
                    spriteRenderer.sprite = closedbook;
                    moving.currentweapon = weaponnumber;
                    moving.isbookup = true;
                    moving.holding = true;
                    _attack.cooldown = true;
                    _attack.StartCoroutine(_attack.Activehit());

                }
            }

            if (gameObject.CompareTag("buff")&&!moving.jumpbuffed)
            {
                Destroy(gamemanager.weaponlist[weaponnumber].GetComponent<Rigidbody2D>());
                Destroy(gamemanager.weaponlist[weaponnumber].GetComponent<BoxCollider2D>());
                gamemanager.weaponlist[weaponnumber].transform.parent = moving.player.transform;
                gamemanager.weaponlist[weaponnumber].transform.localPosition = new Vector3(0,-3.5f,0);
                gamemanager.weaponlist[weaponnumber].transform.localRotation = new Quaternion(0f,0f,0f,0f);
                moving.jumpbuffed = true;
                moving.currentbuff = weaponnumber;
            }


        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
