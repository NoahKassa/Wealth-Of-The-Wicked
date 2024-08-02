using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyscript : MonoBehaviour
{
    public int enemynumber;
    public Vector2 startposition;
    private Moving moving;
    public int moveSpeed = 4;
    public GameObject myWaypoint1;
    public GameObject myWaypoint2;
    public float moveTo;
    public float moveTo2;
    public bool moveleft;
    public GameObject currentWaypoint;
    public float ydirection;
    public float xdirection;
    public Vector2 velocity;
    public Vector2 direction;
    private GameObject goPlayer;
    private Transform transPlayer;
    private Transform myTransform;
    public GameObject projectile;
    public bool ecool;
    public FireballPath path;
    public SpriteRenderer spriteRenderer;
    public Sprite goblinattack;
    public Sprite goblincool;
    public Sprite batswoop;
    // Start is called before the first frame update
    void Start()
    {
        path = projectile.gameObject.GetComponent<FireballPath>();
        moving = GameObject.Find("saitama").GetComponent<Moving>();
        startposition = gameObject.GetComponent<Rigidbody2D>().position;
        currentWaypoint = myWaypoint2;
        myTransform = this.transform;
        goPlayer = GameObject.Find("saitama");
        transPlayer = goPlayer.transform;
        ecool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("enemy"))
        {
            Debug.Log("uriwh");
            if (transform.GetChild(0).gameObject.CompareTag("Bones"))
            {
                Debug.Log("wath");
                velocity = GetComponent<Rigidbody2D>().velocity;
                moveTo = Mathf.Abs(currentWaypoint.transform.position.x) - Mathf.Abs(transform.position.x);
                if (Mathf.Abs(moveTo) <= 0.5f)
                {
                    // At waypoint so stop moving
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    if (currentWaypoint == myWaypoint2)
                    {
                        currentWaypoint = myWaypoint1;
                        moveSpeed *= -1;
                        moveleft = true;
                        transform.GetChild(4).gameObject.SetActive(false);
                        transform.GetChild(5).gameObject.SetActive(true);
                        StartCoroutine(Turntime());
                    }
                    else if (currentWaypoint == myWaypoint1)
                    {
                        currentWaypoint = myWaypoint2;
                        moveSpeed *= -1;
                        moveleft = false;
                        transform.GetChild(3).gameObject.SetActive(false);
                        transform.GetChild(5).gameObject.SetActive(true);
                        StartCoroutine(Turntime());
                    }
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(
                        GetComponent<Transform>().localScale.x * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
                }
            }

            if (transform.GetChild(0).gameObject.CompareTag("bat"))
            {
                Debug.Log("bat");
                direction = transPlayer.position - myTransform.position;
                Vector2 orgin = gameObject.transform.position;
                LayerMask mask = LayerMask.GetMask("Default");
                RaycastHit2D hit = Physics2D.Raycast(orgin, direction, 60f, mask);
                Debug.DrawLine(orgin, direction, Color.red);
                if (hit.collider != null&&hit.collider.CompareTag("Player"))
                {
                    spriteRenderer.sprite = batswoop;
                    Debug.Log("hiy");
                    gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    transform.rotation = new Quaternion(0f, 0f, -90, 0f);
                    gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    if (moving.playerRB.transform.position.x > gameObject.transform.position.x)
                    {
                        xdirection = 15;
                        gameObject.transform.rotation = new Quaternion(0, 180, 0f, 0f);
                    }
                    else
                    {
                        xdirection = -15;
                        gameObject.transform.rotation = new Quaternion(0, 0, 0f, 0f);
                    }

                    if (moving.playerRB.transform.position.y + 3 > gameObject.transform.position.y)
                    {
                        ydirection = 6.5f;
                    }
                    else
                    {
                        if (transform.position.y-moving.playerRB.transform.position.y>11)
                        {
                            ydirection = -7;
                        }
                        else
                        {
                             ydirection = -3;
                        }
                    }

                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xdirection, ydirection);
                }
            }

            if (transform.GetChild(0).gameObject.CompareTag("Goblin"))
            {
                if (gameObject.transform.position.x>goPlayer.transform.position.x&& moveleft == false)
                {
                    moveleft = true;
                    spriteRenderer.flipX = false;
                }
                else if (gameObject.transform.position.x<goPlayer.transform.position.x&& moveleft)
                {
                    moveleft = false;
                    spriteRenderer.flipX = true;
                }
                direction = transPlayer.position - myTransform.position;
                Vector2 orgin = gameObject.transform.position;
                if (ecool == false)
                {
                    LayerMask mask = LayerMask.GetMask("Default");
                    RaycastHit2D hit = Physics2D.Raycast(orgin, direction, 60f, mask);
                    Debug.DrawLine(orgin, direction, Color.red);
                    if (hit.collider != null&&hit.collider.CompareTag("Player"))
                    {
                        ecool = true;
                        Debug.Log("oh yea");
                        projectile.transform.parent = null;
                        projectile.SetActive(true);
                        path.Fire();
                        StartCoroutine(goblinshoot());
                        spriteRenderer.sprite = goblincool;
                    }
                }

            }
        }
    }

    IEnumerator Turntime()
    {
        yield return new WaitForSeconds(.2f);
        transform.GetChild(5).gameObject.SetActive(false);
        if (moveleft)
        {
            transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    IEnumerator goblinshoot()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("bye");
        projectile.SetActive(false);
        spriteRenderer.sprite = goblinattack;
        ecool = false;
        projectile.transform.parent = gameObject.transform;
        projectile.transform.localPosition=new Vector2(0, 0);
    }

    private void OnCollisionEnter2D(Collision2D ben)
    {
        if (ben.gameObject.CompareTag("Sword")||ben.gameObject.CompareTag("buff"))
        {
            Physics2D.IgnoreCollision(ben.gameObject.GetComponent<Collider2D>(),gameObject.GetComponent<Collider2D>());
        }

        if (ben.gameObject.CompareTag("enemy")||ben.gameObject.CompareTag("horrorball"))
        {
            Physics2D.IgnoreCollision(ben.gameObject.GetComponent<Collider2D>(),gameObject.GetComponent<Collider2D>());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("hitbox"))
        {
            if (gameObject.CompareTag("horrorball")==false)
            {
                if (moving.isbookup)
                {
                    other.gameObject.SetActive(false);
                }
                gameObject.SetActive(false);
            }
            if (moving.isbookup)
            {
                gameObject.SetActive(false);
                other.gameObject.SetActive(false);
            }
        }
        
        if (other.gameObject.CompareTag("wall")||other.gameObject.CompareTag("Ground")||other.gameObject.CompareTag("cracked ground")||other.gameObject.CompareTag("hitbox"))
        {
            Debug.Log(other.gameObject.name);
            projectile.SetActive(false);
        }
        
    }
}
