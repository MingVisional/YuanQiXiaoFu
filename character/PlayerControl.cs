using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public float movespeed = 5f, nextatt = 0.6f, nextsprint = 2f, nextmagic = 2f;
    private float canattack = 0, cansprint = 0, canmagic = 0,sprinttime, MaxComboInterval = 1.2f, MinComboInterval = 0.6f;
    public float HP,hurtfor=250f,ATK,MaxHp, Energy,MaxEnergy=100, EnergyRecoverySpeed;
    public Rigidbody2D rig;
    public GameObject hpimg,energy,DeathPanel,UiButton;
    private GameObject att, magic2,Defense;
    private int AttackMode = 1,MagicMode1=1,MagicMode2=1;
    public int CoinNumber = 1000,KnowedgeNum = 0;
    public Sprite Boxing, Hands,Horn,Cloud,Fan;
    public Image AttButton,Magic1,Magic2;
    private bool isatt, isstop = true, inmagic2,insprint,doAttack;
    private float joyhor, joyver,attackcount,attackgap=1f;
    private string[] Attacks = { "Attack", "Boxing" };
    public SpriteRenderer spriteRenderer;
    public Joystick joy;
    public AudioSource Hit,Magic,hurt,Sprint;
    private GameObject sprintit;
    public Animator ani;
    void Start()
    {
        att = transform.GetChild(0).gameObject;
        
        //magic2 = transform.GetChild(1).gameObject;
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        sprintit = transform.Find("sprint1").gameObject;
        attackcount = -1;
        Defense = transform.Find("Defense").gameObject;
        //HP = PlayerPrefs.GetFloat("HP");
        //hpimg.GetComponent<Play_HP>().Change(HP / 100f);
        MagicMode1 = PlayerPrefs.GetInt("MagicMode1");
        AttackMode = PlayerPrefs.GetInt("AttackMode");
        changeAttack(AttackMode);
        ChangeMagic(MagicMode1);
        MaxEnergy = PlayerPrefs.GetFloat("energy") * 100 + 100;
        movespeed = PlayerPrefs.GetFloat("movespeed") * 1.5f + 4;
        Energy = MaxEnergy;
    }

    [System.Obsolete]
    void Update()
    {
        if (!DeathPanel.active)
        {
            move();
            fixani();
        }
        SwitchAbility();
        /*        if (Input.GetKeyDown(KeyCode.N))
                {
                    HP -= 10f;
                    //hpimg.GetComponent<Play_HP>().Change(HP / 100f);
                }*/
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("MagicMode:" + MagicMode1.ToString() + "PlayerPrefs:" + PlayerPrefs.GetInt("MagicMode1"));
            Debug.Log("AttackMode:" + AttackMode.ToString() + "PlayerPrefs:" + PlayerPrefs.GetInt("AttackMode"));
        }
        if (PlayerInfo.GetInstance().Hp <= 0)
        {
            UiButton.SetActive(false);
            DeathPanel.SetActive(true);
            rig.velocity = new Vector2(0, 0);
            PlayerPrefs.SetInt("GameOver",1);
            ani.SetBool("hurted", true);
            ani.Play("hurt");
            att.SetActive(false);
        }
        fixEnergy();
    }
    void speedmanner()
    {
        if (System.Math.Abs(rig.velocity.x) > movespeed)
        {
            if (rig.velocity.x > 0)
            {
                rig.velocity = new Vector2(movespeed, rig.velocity.y);
            }
            else
            {
                rig.velocity = new Vector2(-movespeed, rig.velocity.y);
            }
        }
    }
    void move()
    {
        joyhor = joy.Horizontal;
        joyver = joy.Vertical;
        if (isstop)
        {

            rig.velocity = new Vector2(movespeed * joyhor, movespeed * joyver);
            if (joyhor > 0) transform.localScale = new Vector3(1, 1, 0);
            else if (joyhor < 0) transform.localScale = new Vector3(-1, 1, 0);
            if (Input.GetKey(KeyCode.D))
            {
                transform.localScale = new Vector3(1, 1, 0);
                if (Input.GetKey(KeyCode.W))
                {
                    rig.velocity = new Vector2(movespeed * 0.707f, movespeed * 0.707f);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    rig.velocity = new Vector2(movespeed * 0.707f, -movespeed * 0.707f);
                }
                else
                {
                    rig.velocity = new Vector2(movespeed, 0);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.localScale = new Vector3(-1, 1, 0);
                if (Input.GetKey(KeyCode.W))
                {
                    rig.velocity = new Vector2(-movespeed * 0.707f, movespeed * 0.707f);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    rig.velocity = new Vector2(-movespeed * 0.707f, -movespeed * 0.707f);
                }
                else
                {
                    rig.velocity = new Vector2(-movespeed, 0);
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                rig.velocity = new Vector2(0, movespeed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rig.velocity = new Vector2(0, -movespeed);
            }
        }

        if (!isatt)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                attack();
            }
            else if (Input.GetKey(KeyCode.K))
            {
                magic_a();
            }
        }
        if((Input.GetKeyDown(KeyCode.L)) && (!insprint))
        {
            sprint1();
        }
         else if ((Input.GetKeyUp(KeyCode.L)) && (sprinttime != 0))
        {
            sprint2();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Defenseenter();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Defenseout();
        }

    }
    public void attack()
    {
        /*        if (canattack < Time.time)*/
        {
            /*            canattack = nextatt + Time.time;*/
            /*ani.SetBool("attack", true);
            ani.Play("attack", 0, 0);*/
            if (attackcount < 0 || (Time.time - attackgap > MaxComboInterval))
            {
                attackcount = 0f;
                doAttack = true;
                attackgap = Time.time;
            }
            else if ((Time.time - attackgap) > MinComboInterval)
            {
                attackcount = 1f + attackcount;
                doAttack = true;
                attackgap = Time.time;
            }
            if (doAttack)
            {
                if (attackcount > 1.5)
                {
                    ani.Play(Attacks[AttackMode - 1] + "_c");
                    Invoke("fixattackcount", MinComboInterval);
                    //attackcount = -1;
                }
                else if (attackcount > 0.5)
                {
                    ani.Play(Attacks[AttackMode - 1] + "_b");

                }
                else if (attackcount >= 0)
                {
                    ani.Play(Attacks[AttackMode - 1] + "_a");
                }
                doAttack = false;
                Hit.Play();
            }
            isatt = true;
            //att.gameObject.GetComponent<doattack>().getattack();
            Invoke("fixattack", 0.2f);
        }
    }
    public void Hurt(float hurtnum,float toward)
    {
        if (!ani.GetBool("hurted")&&(!insprint))
        {
            //Hurt.Play();
            Vector2 hurtforce = new Vector2(toward* hurtfor, 0);
            fixspeed();
            sprintit.SetActive(false);

            //rig.AddForce(hurtforce);
                ani.SetBool("hurted", true);
                ani.Play("hurt");
                rig.velocity = new Vector2(toward * movespeed * 0.4f, 0);
            isstop = false;
            isatt = true;
            insprint = true;
            Invoke("fixspeed", 0.4f);
            Invoke("fixattack", 0.4f);
            //HP = HP - hurtnum;
            PlayerInfo.GetInstance().Hp -= hurtnum ;
            //hpimg.GetComponent<Play_HP>().Change(HP / 100f);
        }

    }
    public void sprint2()
    {
        if (sprintit.active)
        {
            Sprint.Play();
            insprint = true;
            ani.SetBool("dosprint", true);
            sprintit.SetActive(false);
            ani.Play("Sprint1", 0, 0);
            rig.velocity = new Vector2(transform.localScale.x * movespeed * 2, 0);
            cansprint = nextsprint + Time.time;
            isstop = false;
            float tr = Time.time - sprinttime;
            if (tr > 1f) tr = 1f;
            Invoke("fixspeed", tr+0.25f);
        }
        
        
    }
    public void sprint1()
    {
        if(Energy >=20f)
        {
            Energy -= 20f;
            if (!insprint)
            {
                if (!ani.GetBool("hurted") && !isatt)
                {
                    isatt = true;
                    sprintit.SetActive(true);
                    sprinttime = Time.time;
                    ani.SetBool("sprint", true);
                    ani.Play("sprint", 0, 0);
                    rig.velocity = new Vector2(0, 0);
                    isstop = false;
                }
            }
        }

    }
    public void magic_a()
    {
        if (Time.time > canmagic&&Energy>40f&&MagicMode1!=0)
        {
            Energy -= 40f;
            Magic.Play();
            canmagic = Time.time + nextmagic;
/*            ani.SetBool("magic1", true);
            ani.Play("Magic1");*/
            Invoke("fixattack", 0.4f);
            isstop = false;
            rig.velocity = new Vector2(0, 0);
            Invoke("fixspeed", 0.3f);
            isatt = true;
            if (MagicMode1 == 1)
            {
                ani.SetBool("magic1", true);
                ani.Play("Magic1"); 
                     PoolManager.GetInstance().GetObj("prefabs/magic_1", (o) =>
                {
                    o.transform.position = transform.position + new Vector3(0, 0.3f, 0);
                    o.transform.localScale = new Vector2(transform.localScale.x * 2, 2f);
                    o.transform.SetParent(this.transform.parent);
                    o.GetComponent<bulletn>().Init();
                });
                Energy += 19f;
            }
            else if (MagicMode1 == 2)
            {
                ani.SetBool("Fan", true);
                ani.Play("CharFan");
                PoolManager.GetInstance().GetObj("prefabs/other_0", (o) =>
                {
                    o.transform.position = transform.position + new Vector3(transform.localScale.x * 1f, 2f, 0);
                    o.transform.localScale = new Vector2(transform.localScale.x * 4, 4f);
                    o.transform.SetParent(this.transform.parent);
                    o.GetComponent<Cloud>().Init();
                });
            }
            else if (MagicMode1 == 3)
            {
                callFan();
                Energy += 19f;
            }
            Debug.Log("MagicMode" + MagicMode1.ToString());
        }
    }
    void callFan()
    {
        ani.SetBool("Fan", true);
        ani.Play("CharFan");
        PoolManager.GetInstance().GetObj("prefabs/Fan", (o) =>
        {
            o.transform.position = transform.position + new Vector3(transform.localScale.x, 0, 0);
            o.transform.localScale = new Vector2(transform.localScale.x * 1, 1f);
            o.transform.SetParent(this.transform.parent);
            o.GetComponent<Fan>().Init();
        });
    }
    public void magic_b()
    {
        if (Time.time > canmagic)
        {
            canmagic = Time.time + nextmagic;
            /*            ani.SetBool("magic1", true);
                        ani.Play("Magic1");*/
            Invoke("fixattack", 0.4f);
            isstop = false;
            rig.velocity = new Vector2(0, 0);
            Invoke("fixspeed", 0.3f);
            isatt = true;
            if (MagicMode2 == 1)
            {
                ani.SetBool("magic1", true);
                ani.Play("Magic1");
                PoolManager.GetInstance().GetObj("prefabs/magic_1", (o) =>
                {
                    o.transform.position = transform.position + new Vector3(0, 0.3f, 0);
                    o.transform.localScale = new Vector2(transform.localScale.x * 2, 2f);
                    o.transform.SetParent(this.transform.parent);
                    o.GetComponent<bulletn>().Init();
                });
            }
            else if (MagicMode2 == 2)
            {
                PoolManager.GetInstance().GetObj("prefabs/other_0", (o) =>
                {
                    o.transform.position = transform.position + new Vector3(0, 0.3f, 0);
                    o.transform.localScale = new Vector2(transform.localScale.x * 2, 2f);
                    o.transform.SetParent(this.transform.parent);
                    o.GetComponent<bulletn>().Init();
                });
            }
            else if (MagicMode2 == 3)
            {
                if ((Input.GetKeyDown(KeyCode.L)) && (!insprint))
                {

                    sprint1();
                }
                else if ((Input.GetKeyUp(KeyCode.L)) && (sprinttime != 0))
                {
                    sprint2();
                }
            }
            Debug.Log("MagicMode1"+MagicMode1.ToString());
        }
    }
    void fixattack()
    {
        ani.SetBool("magic1", false);
        ani.SetBool("Fan", false);
        //ani.SetBool("magic2-1", false);
        /*ani.SetBool("attack", false);*/
        isatt = false;
    }
    void fixspeed()
    {
        ani.SetBool("sprint", false);
        ani.SetBool("dosprint", false);
        rig.velocity = new Vector2(0, 0);
        ani.SetBool("hurted", false);
        insprint = false;
        isstop = true;
        isatt = false;
        sprinttime = 0;

    }
    void fixmagic2()
    {
        ani.SetBool("magic2-2", true);
        ani.Play("magic2-2");
        isstop = false;
        magic2.SetActive(true);
        rig.velocity = new Vector2(0, 0);
        ani.SetBool("magic2-1", false);
        Invoke("fixmagic22", 0.6f);
    }
    void fixmagic22()
    {
        magic2.SetActive(false);
        ani.SetBool("magic2-2", false);
        isstop = true;
        movespeed = 5f;
        isatt = false;
    }
    void fixattackcount()
    {
        attackcount = -1;
    }
    void fixani()
    {
        if ((Time.time - attackgap) > MaxComboInterval)
        {
            attackcount = -1;
        }
        if (AttackMode == 1)
        {
            ani.SetFloat("Attackcount", attackcount);
            ani.SetFloat("BoxingCount", 0);
        }
        else if (AttackMode == 2)
        {
            ani.SetFloat("BoxingCount", attackcount);
            ani.SetFloat("Attackcount", 0);
        }
        if (isstop)
        {
            if (rig.velocity.x == 0 && rig.velocity.y == 0)
            {
                ani.SetBool("ismove", false);
            }
            else
            {
                ani.SetBool("ismove", true);
            }
        }
        
    }
    void Defenseenter()
    {
        if (!ani.GetBool("hurted"))
        {
            rig.velocity = new Vector2(0, 0);
            ani.SetBool("ismove", false);
            isstop = false;
            ani.Play("Defense1");
            Defense.SetActive(true);
        }
            
    }
    public void loadDoorFix()
    {
        //att.SetActive(false);
        movespeed= PlayerPrefs.GetFloat("movespeed") * 1.5f + 4;
        //fixBullet();
    }
    void Defenseout()
    {
        ani.Play("Defense2");
        isstop = true;
        Defense.SetActive(false);
    }
    void SwitchAbility()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            MaxComboInterval = 1.2f;
            MinComboInterval = 0.4f;
            AttButton.sprite = Hands;
            AttackMode = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MaxComboInterval = 0.6f;
            MinComboInterval = 0.2f;
            AttButton.sprite = Boxing;
            AttackMode = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MagicMode1 = 1;
            Magic1.sprite = Horn;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MagicMode1 = 2;
            Magic1.sprite = Cloud;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            MagicMode1 = 3;
            Magic1.sprite = Fan;
        }
    }
    void fixEnergy()
    {
        if (Energy < MaxEnergy)
        {
            Energy += Time.deltaTime*EnergyRecoverySpeed;

        }
        energy.GetComponent<Play_HP>().Change( Energy/ MaxEnergy);
    }
    public void fixData()
    {
        PlayerPrefs.SetInt("MagicMode1", MagicMode1);
        PlayerPrefs.SetInt("AttackMode", AttackMode);
        PlayerPrefs.SetInt("KnowledgePoints", PlayerPrefs.GetInt("KnowledgePoints") + KnowedgeNum);
        KnowedgeNum = 0;
    }
    public void changeAttack(int num)
    {
        if (num == AttackMode)
        {
            PlayerInfo.GetInstance().NormalAttackPower = PlayerInfo.GetInstance().NormalAttackPower * 1.2f;
        }
        else
        {
            PlayerInfo.GetInstance().NormalAttackPower = 1f;
        }
        if (num==1)
        {
            MaxComboInterval = 1.2f;
            MinComboInterval = 0.4f;
            AttButton.sprite = Hands;
            AttackMode = 1;
        }
        else if (num==2)
        {
            MaxComboInterval = 0.6f;
            MinComboInterval = 0.2f;
            AttButton.sprite = Boxing;
            PlayerInfo.GetInstance().NormalAttackPower = 1.2f;
            AttackMode = 2;
        }
        
    }
    public void ChangeMagic(int Mode)
    {
        Magic1.gameObject.SetActive(true);
        if (Mode == MagicMode1)
        {
            PlayerInfo.GetInstance().SoundBulletPower = PlayerInfo.GetInstance().SoundBulletPower * 1.2f;
        }
        else
        {
            PlayerInfo.GetInstance().SoundBulletPower = 3f;
        }
        if (Mode==1)
        {
            MagicMode1 = 1;
            Magic1.sprite = Horn;

        }
        else if (Mode==2)
        {
            MagicMode1 = 2;
            Magic1.sprite = Cloud;
        }
        else if (Mode == 3)
        {
            MagicMode1 = 3;
            Magic1.sprite = Fan;
        }
       else if (Mode == 0)
        {
            Magic1.gameObject.SetActive(false);
            MagicMode1 = 0;
        }
    }
    public void downCoin(int num)
    {
        CoinNumber -= num;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("EnemyBall"))
        {
            hurted();
        }*/

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //AttIt.Play();
            if (insprint && !ani.GetBool("hurted"))
            {
                collision.GetComponent<EnemyFSM>().Hurt(PlayerInfo.GetInstance().Attack * PlayerInfo.GetInstance().SprintPower*0.55f);
            }
            
        }
        else if (collision.CompareTag("BOSS"))
        {
            //AttIt.Play();
            collision.GetComponent<BOSSFSM>().Hurt(PlayerInfo.GetInstance().Attack * PlayerInfo.GetInstance().SprintPower);
        }
    }
    public void fixBullet()
    {
        for(int i = 0; i < transform.parent.childCount;i++)
        {
            if(transform.parent.GetChild(i).name== "prefabs/Fan")
            {
                Destroy(transform.parent.GetChild(i));
            }
        }
    }
}

