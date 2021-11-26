using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float attack;
    private float dir;
    private float speed;
    new protected string name;
    private bool isInit = false;
    private float timer;
    private float attackTime = 4f;
    public void Init(float attack,Vector3 scale,float speed,float dir)
    {
        this.attack = attack;
        transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        this.dir = dir;
        this.speed = speed;
        this.isInit = true;
        timer = 0f;
    }
    protected void Update()
    {
        timer += Time.deltaTime;
        if (timer > attackTime) DestroyIt();
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.position.y * 100);
        transform.position = Vector2.MoveTowards(transform.position,
           new Vector2(transform.position.x + dir, transform.position.y),
           speed * Time.deltaTime);
    }
    protected void DestroyIt()
    {
        Destroy(gameObject);
        //PoolManager.GetInstance().PushObj("Prefab/Bullet/"+name, this.gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (isInit)
        {
            if (collider.gameObject.transform.CompareTag("Wall"))
            {
                DestroyIt();
            }
            if (collider.gameObject.transform.CompareTag("Player"))
            {
                //collider.GetComponent<PlayerControl>().Hurt(attack,dir);
                collider.GetComponent<PlayerControl>().Hurt(attack, dir);
                DestroyIt();
            }
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.CompareTag("Wall"))
        {
            DestroyIt();
        }
    }
}
