using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knightcontrol : MonoBehaviour
{
    public Joystick joy;
    public Rigidbody2D rig;
    public bool issprint = true, isstop = true;
    public float movespeed = 5f, jumpforce = 100f;
    public Transform groundchack;
    public Animator ami;
    public LayerMask map;
    public GameObject att, magic3;
    private KeyCode currentKey;
    private bool uijump, uiw, uis, uij, uik;
    public int jumpcount;
    public float joyhor, joyver;
    private bool isground = true, isatt;
    float cansprint = 0, nextsprint = 2f, canattack = 0, nextattack = 0.5f, canbullet = 0, nextbullet = 2f;
    void Start()
    {
        Transform transform1 = transform.GetChild(0);
        att = transform1.gameObject;
        groundchack = transform.GetChild(1);
        magic3 = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        move();
        amicontrol();
        joyhor = joy.Horizontal;
        joyver = joy.Vertical;
        if (ami.GetBool("magic2") || ami.GetBool("magic1"))
        {
            rig.velocity = new Vector2(0, 0);
        }
        if (!issprint && isground)
        {
            issprint = true;
        }
    }
    void FixedUpdate()
    {

        isground = Physics2D.OverlapCircle(groundchack.position, 0.1f, map);
        if (isground)
        {
            jumpcount = 1;
        }
    }
    void OnGUI()
    {
        if (Input.anyKeyDown)
        {
            Event e = Event.current;
            if (e.isKey)

            {
                currentKey = e.keyCode;
            }

        }
    }
    void move()
    {
        if (isstop)
        {

            if (Input.GetKey(KeyCode.D) || joyhor > 0.4)
            {
                rig.velocity = new Vector2(movespeed, rig.velocity.y);
                transform.localScale = new Vector2(-1f, 1f);
            }
            else if (Input.GetKey(KeyCode.A) || joyhor < -0.4)
            {
                rig.velocity = new Vector2(-movespeed, rig.velocity.y);
                transform.localScale = new Vector2(1f, 1f);
            }
            else
            {
                rig.velocity = new Vector2(0, rig.velocity.y);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump();
            }

        }
        if (Input.GetKey(KeyCode.L))
        {
            sprint();
        }
        if (!isatt)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                attack();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                magic();

            }

        }

    }
    public void jump()
    {
        if (jumpcount > 0)
        {
            //Vector2 fi = new Vector2(0, jumpforce);
            //rig.AddForce(fi);
            ami.SetBool("isjump1", true);

            rig.velocity = new Vector2(rig.velocity.x, 5f);
            jumpcount -= 1;
        }
    }
    public void sprint()
    {
        if (cansprint < Time.time && issprint)
        {
            ami.SetBool("sprint", true);
            ami.Play("sprint", 0, 0);
            rig.velocity = new Vector2(-transform.localScale.x * movespeed * 5, 0);
            cansprint = nextsprint + Time.time;
            issprint = false;
            isstop = false;
            Invoke("fixspeed", 0.2f);
        }
    }
    public void attack()
    {
        if (canattack < Time.time)
        {
            canattack = nextattack + Time.time;
            ami.SetBool("attack", true);
            ami.Play("attack", 0, 0);
            isatt = true;
            att.gameObject.GetComponent<doattack>().getattack();
            Invoke("fixattack", 0.5f);
        }
    }
    public void magic()
    {
        if (Time.time > canbullet)
        {
            if ((Input.GetKey(KeyCode.W)) || (joyver > 0.5))
            {
                canbullet = Time.time + nextbullet;
                ami.SetBool("magic2", true);
                ami.Play("magic2");
                Invoke("fixattack", 0.3f);
                isatt = true;
                isstop = false;
                Invoke("fixspeed", 0.3f);
                PoolManager.GetInstance().GetObj("prefabs/magic2", (o) =>
                {
                    o.transform.position = transform.position + new Vector3(0, 2f, 0);
                    o.transform.localScale = new Vector2(-transform.localScale.x, 1f);
                    o.transform.SetParent(this.transform.parent);
                    o.GetComponent<magic2>().Init();
                });
            }
            else if ((Input.GetKey(KeyCode.S)) || (joyver < -0.5))
            {
                isatt = true;
                isstop = false;
                rig.velocity = new Vector2(0, -movespeed * 5);
                ami.SetBool("magic3-1", true);

            }
            else
            {
                canbullet = Time.time + nextbullet;
                ami.SetBool("magic1", true);
                ami.Play("magic1");
                Invoke("fixattack", 0.3f);
                isatt = true;
                PoolManager.GetInstance().GetObj("prefabs/magic1", (o) =>
                {
                    o.transform.position = transform.position;
                    o.transform.localScale = new Vector2(-transform.localScale.x, 1f);
                    o.transform.SetParent(this.transform.parent);
                    o.GetComponent<bulletn>().Init();
                });
            }

        }
    }
    void fixspeed()
    {
        ami.SetBool("sprint", false);
        rig.velocity = new Vector2(0, 0);
        isstop = true;

    }
    void fixattack()
    {
        ami.SetBool("magic1", false);
        ami.SetBool("magic2", false);
        ami.SetBool("attack", false);
        isatt = false;
    }
    void fixmagic3()
    {
        magic3.SetActive(false);
        ami.SetBool("magic3-2", false);
        isstop = true;
        isatt = false;
    }
    void amicontrol()
    {
        if (ami.GetBool("isjump1") && rig.velocity.y <= 0)
        {
            ami.SetBool("isjump2", true);
            ami.SetBool("isjump1", false);
        }
        if (!isground && rig.velocity.y < 0)
        {
            ami.SetBool("isjump2", true);
            ami.SetBool("isjump1", false);
            ami.SetBool("ismove", false);
        }
        if (ami.GetBool("isjump2") && rig.velocity.y >= 0)
        {

            ami.SetBool("isjump2", false);
        }
        if (isground)
        {
            if (System.Math.Abs(rig.velocity.x) > 1)
            {
                ami.SetBool("ismove", true);
            }
            else
            {
                ami.SetBool("ismove", false);
            }
        }
        if (isground && ami.GetBool("magic3-1"))
        {
            ami.SetBool("magic3-2", true);
            ami.Play("magic3-2");
            magic3.SetActive(true);
            rig.velocity = new Vector2(0, 0);
            ami.SetBool("magic3-1", false);
            Invoke("fixmagic3", 0.3f);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "map")
        {
            ami.SetBool("isjump2", false);
            ami.SetBool("isjump1", false);
            issprint = true;
        }
    }
}
