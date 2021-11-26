using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magic2 : MonoBehaviour
{
    public float bulletspeed = 20f;
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
        PoolManager.GetInstance().PushObj("prefabs/magic2", gameObject);
    }
    public void Init()
    {
        Invoke("fixbullet", 0.35f);
    }
}
