using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManner : MonoBehaviour
{
    // Start is called before the first frame update
    public Image ATK,HP,MoveSpeed,Gold,Energy;
    public Text KnowledgeNum, ATKt, HPt, MoveSpeedt, Goldt, Energyt;
    public float atk=-0.2f,hp=-0.2f,movespeed=-0.2f,gold=-0.2f,energy=-0.2f;
    public int DifficultLevel = 0, KnowledgePoints;
    public GameObject SelfStudy,mainmenu,uicharacter,setdown,DifficultOfSelect;
    public Button D,C,B,A,S;
    public Animator uicharacterani,selfstudyani;

    void Start()
    {
        loadData();
/*        ATKUp();
        HPUp();
        MoveSpeedUp();
        GoldUp();*/
        uicharacterani = uicharacter.GetComponent<Animator>();
        selfstudyani = SelfStudy.GetComponent<Animator>();
        fixDifficult();
    }

    // Update is called once per frame
    void Update()
    {
/*        fixDifficult();
        if (Input.GetKeyDown(KeyCode.M))
        {
            DifficultLevel += 1;
            
            if (DifficultLevel > 5) DifficultLevel = -1;
            PlayerPrefs.SetInt("DifficultLevel", DifficultLevel);
        }*/
    }
    public void StartGame()
    {
        mainmenu.SetActive(false);
        PlayerPrefs.SetInt("ChoiceLevel", 1);
        PlayerPrefs.SetInt("LevelCount", 0);
        PlayerPrefs.SetInt("BossNumber", 0);
        PlayerPrefs.SetInt("Reward", -1);
        PlayerPrefs.SetInt("InBoss", 0);
        PlayerPrefs.SetFloat("HP",150);
        PlayerPrefs.SetInt("MagicMode1", 0);
        PlayerPrefs.SetInt("AttackMode", 1);
        PlayerPrefs.SetFloat("Time",0);
        PlayerPrefs.SetInt("inShop", 0);
        PlayerPrefs.SetInt("GameOver", 0);
        uicharacterani.Play("UIchr");
        PlayerPrefs.SetFloat("HP", 150*(PlayerPrefs.GetFloat("hp") + 1));
        PlayerPrefs.SetFloat("attack", 2f*(PlayerPrefs.GetFloat("atk")+1));
        PlayerPrefs.SetFloat("goldplayer", 100*(PlayerPrefs.GetFloat("gold")));
        PlayerPrefs.SetFloat("sprintPower", 2);
        PlayerPrefs.SetFloat("soundBulletPower", 3);
        PlayerPrefs.SetFloat("normalAttackPower", 1);
        PlayerInfo.GetInstance().initData();
        Invoke("loadscene", 1.9f);
    }
    public void ContinueGame()
    {
        if ((PlayerPrefs.GetInt("GameOver") == 0)&&PlayerPrefs.HasKey("GameOver"))
        {
            PlayerInfo.GetInstance().initData();
            mainmenu.SetActive(false);
            uicharacterani.Play("UIchr");
            Invoke("loadscene", 1.9f);
        }

    }
    public void e()
    {
        PlayerPrefs.SetFloat("EnemyHpRate",1.0f);
        PlayerPrefs.SetFloat("EnemyAttRate", 1.0f);
    }
     public void d()
    {
        PlayerPrefs.SetFloat("EnemyHpRate",1.2f);
        PlayerPrefs.SetFloat("EnemyAttRate", 1.2f);
    }
    public void c()
    {
        PlayerPrefs.SetFloat("EnemyHpRate", 1.4f);
        PlayerPrefs.SetFloat("EnemyAttRate", 1.4f);
    }
    public void b()
    {
        PlayerPrefs.SetFloat("EnemyHpRate", 1.6f);
        PlayerPrefs.SetFloat("EnemyAttRate", 1.6f);
    }
    public void a()
    {
        PlayerPrefs.SetFloat("EnemyHpRate", 1.8f);
        PlayerPrefs.SetFloat("EnemyAttRate", 1.8f);
    }
    public void s()
    {
        PlayerPrefs.SetFloat("EnemyHpRate", 2.0f);
        PlayerPrefs.SetFloat("EnemyAttRate", 2.0f);
    }
    public void loadscene() {
        SceneManager.LoadScene("character");
    }
    public void Study()
    {

        uicharacterani.Play("uicharacterstudy");
        mainmenu.SetActive(false);
        Invoke("loadStudymenu",0.9f);
    }
    public void fixKnowledge()
    {
        KnowledgeNum.text = KnowledgePoints.ToString();
    }
    public void loadStudymenu()
    {
        fixKnowledge();
        setdown.SetActive(true);
        SelfStudy.SetActive(true);
    }
    public void outstudy()
    {

        selfstudyani.Play("outselfstudy");
        Invoke("outstudychar", 1.6f);
        
    }
    public void outstudychar()
    {
        SelfStudy.SetActive(false);
        setdown.SetActive(false);
        uicharacterani.Play("uicharacteroutstudy");
        Invoke("loadmainlist", 0.8f);
    }
    public void loadmainlist()
    {
        mainmenu.SetActive(true);
    }
    public void ATKUp()
    {
        if ((KnowledgePoints > atk/0.2f)&&(atk<1))
        {
            atk += 0.2f;
            ATK.fillAmount = atk;
            Debug.Log("Atkup");
            PlayerPrefs.SetFloat("atk", atk);
            KnowledgePoints -= (int)(atk / 0.2f);
            ATKt.text= ((int)(atk / 0.2f)).ToString();
            PlayerPrefs.SetInt("KnowledgePoints", KnowledgePoints);
            fixKnowledge();
        }
        
    }
    public void HPUp()
    {
        if ((KnowledgePoints > hp / 0.2f)&&(hp<1))
        {
            hp += 0.2f;
            HP.fillAmount = hp;
            PlayerPrefs.SetFloat("hp", hp);
            KnowledgePoints -= (int)(hp / 0.2f);
            HPt.text= ((int)(hp / 0.2f)).ToString();
            PlayerPrefs.SetInt("KnowledgePoints", KnowledgePoints);
            fixKnowledge();
        }

    }
    public void MoveSpeedUp()
    {
        if ((KnowledgePoints > movespeed / 0.2f)&&(movespeed<1))
        {
            movespeed += 0.2f;
            MoveSpeed.fillAmount = movespeed;
            PlayerPrefs.SetFloat("movespeed", movespeed);
            KnowledgePoints -= (int)(movespeed/ 0.2f);
            MoveSpeedt.text = ((int)(movespeed / 0.2f)).ToString();
            PlayerPrefs.SetInt("KnowledgePoints", KnowledgePoints);
            fixKnowledge();
        }

    }
    public void GoldUp()
    {
        if ((KnowledgePoints > gold / 0.2f)&&(gold<1))
        {
            gold += 0.2f;
            Gold.fillAmount = gold;
            PlayerPrefs.SetFloat("gold", gold);
            KnowledgePoints -= (int)(gold / 0.2f);
            Goldt.text = ((int)(gold / 0.2f)).ToString();
            PlayerPrefs.SetInt("KnowledgePoints", KnowledgePoints);
            fixKnowledge();
        }

    }
    public void EnergyUp()
    {
        if ((KnowledgePoints > energy / 0.2f)&&(energy<1))
        {
            energy += 0.2f;
            Energy.fillAmount = energy;
            PlayerPrefs.SetFloat("energy", energy);
            KnowledgePoints -= (int)(energy / 0.2f);
            Energyt.text = ((int)(energy / 0.2f)).ToString();
            PlayerPrefs.SetInt("KnowledgePoints", KnowledgePoints);
            fixKnowledge();
        }

    }
    public void openDofS()
    {
        mainmenu.SetActive(false);
        DifficultOfSelect.SetActive(true);
    }
    public void outDofS()
    {
        mainmenu.SetActive(true);
        DifficultOfSelect.SetActive(false);
    }
    void fixDifficult()
    {
        if (DifficultLevel > 1)
        {
            C.interactable = true;

        }
        else
        {
            C.interactable = false;
        }
        if (DifficultLevel > 0)
        {
            D.interactable = true;
        }
        else
        {
            D.interactable = false;
        }
        if (DifficultLevel > 2)
        {
            B.interactable = true;
        }
        else
        {
            B.interactable = false;
        }
        if (DifficultLevel > 3)
        {
            A.interactable = true;
        }
        else
        {
            A.interactable = false;
        }
        if (DifficultLevel > 4)
        {
            S.interactable = true;
        }
        else
        {
            S.interactable = false;
        }
    }
    public void loadData()
    {
        if (!PlayerPrefs.HasKey("DifficultLevel"))
        {
            PlayerPrefs.SetInt("DifficultLevel", 0);
        }
        else
        {
            DifficultLevel = PlayerPrefs.GetInt("DifficultLevel");
        }
        if (!PlayerPrefs.HasKey("atk"))
        {
            PlayerPrefs.SetFloat("atk", 0);
        }
        else
        {
            atk = PlayerPrefs.GetFloat("atk");
            ATK.fillAmount = atk;
            ATKt.text = ((int)(atk / 0.2f)).ToString();
        }
        if (!PlayerPrefs.HasKey("gold"))
        {
            PlayerPrefs.SetFloat("gold", 0);
        }
        else
        {
            gold = PlayerPrefs.GetFloat("gold");
            Gold.fillAmount = gold;
            Goldt.text= ((int)(gold / 0.2f)).ToString();
        }
        if (!PlayerPrefs.HasKey("hp"))
        {
            PlayerPrefs.SetFloat("hp", 0);
        }
        else
        {
            hp = PlayerPrefs.GetFloat("hp");
            HP.fillAmount = hp;
            HPt.text= ((int)(hp / 0.2f)).ToString();
        }
        if (!PlayerPrefs.HasKey("movespeed"))
        {
            PlayerPrefs.SetFloat("movespeed", 0);
        }
        else
        {
            movespeed = PlayerPrefs.GetFloat("movespeed");
            MoveSpeed.fillAmount = movespeed;
            MoveSpeedt.text= ((int)(movespeed / 0.2f)).ToString();
        }
        if (!PlayerPrefs.HasKey("energy"))
        {
            PlayerPrefs.SetFloat("energy", 0);
        }
        else
        {
            energy = PlayerPrefs.GetFloat("energy");
            Energy.fillAmount = energy;
            Energyt.text = ((int)(energy / 0.2f)).ToString();
        }
        if (!PlayerPrefs.HasKey("KnowledgePoints"))
        {
            PlayerPrefs.SetInt("KnowledgePoints", 0);
        }
        else
        {
            KnowledgePoints = PlayerPrefs.GetInt("KnowledgePoints");
            //KnowledgePoints = 99;
        }
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
