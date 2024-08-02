using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class gamemanager : MonoBehaviour
{
    public List<GameObject> weaponlist;
    private bool gameover;
    public int life;
    public Canvas menu;
    public Canvas manual;
    public Canvas GameEnd;
    private AudioSource source;
    public Moving Moving;
    public GameObject player;
    private Moving moving;
    // Start is called before the first frame update
    void Start()
    {
        moving = GameObject.Find("saitama").GetComponent<Moving>();
        menu.gameObject.SetActive(true);
        life = 4;
        source = GetComponent<AudioSource>();
        Moving = GameObject.Find("saitama").GetComponent<Moving>();
    }
    public void ButtonStart()
    {
        menu.gameObject.SetActive(false);
        manual.gameObject.SetActive(false);
        player.SetActive(true);
        Moving.GameStart = true;
    }

    public void HowToPlay()
    {
        menu.gameObject.SetActive(false);
        manual.gameObject.SetActive(true);
    }
    public void StopMovement()
    {
        if (menu.gameObject.activeSelf == true)
        {
            Moving.player.SetActive(false);
           
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        if (life == 0)
        {
            gameover = true;
        }

        if (gameover)
        {
            Moving.speed = 0;
            Moving.jumpvelocity = 0;
            Moving.GameStart = false;
            GameEnd.gameObject.SetActive(true);
            FindObjectOfType<PlayAudio>().Play("Game Over");
        }

    }
    private void OnCollisionEnter2D(Collision2D ben)
    {
        if (ben.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(ben.gameObject.GetComponent<Collider2D>(),gameObject.GetComponent<Collider2D>());
            if (moving.playerRB.velocity.y < -1)
            {
                life = 0;
                Debug.Log(+life);
            }
        }
        if (ben.gameObject.CompareTag("enemy")||ben.gameObject.CompareTag("horrorball"))
        {
            Physics2D.IgnoreCollision(ben.gameObject.GetComponent<Collider2D>(),gameObject.GetComponent<Collider2D>());
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
