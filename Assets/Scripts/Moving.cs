using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public Animator anim;
    public bool holding;
    public bool isstickup;
    public List<Rigidbody2D> enemyRB;
    private gamemanager gamemanager;
    private WeaponPickup WeaponPickup;
    public float horizontalinput;
    public float speed = 1;
    public bool isonground;
    public float jumpforce = 1000;
    public Rigidbody2D playerRB;
    public float gravmod = 0;
    public bool hit = false;
    public float hitstun = 0.2f;
    public bool faceright;
    public float jumpvelocity = 70;
    public bool jumpbuffed = false;
    public float stickchangex = 3.7f;
    public float stickchangez = 1f;
    public float bookchangex = -1.5f;
    public GameObject player;
    public float tossaside = 8;
    public int currentweapon;
    public int currentbuff;
    public float fast, forceJump;
    float hopdifference = 35.0f;
    public Vector2 velocity;
    public GameObject child;
    public bool walking;
    public bool cooldown = false;
    private AudioSource source;
    public bool GameStart;
    public PlayAudio playAudio;
    public bool fall;
    public List<GameObject> cracked;
    public int nowque;
    public int nextque;
    public bool running;
    public bool isbookup;
    public float fireballpos = -2.15f;
    public int checking;

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("isMoving", false);
        source = GetComponent<AudioSource>();
        hit = false;
        isstickup = false;
        holding = false;
        isonground = true;
        Physics.gravity *= gravmod;
        gamemanager = GameObject.Find("gamemanager").GetComponent<gamemanager>();
        WeaponPickup = GameObject.Find("gamemanager").GetComponent<WeaponPickup>();
        faceright = false;
        currentweapon = -1;
        currentbuff = -1;
        nowque = 0;
        nextque = 0;
        GameStart = false;
        //faceright = false;
        
    }
    public void StartButton()
    {
        
    }

    void Fixedupdate()
    {
        if (faceright)
        {
            gameObject.GetComponent<attack>().bulletspeed = 30;
        }
        if (!faceright)
        {
            gameObject.GetComponent<attack>().bulletspeed = -30;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;  
        GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Extrapolate;
        if (jumpbuffed)
        {
            if (playerRB.velocity.y<47)
            {
                hopdifference = 30.0f;
            }
            else
            {
                hopdifference = 55.0f;
            }
           
            forceJump = 125;
        }
        if (!jumpbuffed)
        {
            hopdifference = 35f;
            forceJump = 90;
        }
        velocity = playerRB.velocity;
        horizontalinput = Input.GetAxis("Horizontal");
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 25;
            walking = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            walking = false;
            speed = 50;
        }
        velocity = playerRB.velocity;
        horizontalinput = Input.GetAxis("Horizontal");
        if (horizontalinput < 0 && faceright)
        {
            player.transform.Rotate(0, -180, 0);
            faceright = false;
            if (tossaside > 0)
            {
                tossaside = -8;
            }
            Fixedupdate();
        }

        if (horizontalinput > 0 && faceright == false)
        {
            player.transform.Rotate(0, 180, 0);
            faceright = true;
            stickchangez = -stickchangez;
            tossaside = 8;
            Fixedupdate();
        }

        if (hit == false)
        {
            if (faceright)
            {
                playerRB.transform.Translate(Vector3.right * -horizontalinput * Time.deltaTime * speed);
                if (Input.GetKey(KeyCode.RightArrow) == true || Input.GetKey(KeyCode.LeftArrow) == true)
                {
                    if (walking)
                    {
                        anim.SetBool("isMoving", false);
                        anim.SetBool("isWalking", true);
                    }
                    else if (!walking)
                    {
                        anim.SetBool("isMoving", true);
                        anim.SetBool("isWalking", false);
                    }

                }
            }
            else if (!faceright)
            {
                playerRB.transform.Translate(Vector3.right * horizontalinput * Time.deltaTime * speed);
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
                {
                    if (walking)
                    {
                        anim.SetBool("isMoving", false);
                        anim.SetBool("isWalking", true);
                    }
                    else if (!walking)
                    {
                        anim.SetBool("isMoving", true);
                        anim.SetBool("isWalking", false);
                    }
                }
            }
            if (horizontalinput == 0)
            {
                anim.SetBool("isMoving", false);
                anim.SetBool("isWalking", false);
            }
            float horizontal;
            horizontal = Input.GetAxis("Horizontal");
            Vector2 position = playerRB.position;
            
            if (isonground && Input.GetKeyDown(KeyCode.UpArrow))
            {
                anim.SetBool("isJumping", true);
                playerRB.velocity = (new Vector2(playerRB.velocity.x, forceJump));
                FindObjectOfType<PlayAudio>().Play("Jump");
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (playerRB.velocity.y > 0.0f)
                {
                    playerRB.velocity = (new Vector2(playerRB.velocity.x, playerRB.velocity.y - hopdifference));
                }
            }

            position.x = position.x + horizontal * fast * Time.timeScale;
            playerRB.position = position;
            if (Input.GetKey(KeyCode.DownArrow) && isonground)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift)&& jumpbuffed)
                {
                    Debug.Log("f you jays");
                    gamemanager.weaponlist[currentbuff].transform.parent = null;
                    gamemanager.weaponlist[currentbuff].AddComponent<BoxCollider2D>();
                    gamemanager.weaponlist[currentbuff].AddComponent<Rigidbody2D>();
                    gamemanager.weaponlist[currentbuff].GetComponent<Rigidbody2D>().constraints =
                        RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    gamemanager.weaponlist[currentbuff].transform.position =
                        player.transform.position - new Vector3(tossaside+2, 2, 0);
                    currentbuff = -1;
                    jumpbuffed = false;
                }
                if (holding&&Input.GetKey(KeyCode.LeftShift)==false)
                {
                    if (isstickup)
                    {
                        isstickup = false;
                        gamemanager.weaponlist[currentweapon].transform.parent = null;
                        gamemanager.weaponlist[currentweapon].AddComponent<BoxCollider2D>();
                        gamemanager.weaponlist[currentweapon].AddComponent<Rigidbody2D>();
                        gamemanager.weaponlist[currentweapon].GetComponent<Rigidbody2D>().constraints =
                            RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                        gamemanager.weaponlist[currentweapon].transform.position =
                            player.transform.position - new Vector3(tossaside, 2, 0);
                    }

                    if (isbookup)
                    {
                        Debug.Log("fuck reading");
                        isbookup = false;
                        WeaponPickup.spriteRenderer.sprite = WeaponPickup.droppedbook;
                        gamemanager.weaponlist[currentweapon].transform.parent = null;
                        gamemanager.weaponlist[currentweapon].AddComponent<BoxCollider2D>();
                        gamemanager.weaponlist[currentweapon].AddComponent<Rigidbody2D>();
                        gamemanager.weaponlist[currentweapon].GetComponent<Rigidbody2D>().constraints =
                            RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                        gamemanager.weaponlist[currentweapon].transform.position =
                            player.transform.position - new Vector3(tossaside, 2, 0);
                    }

                    currentweapon = -1;
                    holding = false;

                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")||collision.gameObject.CompareTag("cracked ground"))
        {
           
                isonground = true;
                anim.SetBool("isJumping", false);
            
        }
       
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")||other.gameObject.CompareTag("cracked ground"))
        {
            anim.SetBool("isJumping", true);
            isonground = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject d = GameObject.Find("AudioManager");
        if (collision.CompareTag("SoundActivation"))
        {
            Debug.Log("help");
            AudioSource[] sources = d.GetComponents<AudioSource>();
            sources[0].Stop();
            FindObjectOfType<PlayAudio>().Play("Rick");
        }
        if (collision.gameObject.CompareTag("horrorball"))
        { 
            collision.gameObject.SetActive(false);
            //float Ekickbackup = 10; float Ekickback = 90;
            gamemanager.life -= 1;
            if (playerRB.position.x < collision.gameObject.transform.position.x)
            {
                if (!isonground)
                {
                    hit = true;
                    StartCoroutine(HitStun());
                    //StartCoroutine(HitStunLand());
                    playerRB.velocity = new Vector2(0, 0);
                    playerRB.AddForce(new Vector2(-1500, 800), ForceMode2D.Impulse);
                }
                else
                {
                    hit = true;
                    StartCoroutine(HitStun());
                    //StartCoroutine(HitStunLand());
                    playerRB.velocity = new Vector2(0, 0);
                    playerRB.AddForce(new Vector2(-1500, 2100), ForceMode2D.Impulse);
                    isonground = false;
                }

            }
            else if (playerRB.position.x > collision.gameObject.transform.position.x)
            {
                if (!isonground)
                {
                    hit = true;
                    StartCoroutine(HitStun());
                    //StartCoroutine(HitStunLand());
                    playerRB.velocity = new Vector2(0, 0);
                    playerRB.AddForce(new Vector2(1500, 800), ForceMode2D.Impulse);
                }
                else
                {
                    hit = true;
                    StartCoroutine(HitStun());
                    //StartCoroutine(HitStunLand());
                    playerRB.velocity = new Vector2(0, 0);
                    playerRB.AddForce(new Vector2(1500, 2100), ForceMode2D.Impulse);
                    isonground = false;
                }

            }

            /*if (gamemanager.life <= 0)
            {
                Debug.Log("dead");
                PlayerRB.transform.rotation.z.Equals(90);
                transform.rotation.z.Equals(90);
            }*/
            Debug.Log(+gamemanager.life);
        }
        
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.CompareTag("pointy"))
        {
            gamemanager.life -= 100;
        }*/
        if (collision.gameObject.CompareTag("Ground"))
        {
            hit = false;
        }
        if (collision.gameObject.CompareTag("cracked ground")&& !cracked.Contains(collision.gameObject))
        {
            StartCoroutine(CrackedTimer(collision));
            
        }

        if (collision.gameObject.CompareTag("enemy")&& !gameObject.CompareTag("hitbox"))
        {
            //float Ekickbackup = 10; float Ekickback = 90;
            gamemanager.life -= 1;
            if (playerRB.position.x < collision.gameObject.transform.position.x)
            {
                if (!isonground)
                {
                    hit = true;
                    StartCoroutine(HitStun());
                    //StartCoroutine(HitStunLand());
                    playerRB.velocity = new Vector2(0, 0);
                    playerRB.AddForce(new Vector2(-1500, 800), ForceMode2D.Impulse);
                }
                else
                {
                    hit = true;
                    StartCoroutine(HitStun());
                    //StartCoroutine(HitStunLand());
                    playerRB.velocity = new Vector2(0, 0);
                    playerRB.AddForce(new Vector2(-1500, 2100), ForceMode2D.Impulse);
                    isonground = false;
                }

            }
            else if (playerRB.position.x > collision.gameObject.transform.position.x)
            {
                if (!isonground)
                {
                    hit = true;
                    StartCoroutine(HitStun());
                    //StartCoroutine(HitStunLand());
                    playerRB.velocity = new Vector2(0, 0);
                    playerRB.AddForce(new Vector2(1500, 800), ForceMode2D.Impulse);
                }
                else
                {
                    hit = true;
                    StartCoroutine(HitStun());
                    //StartCoroutine(HitStunLand());
                    playerRB.velocity = new Vector2(0, 0);
                    playerRB.AddForce(new Vector2(1500, 2100), ForceMode2D.Impulse);
                    isonground = false;
                }

            }

            /*if (gamemanager.life <= 0)
            {
                Debug.Log("dead");
                PlayerRB.transform.rotation.z.Equals(90);
                transform.rotation.z.Equals(90);
            }*/
            Debug.Log(+gamemanager.life);
        }

    }
    IEnumerator CrackedTimer(Collision2D collision)
    {
        cracked.Add(collision.gameObject);
        running = true;
        nextque += 1;
        yield return new WaitForSeconds(1);
        cracked[nowque].gameObject.SetActive(false);
        nowque += 1;
        running = false;
        yield return new WaitForSeconds(5);
        foreach(GameObject obj in cracked) 
        {
            if (obj.activeSelf==false)
            {
                checking += 1;
            }
            if (checking+1>=nowque)
            {
                nowque = 0;
                nextque = 0;
                cracked = new List<GameObject>(20);
                checking = 0;
            }
        }
    }

    IEnumerator HitStun()
    {

        horizontalinput = 0;

        yield return new WaitForSeconds(hitstun);
        hit = false;
    }
    /*IEnumerator HitStunLand()
    {
        while (hit)
        {
            horizontalinput = 0;
        }
        yield return new WaitForSeconds(0.15f);
        while (!isonground)
        {
            Debug.Log("AAAAAAAAAAAAA");
            hit = true;
        }
        Debug.Log("land");
        hit = false;
    }*/

}





