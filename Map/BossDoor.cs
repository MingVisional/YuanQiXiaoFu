using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    // Start is called before the first frame update

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
            PlayerPrefs.SetFloat("Time", Time.time);
            PlayerPrefs.SetInt("BossNumber", PlayerPrefs.GetInt("BossNumber")+1);
            Debug.Log("doorBossNumber:" + PlayerPrefs.GetInt("BossNumber"));
            collision.GetComponent<HorizontalControl>().fixData();
            collision.GetComponent<HorizontalControl>().loadDoorFix();
            PlayerInfo.GetInstance().fixData();
            for (int i = 0; i < GameObject.Find("HorizontalPlate").transform.FindChild("BossLevelManner").transform.childCount; i++)
            {
                Destroy(GameObject.Find("HorizontalPlate").transform.FindChild("BossLevelManner").transform.GetChild(i).gameObject);
            }
            GameObject.Find("TotalControl").GetComponent<TotalControl>().loadTopView();
            Destroy(gameObject);
        }
    }
}
