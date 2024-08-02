using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public GameObject hitbox;
    public GameObject stickBox;
    public GameObject fireball;
    public WeaponPickup WeaponPickup;
    private Moving moving;
    public bool attacking;
    public bool cooldown = false;
    public SpriteRenderer spriteRenderer;
    public Sprite fireball1;
    public Sprite fireball2;
    public int bulletspeed = -20;
    Animator ation;
    // Start is called before the first frame update
    void Start()
    {
       
        moving = GameObject.Find("saitama").GetComponent<Moving>();
        attacking = false;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving.holding)
        {
            WeaponPickup = GameObject.Find("gamemanager").GetComponent<gamemanager>().weaponlist[moving.currentweapon].gameObject.GetComponent<WeaponPickup>();
        }
        if (Input.GetKeyDown(KeyCode.Z) && moving.isstickup == false && moving.cooldown == false)
        {
            hitbox.SetActive(true);
            attacking = true;
            moving.cooldown = true;
            StartCoroutine(Activehit());
        }
        if (Input.GetKeyDown(KeyCode.Z) && moving.isstickup && moving.cooldown == false)
        {
            stickBox.SetActive(true);
            attacking = true;
            StartCoroutine(Activehit());
            moving.cooldown = true;
           
        }
        if (Input.GetKeyDown(KeyCode.Z)&&moving.isbookup&& cooldown == false)
        {
            fireball.transform.parent = null;
            WeaponPickup.spriteRenderer.sprite = WeaponPickup.closedbook;
            fireball.SetActive(true);
            attacking = true;
            cooldown = true;
            fireball.GetComponent<Rigidbody2D>().AddForce(new Vector2(bulletspeed,0f),ForceMode2D.Impulse);
            StartCoroutine(Activehit());
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy") && attacking)
        {
            collision.gameObject.SetActive(false);
            FindObjectOfType<PlayAudio>().Play("Smoke");
            Debug.Log("DIE");
        }
        if (collision.gameObject.CompareTag("Sword"))
        {
            ation = transform.GetChild(3).gameObject.GetComponentInChildren<Animator>();
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            Debug.Log(collision.gameObject.name);
            fireball.SetActive(false);
        }
    }
    public IEnumerator Activehit()
    {
        if (moving.isbookup)
        {
            yield return new WaitForSeconds(2f);
            attacking = false;
            fireball.SetActive(false);
        }
        if (moving.isstickup)
        { 
            Debug.Log("fuck");
            yield return new WaitForSeconds(0.2f);
            hitbox.SetActive(false);
            stickBox.SetActive(false);
            attacking = false;
            Debug.Log("you");
            yield return new WaitForSeconds(0.3f);
            Debug.Log("code");
        }
        else if (moving.holding==false)
        {
            yield return new WaitForSeconds(0.3f);
            hitbox.SetActive(false);
            stickBox.SetActive(false);
            attacking = false;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSecondsRealtime(0.2f);
        if (moving.isbookup)
        {
            WeaponPickup.spriteRenderer.sprite = WeaponPickup.openbook;
            fireball.transform.parent = moving.player.transform;
            fireball.gameObject.transform.localPosition = new Vector3(moving.fireballpos,1.63f,0);
            fireball.transform.localRotation = new Quaternion(0f,0f,0f,0f);
        }
        cooldown = false;
    }

}
