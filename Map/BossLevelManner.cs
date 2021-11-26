using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
public class BossLevelManner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Character, Camera,UiButton,TalkPanel;
    public Text Talk;
    private Vector3 DoorPos;
    private string[] RoomNames = { "Bedroom", "SportRoom", "ClassRoom", "SchoolPath", "EDoor"};
    public int Count,BossNumber=0;
    void Start()
    {
        
        Debug.Log("startBossNumber:" + PlayerPrefs.GetInt("BossNumber"));
        //loadBossRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            loadDoor();
        }
    }
    public void loadBossRoom()
    {
        BossNumber = PlayerPrefs.GetInt("BossNumber");
        loadMap(BossNumber);
        PlayerPrefs.SetInt("BossNumber", BossNumber);
        //Invoke("loadTalk", 2f);
        UiButton.SetActive(true);
        Debug.Log("BossNumber:"+PlayerPrefs.GetInt("BossNumber"));
    }
    void loadMap(int Number)
    {
        if (Number < RoomNames.Length)
        {
            PoolManager.GetInstance().GetObj("prefabs/BossRoom/" + RoomNames[Number], (o) =>
            {
                o.transform.SetParent(transform);
                o.transform.position = Character.transform.position;
                Camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = o.GetComponent<PolygonCollider2D>();
                DoorPos = o.transform.FindChild("Door").transform.position;
                Character.transform.position = o.transform.FindChild("BirthPoint").transform.position;
            });
        }
        else
        {
            
                if ((PlayerPrefs.GetFloat("EnemyHpRate") - 1.0f) / 0.2f >= PlayerPrefs.GetInt("DifficultLevel"))
                {
                    PlayerPrefs.SetInt("DifficultLevel", PlayerPrefs.GetInt("DifficultLevel") + 1);
                }
                PlayerPrefs.SetInt("GameOver", 1);
                SceneManager.LoadScene("MainMenu");
            
        }
        
    }
    public void timeStart()
    {
        if (Count > 5)
        {
            Time.timeScale = 1;
            UiButton.SetActive(true);
            TalkPanel.SetActive(false);
        }
        else
        {
            Count += 1;
            Talk.text = Count.ToString();
        }
    }
    void loadTalk()
    {
        Time.timeScale = 0;
        Count = 0;
        UiButton.SetActive(false);
        Talk.text = "0";
        TalkPanel.SetActive(true);
    }
    public void loadDoor()
    {
        PoolManager.GetInstance().GetObj("prefabs/BossRoom/BossDoor" , (o) =>
        {
            o.transform.SetParent(transform.parent);
            o.transform.position = DoorPos;
        });
    }
}
