using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelManner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Character,Camera,UiButton,Tips,NarrowPathBackGround;
    public string BedRoom,ClassRoom;
    public int TheNumToNext = 5,ChoiceLevel, Count;
    public AudioSource Next;
    public Item[] RewardItems;
    private string[] SportRoomNames = {"Room1","Room2","Room3"};
    private int  RewardItem1, RewardItem2,GetReward=-1;
    public Vector3 DoorPos1, DoorPos2, BedRoomBirth = new Vector3(15, 6, 0);
    void Start()
    {
        Count = PlayerPrefs.GetInt("LevelCount");
        ChoiceLevel = PlayerPrefs.GetInt("ChoiceLevel");
        GetReward = PlayerPrefs.GetInt("Reward");
        //getLevel();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            loadDoor();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            loadShop();
        }
        switchLevel();
    }
    public void getLevel()
    {
        NarrowPathBackGround.SetActive(false);
        RewardItem1 = Random.Range(0, RewardItems.Length);
        RewardItem2 = Random.Range(0, RewardItems.Length);
        if (Count >=TheNumToNext)
        {
            ChoiceLevel += 1;
            PlayerPrefs.SetInt("ChoiceLevel", ChoiceLevel);
            Count = 0;
            PlayerPrefs.SetInt("LevelCount", 0);
             Tips.SetActive(true);
                getBoss();
        }
        else
        {
            if (ChoiceLevel == 3)
            {
                TheNumToNext = 4;
                loadMapPart(ClassRoom, new Vector3(24, 12, 0));
            }
            else if (ChoiceLevel == 2)
            {
                TheNumToNext = 4;
                int index = Random.Range(0, SportRoomNames.Length);
                loadMapPart("prefabs/mappart/SportRoom/" + SportRoomNames[index], new Vector3(0, 0, 0));
            }
            else if (ChoiceLevel == 1)
            {
                TheNumToNext = 4;
                loadMapPart(BedRoom, BedRoomBirth);
            }
            else if (ChoiceLevel == 4)
            {
                TheNumToNext = 2;
                //NarrowPathBackGround.transform.localScale = new Vector3(1.04f * (Screen.width / 2160), (Screen.height / 1080), 0);
                NarrowPathBackGround.SetActive(true);
                loadMapPart("prefabs/mappart/NarrowPath", new Vector3(18f, 0, 0));
            }
            else 
            {
                Tips.SetActive(true);
                getBoss();
            }
            
            PlayerPrefs.SetInt("LevelCount", Count);
            Count += 1;

        }
        Character.SetActive(true);
        
        UiButton.SetActive(true);

    }
    private void loadMapPart(string PartName,Vector3 FixPos)
    {
        PoolManager.GetInstance().GetObj(PartName, (o) =>
        {
            o.transform.SetParent(transform);
            o.transform.position = Character.transform.position + FixPos;
            Camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = o.GetComponent<PolygonCollider2D>();
            DoorPos1 = o.transform.Find("Door1").transform.position;
            DoorPos2 = o.transform.Find("Door2").transform.position;
            Character.transform.position = new Vector3(o.transform.FindChild("BirthPoint").transform.position.x, o.transform.FindChild("BirthPoint").transform.position.y,0);
        });
    }
    public void getNextAfter(float WaitTime)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.StartsWith("prefabs/"))
            {
                //Debug.Log(transform.GetChild(i).name);
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        Character.SetActive(false);
        Tips.SetActive(true);
        UiButton.SetActive(false);
        if (PlayerPrefs.GetInt("inShop") == 1)
        {
            Invoke("loadShop", WaitTime);
            if(TheNumToNext>Count)Invoke("offTips", WaitTime + 0.75f);
        }
        else
        {
            Invoke("getLevel", WaitTime);
            if (TheNumToNext > Count) Invoke("offTips", WaitTime + 0.75f);
        }
    }
    public void checkRewardNum(int DoorNum)
    {
        GetReward = DoorNum;
        PlayerPrefs.SetInt("Reward", GetReward);
    }
    public void offTips()
    {
        Tips.SetActive(false);
    }
    public void loadDoor()
    {
        Next.Play();
        if (GetReward > -1)
        {
            PoolManager.GetInstance().GetObj("prefabs/StuffOnGround/" + RewardItems[GetReward].ItemName, (o) => 
            {
                o.transform.SetParent(transform);
                o.transform.position = new Vector3((DoorPos1.x + DoorPos2.x) / 2-1f, (DoorPos1.y + DoorPos2.y) / 2, 0);
            });
        }
        if (Count >= TheNumToNext)
        {
            PoolManager.GetInstance().GetObj("prefabs/mappart/DoorToBoss", (o) =>
            {
                o.transform.SetParent(transform);
                o.transform.GetComponent<Door>().DoorNum = -2;
                o.transform.position = new Vector3((DoorPos1.x+DoorPos2.x)/2, (DoorPos1.y + DoorPos2.y) / 2, 0);
                //o.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = RewardItems[RewardItem1].ItemIcon;
            });
        }
        else
        {
            if (RewardItem1 == RewardItems.Length - 1)
            {
                PoolManager.GetInstance().GetObj("prefabs/mappart/Door", (o) =>
                {
                    o.transform.SetParent(transform);
                    o.transform.GetComponent<Door>().DoorNum = -1;
                    o.transform.position = new Vector3(DoorPos1.x, DoorPos1.y, 1);
                    o.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = RewardItems[RewardItem1].ItemIcon;
                });
            }
            else
            {
                PoolManager.GetInstance().GetObj("prefabs/mappart/Door", (o) =>
                {
                    o.transform.SetParent(transform);
                    o.transform.GetComponent<Door>().DoorNum = RewardItem1;
                    o.transform.position = new Vector3(DoorPos1.x, DoorPos1.y, 1);
                    o.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = RewardItems[RewardItem1].ItemIcon;
                });
            }
            if (RewardItem2 == RewardItems.Length - 1)
            {
                PoolManager.GetInstance().GetObj("prefabs/mappart/Door", (o) =>
                {
                    o.transform.SetParent(transform);
                    o.transform.GetComponent<Door>().DoorNum = -1;
                    o.transform.position = new Vector3(DoorPos2.x, DoorPos2.y, 1);
                    o.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = RewardItems[RewardItem2].ItemIcon;
                });
            }
            else
            {
                PoolManager.GetInstance().GetObj("prefabs/mappart/Door", (o) =>
                {
                    o.transform.SetParent(transform);
                    o.transform.GetComponent<Door>().DoorNum = RewardItem2;
                    o.transform.position = new Vector3(DoorPos2.x, DoorPos2.y, 1);
                    o.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = RewardItems[RewardItem2].ItemIcon;
                });
            }
        }
        

    }
    public void loadShop()
    {
        NarrowPathBackGround.SetActive(false);
        Character.SetActive(true);
        Tips.SetActive(false);
        UiButton.SetActive(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.StartsWith("prefabs/mappart"))
            {
                //Debug.Log(transform.GetChild(i).name);
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        RewardItem1 = Random.Range(0, RewardItems.Length-1);
        RewardItem2 = Random.Range(0, RewardItems.Length-1);
        loadMapPart("prefabs/mappart/Shop",new Vector3(16,8,0));
        GetReward = -1;
        Invoke("loadDoor",3f);
    }
    public void switchLevel()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ChoiceLevel = 1;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            ChoiceLevel = 2;
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            ChoiceLevel = 3;
        }
    }
    public void getBoss()
    {
        //Tips.SetActive(false);
        GameObject.Find("TotalControl").GetComponent<TotalControl>().loadHorizontalPlate();
    }
}
