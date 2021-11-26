using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSaunt_bullet1 : MonoBehaviour
{
    public float attack;
    private float dir;
    private float speed;
    private Vector2 targetDir;
    new protected string name = "BOSSaunt_bullet1";
    private float targetPositionY;
    private float timer;
    private float attTime = 5f;

    private int state = 1;
    public void Init(float attack, Vector3 scale, float speed, float dir,Vector2 targetPosition,float targetPositionY)
    {
        state = 1;
        this.attack = attack;
        this.timer = 0;
        transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        this.dir = dir;
        this.targetDir = targetPosition - (Vector2)transform.position;
        this.speed = speed;
        this.targetPositionY = targetPositionY;
    }
    protected void Update()
    {
        if(state == 1)
        transform.position = Vector2.MoveTowards(transform.position,(Vector2)transform.position + targetDir,speed * Time.deltaTime);
        timer += Time.deltaTime;
        if (transform.position.y <= targetPositionY) state = 2;
        if (timer > attTime)
        {
            Destroy(gameObject);
        }
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
            if (PlayerPrefs.GetInt("InBoss") == 1)
            {
                collider.GetComponent<HorizontalControl>().Hurt(attack, dir);
            }
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
