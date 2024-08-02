using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballPath : MonoBehaviour
{
    public Vector2 direction;
    public GameObject saitama;
    public Transform player;
    public float zoom;
    public GameObject caster;
    public Vector3 pos;
    private Vector3 aim;
    public Enemyscript enemy;
    public SpriteRenderer spite;
    // Start is called before the first frame update
    void Start()
    {
        enemy = caster.gameObject.GetComponent<Enemyscript>();
        zoom = 1.2f;
    }

    public void Fire()
    {
        Debug.Log("udsavfdbsgvfdjs");
        pos= new Vector3(player.position.x, player.position.y + 2, player.position.z);
        Vector3 difference = pos  - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        if (!enemy.moveleft)
        {
            spite.flipX = true;
        }
        else if (enemy.moveleft)
        {
            spite.flipX = false;
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * zoom);
        
    }
}
