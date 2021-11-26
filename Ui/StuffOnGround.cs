using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffOnGround : MonoBehaviour
{
    // Start is called before the first frame update
    public Item item;
    private AudioSource AS;
    void Start()
    {
        AS = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //AS.Play();
            if (item.ItemType == 1)
            {
                Debug.Log("角色恢复" + item.TypeNum.ToString() + "血量");
                PlayerInfo.GetInstance().Hp += item.TypeNum;
                if (PlayerInfo.GetInstance().Hp > PlayerInfo.GetInstance().MaxHp)
                {
                    PlayerInfo.GetInstance().Hp = PlayerInfo.GetInstance().MaxHp;
                }
            }
            else if (item.ItemType == 2)
            {
                //PlayerPrefs.SetInt("KnowledgePoints", PlayerPrefs.GetInt("KnowledgePoints") + 1);
                if (PlayerPrefs.GetInt("InBoss") == 1)
                {
                    collision.GetComponent<HorizontalControl>().KnowedgeNum += 3;
                }
                else
                {
                    collision.GetComponent<PlayerControl>().KnowedgeNum += 1;
                }
                Debug.Log("KnowledgePoints增加+");
            }
            else if(item.ItemType == 0)
            {
                if (PlayerPrefs.GetInt("InBoss") == 1)
                {
                    collision.GetComponent<HorizontalControl>().changeAttack(2);
                }
                else
                {
                    collision.GetComponent<PlayerControl>().changeAttack(2);
                }
                Debug.Log("切换拳套或增加伤害");
            }
            else if (item.ItemType == 3)
            {
                Debug.Log("切换技能");
                if (PlayerPrefs.GetInt("InBoss") == 1)
                {
                    collision.GetComponent<HorizontalControl>().ChangeMagic(item.TypeNum);
                }
                else
                {
                    collision.GetComponent<PlayerControl>().ChangeMagic(item.TypeNum);
                }
                
               
            }
            else if (item.ItemType == 4)
            {
                Debug.Log("角色金币增加:"+item.TypeNum.ToString());
                PlayerInfo.GetInstance().Gold += item.TypeNum;

            }
            //PoolManager.GetInstance().PushObj(gameObject.name, gameObject);
            Destroy(gameObject);
        }
    }
}
