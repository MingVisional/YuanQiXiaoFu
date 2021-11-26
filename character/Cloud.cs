using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float bulletspeed = 7f, EndTime = 3,AttackGap=0.7f,nextAtt=0;
    public Rigidbody2D rig;
    public GameObject AttObject;
    public AudioSource audioS;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        nextAtt += Time.deltaTime;
        if (nextAtt > AttackGap)
        {
            audioS.Play();
            AttObject.SetActive(true);
            Invoke("FixAtt", 0.2f);
            nextAtt = 0f;
        }
    }
    private void FixAtt()
    {
        AttObject.SetActive(false);
    }
    void fixbullet()
    {
        //PoolManager.GetInstance().PushObj("prefabs/Other_0", gameObject);
        Destroy(gameObject);
    }
    public void Init()
    {
        rig = GetComponent<Rigidbody2D>();
        AttObject = transform.GetChild(0).gameObject;
        rig.velocity = new Vector2(transform.localScale.x * bulletspeed, 0f);
        Invoke("fixbullet", 5f);
    }
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.transform.CompareTag("Wall"))
        {
            //fixbullet();
        }
        if (collider.gameObject.transform.CompareTag("Enemy"))
        {
            collider.GetComponent<EnemyFSM>().Hurt(PlayerInfo.GetInstance().Attack * PlayerInfo.GetInstance().SoundBulletPower*0.75f);
            
        }
        else if (collider.CompareTag("BOSS"))
        {
            collider.GetComponent<BOSSFSM>().Hurt(PlayerInfo.GetInstance().Attack * PlayerInfo.GetInstance().SoundBulletPower);
            //fixbullet();
        }
    }
}