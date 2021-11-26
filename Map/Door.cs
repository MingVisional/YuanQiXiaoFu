using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    public int DoorNum;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerPrefs.SetFloat("Time",Time.time);
            collision.GetComponent<PlayerControl>().fixData();
            PlayerInfo.GetInstance().fixData();
            Debug.Log(PlayerPrefs.GetInt("MagicMode1"));
            collision.GetComponent<PlayerControl>().loadDoorFix();
            if (DoorNum == -1)
            {
                Debug.Log("loadshop");
                PlayerPrefs.SetInt("inShop", 1);
                transform.parent.GetComponent<LevelManner>().getNextAfter(1);
            }
            else if (DoorNum == -2)
            {
                PlayerPrefs.SetInt("inShop", 0);
                transform.parent.GetComponent<LevelManner>().checkRewardNum(-1);
                transform.parent.GetComponent<LevelManner>().getNextAfter(0.5f);
            }
            else 
            {
                PlayerPrefs.SetInt("LevelCount", PlayerPrefs.GetInt("LevelCount")+1);
                PlayerPrefs.SetFloat("inShop", 0);
                transform.parent.GetComponent<LevelManner>().checkRewardNum(DoorNum);
                transform.parent.GetComponent<LevelManner>().getNextAfter(1.5f);
            }

        }
    }
}
