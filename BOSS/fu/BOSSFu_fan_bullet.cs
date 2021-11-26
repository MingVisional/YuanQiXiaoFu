using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSFu_fan_bullet : MonoBehaviour
{
    public float attack;
    private float dir;
    private float speed;
    new protected string name = "BOSSFu_fan_bullet";

    public void Init(float attack, Vector3 scale, float speed, float dir)
    {
        this.attack = attack;
        transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        this.dir = dir;
        this.speed = speed;
    }
    protected void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(dir, 0, 0), speed * Time.deltaTime);
    }
    protected void DestroyIt()
    {
        Destroy(gameObject);
        //PoolManager.GetInstance().PushObj("Prefab/Bullet/" + name, this.gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.transform.CompareTag("Wall"))
        {
            DestroyIt();
        }
        if (collider.gameObject.transform.CompareTag("Player"))
        {
            collider.GetComponent<HorizontalControl>().Hurt(attack, dir);
            DestroyIt();
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
