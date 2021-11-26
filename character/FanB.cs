using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanB : MonoBehaviour
{
    public float bulletspeed = 20f, EndTime = 3;
    public Rigidbody2D rig;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    void fixbullet()
    {
        //PoolManager.GetInstance().PushObj("prefabs/FanBullet", gameObject);
        Destroy(gameObject);
    }
    public void Init()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = new Vector2(transform.localScale.x * bulletspeed, 0f);
        Invoke("fixbullet", 3f);
    }
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.transform.CompareTag("Wall"))
        {
            fixbullet();
        }
        if (collider.gameObject.transform.CompareTag("Enemy"))
        {
            collider.GetComponent<EnemyFSM>().Hurt(PlayerInfo.GetInstance().Attack * PlayerInfo.GetInstance().SoundBulletPower*0.35f);
            fixbullet();
        }
        else if (collider.CompareTag("BOSS"))
        {
            collider.GetComponent<BOSSFSM>().Hurt(PlayerInfo.GetInstance().Attack * PlayerInfo.GetInstance().SoundBulletPower*0.15f);
            fixbullet();
        }
    }
}