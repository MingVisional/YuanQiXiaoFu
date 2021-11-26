using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSFu_fan : MonoBehaviour
{
    public float attack;
    private float bulletSpeed;
    new protected string name = "BOSSFu_fan";

    private float timer;
    private float attackTime = 1.5f;
    private float allTimer;

    public AudioSource attackAudio;
    public void Init(float attack, Vector3 scale, float bulletSpeed)
    {
        timer = 0;
        allTimer = 0;
        this.attack = attack;
        transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        this.bulletSpeed = bulletSpeed;
    }
    protected void Update()
    {
        allTimer += Time.deltaTime;
        timer += Time.deltaTime;
        if (allTimer > 5.1 * attackTime) DestroyIt();
        if(timer > attackTime)
        {
            attackAudio.Play();
            PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSFu_fan_bullet", (o) =>
            {
                o.transform.position = transform.position + new Vector3(1.24f * transform.localScale.x, 0.16f, 0);
                o.GetComponent<BOSSFu_fan_bullet>().Init(attack, new Vector2(1 * transform.localScale.x, 1), bulletSpeed, transform.localScale.x);
                o.transform.SetParent(GameObject.Find("BulletManager").transform);
            });
            timer = 0;
        }
    }
    protected void DestroyIt()
    {
        Destroy(gameObject);
        //PoolManager.GetInstance().PushObj("Prefab/Bullet/" + name, this.gameObject);
    }
}
