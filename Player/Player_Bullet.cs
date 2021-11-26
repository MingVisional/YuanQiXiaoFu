using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet : MonoBehaviour
{
    private float speed = 20f;
    private float dir;
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
           new Vector2(transform.position.x + dir, transform.position.y),
           speed * Time.deltaTime);

        //跑太远了销毁子弹
        if (transform.position.x > 100 || transform.position.x < -100 || transform.position.y > 100 || transform.position.y < -100)
        {
            DestroyIt();
        }
    }
    public void Init(float dir)
    {
        this.dir = dir;
        transform.localScale = new Vector3(dir * 2, transform.localScale.y, transform.localScale.z);
    }
    protected void DestroyIt()
    {
        //Destroy(gameObject);
        PoolManager.GetInstance().PushObj("Prefab/Bullet/player_bullet", this.gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.transform.CompareTag("Wall"))
        {
            DestroyIt();
        }
        if (collider.gameObject.transform.CompareTag("Enemy"))
        {
            collider.GetComponent<EnemyFSM>().Hurt(PlayerInfo.GetInstance().Attack*PlayerInfo.GetInstance().SoundBulletPower);
            DestroyIt();
        }
        else if (collider.CompareTag("BOSS"))
        {
            collider.GetComponent<BOSSFSM>().Hurt(PlayerInfo.GetInstance().Attack * PlayerInfo.GetInstance().SprintPower);
        }
    }
}
