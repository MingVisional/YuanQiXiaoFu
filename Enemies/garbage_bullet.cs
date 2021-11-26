using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garbage_bullet : MonoBehaviour
{
    public Sprite state1;
    public Sprite state2;

    public float attack;
    private Vector3 bornPoint;
    private Vector3 targetPoint;
    private float highPointY;//抛物线的最高点的y坐标
    new private string name;

    private float high = 2f;
    private float xSpeed;
    public float ySpeed;
    private float flyTime = 1f;
    private float currentFlyTime;
    public void Awake()
    {
        name = "garbage_bullet";
    }
    public void Init(float attack,Vector3 bornPoint,Vector3 targetPoint)
    {
        GetComponent<SpriteRenderer>().sprite = state1;
        this.attack = attack;
        this.bornPoint = bornPoint;
        this.targetPoint = targetPoint;
        this.currentFlyTime = 0f;
        this.xSpeed = (targetPoint.x - bornPoint.x) / flyTime;
        this.highPointY = (bornPoint.y + targetPoint.y) / 2 + high;
    }
    protected void Update()
    {
        Fly();
        if (currentFlyTime > flyTime) GetComponent<SpriteRenderer>().sprite = state2;
        if (currentFlyTime > 5f) DestroyIt();
    }
    private void Fly()
    {
        if (currentFlyTime < flyTime)
        {
            if (currentFlyTime < 0.5 * flyTime) ySpeed = (float)((highPointY - bornPoint.y) / (0.5 * flyTime));
            if (currentFlyTime > 0.5 * flyTime) ySpeed = (float)((targetPoint.y - highPointY) / (0.5 * flyTime));
            transform.position = new Vector3(transform.position.x + xSpeed* Time.deltaTime, transform.position.y + ySpeed * Time.deltaTime, transform.position.z);
        }
        currentFlyTime += Time.deltaTime;
    }
    protected void DestroyIt()
    {
        Destroy(gameObject);
        //PoolManager.GetInstance().PushObj("Prefab/Bullet/" + name, this.gameObject);
    }

    protected void OnTriggerStay2D(Collider2D collider)
    {
        if (currentFlyTime > flyTime && collider.gameObject.transform.CompareTag("Player"))
        {
            //collider.GetComponent<PlayerControl>().Hurt(attack, dir);
           PlayerInfo.GetInstance().Hp -= attack*Time.deltaTime;
        }
    }
}
