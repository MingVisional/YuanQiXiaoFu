using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSBlackTree_bullet : MonoBehaviour
{
    public float attack;
    private float dir;
    private float speed;
    private Vector2 targetPosition;
    new protected string name = "BOSSBlackTree_bullet";
    private float flyTime = 1f;
    private float xSpeed;
    private float ySpeed;
    private float startPositionY;
    private float midPositionY;
    private int state = 1;
    public void Init(float attack, Vector3 scale, float dir, Vector2 targetPosition)
    {
        state = 1;
        this.attack = attack;
        transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        this.dir = dir;
        this.targetPosition = targetPosition;

        startPositionY = transform.position.y;
        midPositionY = (targetPosition.y + transform.position.y) / 2f + 3f;
        xSpeed = (targetPosition.x - transform.position.x) / flyTime;
    }
    protected void Update()
    {
        if (state == 1) ySpeed = (midPositionY - startPositionY) / (flyTime / 2);
        else ySpeed = (targetPosition.y - midPositionY) / (flyTime / 2);
        transform.position = new Vector3(transform.position.x + xSpeed * Time.deltaTime, transform.position.y + ySpeed * Time.deltaTime, transform.position.z);

        if (midPositionY - transform.position.y < 0.2f) state = 2;
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
