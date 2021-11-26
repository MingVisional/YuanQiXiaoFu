using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSFu_cloud_bullet : MonoBehaviour
{
    public float attack;
    private float speed;
    new protected string name = "BOSSFu_cloud_bullet";

    public void Init(float attack, float speed)
    {
        this.attack = attack;
        this.speed = speed;
    }
    protected void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(0, -1, 0), speed * Time.deltaTime);
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
            collider.GetComponent<HorizontalControl>().Hurt(attack, 0);
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
