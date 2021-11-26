using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopStuff : MonoBehaviour
{
    // Start is called before the first frame update
    public Item TheItem;
    void Start()
    {
       /* stuff = Resources.Load<GameObject>("prefabs/StuffOnGround/" + gameObject.name.Split('/')[2]);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStuff()
    {
        float CoinNum = PlayerInfo.GetInstance().Gold;
        Debug.Log(CoinNum);
        if (CoinNum >= TheItem.ItemCost)
        {
            /*            Debug.Log(gameObject.name.Split('/')[2]);*/
            PlayerInfo.GetInstance().Gold-=TheItem.ItemCost;
            PoolManager.GetInstance().GetObj("prefabs/StuffOnGround/" + gameObject.name.Split('/')[2], (o) =>
            {
                //o.transform.SetParent(transform.parent.parent);
                //o.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(transform.position).x, Camera.main.ScreenToWorldPoint(transform.position).y - 4f, 0f);
                o.transform.position = GameObject.Find("TopView").transform.Find("character").transform.position - new Vector3(0, 3, 0);
            });
            //PoolManager.GetInstance().PushObj(gameObject.name, gameObject);
            Destroy(gameObject);
            Debug.Log("金币减少"+TheItem.ItemCost.ToString());
        }
        else
        {
            Debug.Log("金币不足");
        }

        /*GameObject stuffs = Instantiate(stuff) as GameObject;
        stuffs.transform.position = Camera.main.ScreenToWorldPoint(transform.position) + new Vector3(0, -4f, 0);*/
    }
}
