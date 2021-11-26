using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalControl : MonoBehaviour
{
    public GameObject TopView, HorizontalPlate,Tips;
    public Text Timet;
    public int KillCount, SumOfTheLevel=100;
    private int InBoss;
    private float TimeCount;
    void Start()
    {
        InBoss = PlayerPrefs.GetInt("InBoss");
        if (InBoss == 1)
        {
            //PlayerPrefs.SetInt("BossNumber", PlayerPrefs.GetInt("BossNumber") - 1);
            loadHorizontalPlate();
        }
        else
        {
            //PlayerPrefs.SetInt("LevelCount", PlayerPrefs.GetInt("LevelCount") - 1);
            loadTopView();
        }
        TimeCount = PlayerPrefs.GetFloat("Time");
        Invoke("fixGoldAndHp",2f);
    }

    // Update is called once per frame
    void Update()
    {
        //TimeCount += Time.deltaTime;
        //Timet.text = ((int)(TimeCount) / 60).ToString() + "分" + ((int)(TimeCount) % 60).ToString() + "秒";
        if (KillCount >= SumOfTheLevel && PlayerPrefs.GetInt("InBoss")==0)
        {
            KillCount = 0;
            TopView.transform.FindChild("LevelManner").GetComponent<LevelManner>().loadDoor();

        }
    }
    public void loadTopView()
    {
        KillCount = 0;
        HorizontalPlate.SetActive(false);
        Tips.SetActive(true);
        Invoke("offTips", 2f);
        Debug.Log(PlayerPrefs.GetInt("MagicMode1"));
        PlayerPrefs.SetInt("InBoss", 0);
        TopView.SetActive(true);
        TopView.transform.FindChild("LevelManner").GetComponent<LevelManner>().getNextAfter(1);
        
    }
    public void loadHorizontalPlate()
    {
        KillCount = 0;
        TopView.SetActive(false);
        Tips.SetActive(true);
        Invoke("offTips", 2f);
        Debug.Log(PlayerPrefs.GetInt("MagicMode1"));
        PlayerPrefs.SetInt("InBoss", 1);
        HorizontalPlate.SetActive(true);
        HorizontalPlate.transform.FindChild("BossLevelManner").GetComponent<BossLevelManner>().loadBossRoom();
        HorizontalPlate.transform.FindChild("character").GetComponent<HorizontalControl>().updateAtt();
    }
    public void loadBossDoor()
    {
        HorizontalPlate.transform.FindChild("BossLevelManner").GetComponent<BossLevelManner>().loadDoor();
    }
    public void fixGoldAndHp()
    {
        PlayerInfo.GetInstance().Gold = PlayerPrefs.GetFloat("goldplayer");
        PlayerInfo.GetInstance().Hp = PlayerPrefs.GetFloat("HP");
    }
    public void offTips()
    {
        Tips.SetActive(false);
    }
}
