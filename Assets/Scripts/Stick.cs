/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private int startDelay;
    public GameObject gameObject;
    private Moving moving;
    Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        moving = GameObject.Find("saitama").GetComponent<Moving>();
        startDelay = 10;
        Anim = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)&& moving.cooldown==false)
        {
            gameObject.GetComponent<Animator>().Play("FullAttack");
            gameObject.GetComponent<Animator>().Play("Idle");
            StartCoroutine(DelayedAnimation());
            Anim.ResetTrigger("attack");
        }
    }
    IEnumerator DelayedAnimation()
    {
       yield return new WaitForSeconds(startDelay);
     //   gameObject.GetComponent<Animator>().Play("FullAttack");
     //   gameObject.GetComponent<Animator>().Play("Idle");
        Anim.ResetTrigger("attack");
    }
}
*/