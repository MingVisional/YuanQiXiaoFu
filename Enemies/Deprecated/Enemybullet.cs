using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemybullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rig;
    public float bulletspeed = 10f;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void fixbullet()
    {
        PoolManager.GetInstance().PushObj("prefabs/enemybullet", gameObject);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float toward;
            if (collision.gameObject.transform.position.x < transform.position.x)
            {
                toward = -1f;
            }
            else
            {
                toward = 1f;
            }
            collision.gameObject.GetComponent<PlayerControl>().Hurt(10f,toward);
            fixbullet();
        }
    }
    public void Init(Vector2 target)
    {
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = target * bulletspeed;
        Invoke("fixbullet", 3f);
    }
}
