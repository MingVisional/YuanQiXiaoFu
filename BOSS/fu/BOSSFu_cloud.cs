using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSFu_cloud : MonoBehaviour
{
    public float attack;
    private float bulletSpeed = 6f;
    new protected string name = "BOSSFu_cloud";

    private float targetPositionX;
    private float targetPositionY;
    private float timer;
    private float attackTime = 2f;

    public AudioSource attackAudio;
    public void Init(float attack,float targetPositionX,float targetPositionY)
    {
        timer = 0;
        this.attack = attack;
        this.targetPositionX = targetPositionX;
        this.targetPositionY = targetPositionY;
    }
    protected void Update()
    {
        timer += Time.deltaTime;
        if (timer > attackTime)
        {
            attackAudio.Play();

            PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSFu_cloud_bullet", (o) =>
            {
                o.transform.position = transform.position + new Vector3(0, -0.46f, 0);
                o.GetComponent<BOSSFu_cloud_bullet>().Init(attack,bulletSpeed);
                o.transform.SetParent(GameObject.Find("BulletManager").transform);
            });
            timer = 0;
        }
        transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(targetPositionX,targetPositionY), 10f * Time.deltaTime);
    }
}
