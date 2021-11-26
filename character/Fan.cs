using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    // Start is called before the first frame update
    public float AttackGap = 2f;
    private float nextAtt=0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nextAtt += Time.deltaTime;
        if (nextAtt > AttackGap)
        {
            getBullet();
            nextAtt = 0f;
        }
    }
    public void getBullet()
    {
        PoolManager.GetInstance().GetObj("prefabs/FanBullet", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0, 0.3f, 0);
            o.transform.localScale = new Vector2(transform.localScale.x * 0.5f, 0.5f);
            o.transform.SetParent(this.transform.parent);
            o.GetComponent<FanB>().Init();
        });
    }
    public void fixFan()
    {
        //PoolManager.GetInstance().PushObj("prefabs/Fan", gameObject);
        Destroy(gameObject);
    }
    public void Init()
    {
        Invoke("fixFan", 10f);
    }
}
