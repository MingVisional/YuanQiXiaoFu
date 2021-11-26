using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalControl : MonoBehaviour
{
    public float movespeed = 5f, nextatt = 0.6f, nextsprint = 2f, nextmagic = 2f,JumpForce=8f;
    private float canattack = 0, cansprint = 0, canmagic = 0, sprinttime,MaxComboInterval = 1.2f, MinComboInterval = 0.6f;
    public float HP = 100, hurtfor = 250f, ATK, MaxHp, Energy, MaxEnergy = 100, EnergyRecoverySpeed;
    public Rigidbody2D rig;
    public GameObject hpimg,energy,DeathPanel,UiButton;
    public Image AttButton,Magic1,Magic2;
    private GameObject att, magic2, Defense, sprintit;
    private bool isatt, isstop = true, inmagic2, insprint,isGround=true,doAttack;
    public Transform groundchack;
    private float joyhor, joyver, attackcount, attackgap = 1f;
    public Joystick joy;
    public LayerMask map;
    public Sprite Boxing,Hands,Horn,Cloud,Fan;
    public AudioSource Hit, Magic, hurt, Sprint,Jump;
    public Animator ani;
    public int jumpcount,MagicMode1,KnowedgeNum=0;
    private string[] Attacks = { "Attack", "Boxing" };
    private int AttackMode = 1;
    void Start()
    {
        att = transform.GetChild(0).gameObject;
        //magic2 = transform.GetChild(1).gameObject;
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
        movespeed = PlayerPrefs.GetFloat("movespeed") * 1.5f + 5;
        Energy = MaxEnergy;
    }
    void Update()
    {
        if (!DeathPanel.active)
        {
            move();
            fixani();
        }
        SwitchAbility();
        fixEnergy();
        /*        if (Input.GetKeyDown(KeyCode.N))
                {
                    HP -= 10f;
                    hpimg.GetComponent<Play_HP>().Change(HP / 100f);
                }*/
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("MagicMode:"+MagicMode1.ToString()+"PlayerPrefs:"+PlayerPrefs.GetInt("MagicMode1"));
            Debug.Log("AttackMode:" + AttackMode.ToString() + "PlayerPrefs:" + PlayerPrefs.GetInt("AttackMode"));
        }
        if (PlayerInfo.GetInstance().Hp <= 0)
        {
            UiButton.SetActive(false);
            rig.velocity = new Vector2(0, 0);
            DeathPanel.SetActive(true);
            PlayerPrefs.SetInt("GameOver", 1);
            ani.SetBool("hurted", true);
            ani.Play("hurt");
            att.SetActive(false);
        }

    }
    void FixedUpdate()
    {

        isGround = Physics2D.OverlapCircle(groundchack.position, 0.1f, map);
        if (isGround)
        {
            jumpcount = 1;
        }
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
            rig.velocity = new Vector2(movespeed * joyhor,rig.velocity.y);
            if (joyhor > 0) transform.localScale = new Vector3(1, 1, 0);
            else if (joyhor < 0) transform.localScale = new Vector3(-1, 1, 0);
            if (Input.GetKey(KeyCode.D))
            {
                transform.localScale = new Vector3(1, 1, 0);
                rig.velocity = new Vector2(movespeed, rig.velocity.y);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.localScale = new Vector3(-1, 1, 0);
                
                   rig.velocity = new Vector2(-movespeed, rig.velocity.y);
                
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump();
            }
        }

        if (!isatt)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                attack();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                magic_a();
            }
        }
        if ((Input.GetKeyDown(KeyCode.L)) && (!insprint)&&(isGround))
        {

            sprint1();
        }
        else if ((Input.GetKeyUp(KeyCode.L)) && (sprinttime != 0))
        {
            sprint2();
        }



    }
    public void jump()
    {
        if (jumpcount > 0)
        {
            Jump.Play();
            //Vector2 fi = new Vector2(0, jumpforce);
            //rig.AddForce(fi);
            ani.SetBool("isJump1", true);
            rig.velocity = new Vector2(rig.velocity.x, JumpForce);
            jumpcount -= 1;
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
            else if ((Time.time - attackgap )> MinComboInterval)
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
                    Debug.Log("ComboA");
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
    public void loadDoorFix()
    {
        //att.SetActive(false);
        movespeed = PlayerPrefs.GetFloat("movespeed") * 1.5f + 5;
    }
    public void Hurt(float hurtnum, float toward)
    {
        if (!ani.GetBool("hurted")&&(!insprint))
        {
            //Hurt.Play();
            Vector2 hurtforce = new Vector2(toward * hurtfor, 0);
            fixspeed();
            sprintit.SetActive(false);

                ani.SetBool("hurted", true);
                ani.Play("hurt");
                //rig.AddForce(hurtforce);
                rig.velocity = new Vector2(toward * movespeed * 0.4f, 0);
            

            isstop = false;
            isatt = true;
            insprint = true;
            Invoke("fixspeed", 0.2f);
            Invoke("fixattack", 0.2f);
            //HP = HP - hurtnum;
            PlayerInfo.GetInstance().Hp -= hurtnum;
           // hpimg.GetComponent<Play_HP>().Change(HP / 100f);
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
        if (Energy >= 20f&&isGround)
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
                    o.transform.position = transform.position + new Vector3(transform.localScale.x*1f, 2f, 0);
                    o.transform.localScale = new Vector2(transform.localScale.x * 4, 4f);
                    o.transform.SetParent(this.transform.parent);
                    o.GetComponent<Cloud>().Init();
                });
                Debug.Log("Cloud");
            }
            else if (MagicMode1 == 3)
            {
                callFan();
                Energy += 19f;
            }
            //Debug.Log("MagicMode" + MagicMode1.ToString());
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
    void fixEnergy()
    {
        if (Energy < MaxEnergy)
        {
            Energy += Time.deltaTime * EnergyRecoverySpeed;

        }
        energy.GetComponent<Play_HP>().Change(Energy / MaxEnergy);
    }
    public void magic()
    {
        if (Time.time > canmagic)
        {
            canmagic = Time.time + nextmagic;
            ani.SetBool("magic1", true);
            ani.Play("Magic1");
            Invoke("fixattack", 0.4f);
            isstop = false;
            rig.velocity = new Vector2(0, 0);
            Invoke("fixspeed", 0.3f);
            isatt = true;
            PoolManager.GetInstance().GetObj("prefabs/magic_1", (o) =>
            {
                o.transform.position = transform.position + new Vector3(0, 0.3f, 0);
                o.transform.localScale = new Vector2(transform.localScale.x * 2, 2f);
                o.transform.SetParent(this.transform.parent);
                o.GetComponent<bulletn>().Init();
            });


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
        if ((Time.time - attackgap )> MaxComboInterval)
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
        if (ani.GetBool("isJump1") && rig.velocity.y <= 0)
        {
            ani.SetBool("isJump2", true);
            ani.SetBool("isJump1", false);
        }
        if (!isGround && rig.velocity.y < 0)
        {
            ani.SetBool("isJump2", true);
            ani.SetBool("isJump1", false);
            ani.SetBool("ismove", false);
        }
        if (ani.GetBool("isJump2") && rig.velocity.y >= 0)
        {

            ani.SetBool("isJump2", false);
        }

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
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            MagicMode1 = 1;
            Magic2.sprite = Horn;

        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            MagicMode1 = 2;
            Magic2.sprite = Cloud;
        }
    }
    public void fixData()
    {
        PlayerPrefs.SetInt("MagicMode1", MagicMode1);
        PlayerPrefs.SetInt("AttackMode", AttackMode);
        PlayerPrefs.SetInt("KnowledgePoints", PlayerPrefs.GetInt("KnowledgePoints") + KnowedgeNum);
        KnowedgeNum = 0;
    }
    public void ChangeMagic(int Mode)
    {
        Magic2.gameObject.SetActive(true);
        if (Mode == MagicMode1)
        {
            PlayerInfo.GetInstance().SoundBulletPower = PlayerInfo.GetInstance().SoundBulletPower * 1.2f;
        }
        else
        {
            PlayerInfo.GetInstance().SoundBulletPower = 3f;
        }
        if (Mode == 1)
        {
            MagicMode1 = 1;
            Magic2.sprite = Horn;

        }
        else if (Mode == 2)
        {
            MagicMode1 = 2;
            Magic2.sprite = Cloud;
        }
        else if (Mode == 3)
        {
            MagicMode1 = 3;
            Magic2.sprite = Fan;
        }
        else if (Mode == 0)
        {
            MagicMode1 = 0;
            Magic2.gameObject.SetActive(false);
        }
    }
    public void updateAtt()
    {
        if(MagicMode1 != PlayerPrefs.GetInt("MagicMode1"))
        {
            ChangeMagic(PlayerPrefs.GetInt("MagicMode1"));
        }
        if(AttackMode!= PlayerPrefs.GetInt("AttackMode"))
        {
            changeAttack(PlayerPrefs.GetInt("AttackMode"));
        }
        
    }
    public void changeAttack(int num)
    {
        if (num == 1)
        {

            MaxComboInterval = 1.2f;
            MinComboInterval = 0.4f;
            AttButton.sprite = Hands;
            AttackMode = 1;
        }
        else if (num == 2)
        {
            MaxComboInterval = 0.6f;
            MinComboInterval = 0.2f;
            AttButton.sprite = Boxing;
            AttackMode = 2;
        }
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
                collision.GetComponent<EnemyFSM>().Hurt(PlayerInfo.GetInstance().Attack * PlayerInfo.GetInstance().SprintPower);
            }

        }
        else if (collision.CompareTag("BOSS"))
        {
            //AttIt.Play();
            if (insprint && !ani.GetBool("hurted"))
            {
                collision.GetComponent<BOSSFSM>().Hurt(PlayerInfo.GetInstance().Attack * PlayerInfo.GetInstance().SprintPower);
            }
                
        }
    }
}

