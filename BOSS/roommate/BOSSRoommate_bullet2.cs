using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSRoommate_bullet2 : MonoBehaviour
{
    public float attack;
    private float dir;
    private float speed;
    private Vector2 targetDir;
    new protected string name = "BOSSRoommate_bullet2";
    public void Init(float attack, Vector3 scale, float speed, float dir, Vector2 targetPosition)
    {
        this.attack = attack;
        transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        this.dir = dir;
        this.targetDir = targetPosition - (Vector2)transform.position;
        this.speed = speed;
    }
    protected void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + targetDir, speed * Time.deltaTime);

        //if (Vector2.Distance(transform.position, targetPosition) < 0.1f) DestroyIt();
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