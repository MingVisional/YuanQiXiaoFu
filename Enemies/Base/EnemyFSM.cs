﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 所有敌人继承
/// </summary>

//状态种类
public enum StateType { Idle,Walk,Chase,Attack,Attack2, Death }

//敌人信息
[Serializable]
public class Parameter
{
    public string name;
    public float maxHealth;//最大生命值
    public float health = 1f;//当前生命值
    public float moveSpeed;//移动速度
    public float moveSpeedBasic;//基础移动速度
    public Transform target;//追击目标
    public LayerMask targetLayer;//玩家Layer
    public float attack;//攻击力
    public float attackBasic;//基础攻击力
    public bool create = true;//是否刚创建

    //部分敌人用到的：
    public float attackTime;//多久攻击一次
    public float currentAttackTime = 0f;

    public Animator animator;
    public Rigidbody2D rigidbody2D;
    public SpriteRenderer spriteRenderer;

    public AudioSource attackAudio;
}
public class EnemyFSM : MonoBehaviour
{
    public Parameter parameter;//敌人信息
    public StateType currentStateType;//当前状态
    protected StateBase currentState;//当前状态对应的class
    protected Dictionary<StateType, StateBase> states = new Dictionary<StateType, StateBase>();//存储所有状态和对应的class

    public void Init()
    {//初始化
        TransitionState(StateType.Idle);//生成时状态为站立
        parameter.health = parameter.maxHealth*PlayerPrefs.GetFloat("EnemyHpRate");//生成时体力回满
        parameter.attack = parameter.attackBasic*PlayerPrefs.GetFloat("EnemyAttRate");
        parameter.moveSpeed = parameter.moveSpeedBasic;
    }
    void Update()
    {
        if (parameter.create)//如果这个敌人刚生成，要先初始化，放在Update里是为了用于从缓存池拿出来的敌人
        {
            parameter.create = false;
            StartCoroutine(ColorEF(0.05f, new Color(1f, 1f, 1f), 0.05f, null));
            Init();
        }
        parameter.spriteRenderer.sortingOrder = (int)(-transform.position.y * 10);
        transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y, 1);
        parameter.currentAttackTime += Time.deltaTime;
        currentState.OnUpdate();//执行当前状态的Update函数
    }
    //切换状态
    public void TransitionState(StateType type)
    {
        if (currentState != null) currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
        currentStateType = type;
    }
    //让敌人朝向目标target的横坐标x
    public void FlipTo(float x)
    {
        if (transform.position.x > x)transform.localScale = new Vector3(-1, 1, 1);
        else if (transform.position.x < x)transform.localScale = new Vector3(1, 1, 1);
    }
    //随机移动
    public void MoveRandom()
    {
        Vector2 targetPosition = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        //Debug.Log(targetPosition.x);
        if (targetPosition.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);
        GetComponent<Rigidbody2D>().velocity = parameter.moveSpeed * targetPosition;
    }
    /*碰撞
     * 碰墙后立即移动
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.CompareTag("Wall"))
        {
            TransitionState(StateType.Walk);
        }
    }
    //被武器打到
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.CompareTag("Weapon"))
        {
            //Hurt(PlayerInfo.GetInstance().Attack);
        }
    }
    //进入攻击范围锁定目标
    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = other.transform;
        }
    }
    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = null;
        }

    }

    public void Death()
    {
        GameObject.Find("TotalControl").GetComponent<TotalControl>().KillCount += 1;
        PlayerInfo.GetInstance().Gold += 10f;
        Destroy(transform.parent.gameObject);
        parameter.create = true;//先设置成true，以后从缓存池里取出来可以初始化
        //PoolManager.GetInstance().PushObj("Prefab/Enemy/" + parameter.name, transform.parent.gameObject);
    }

    public void Hurt(float damage)
    {
        if(parameter.health > 0)
        {
            StartCoroutine(ColorEF(0.2f, new Color(0.6f, 0.6f, 0.6f), 0.05f, null));
            parameter.health -= damage;
            if (parameter.health <= 0)
            {
                TransitionState(StateType.Death);
            }
        }
    }

    //对玩家造成伤害
    public void DoHurt(float hurtnum,float toward)
    {
        GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerControl>().Hurt(hurtnum, toward);
    }


    public IEnumerator ColorEF(float wantTime, Color targetColor, float delayTime, UnityAction fun)
    /*
     * wantTime闪烁总时长，targetColor 闪烁至目标颜色，delayTime 改变颜色间隔，fun闪烁后执行一个方法
     * 调用方法例如MonoManager.GetInstance().StartCoroutine(ColorEF(0.2f, new Color(0.4f, 0.4f, 0.4f), 0.05f, null));
     * 可以怪物受伤的时候让他闪一下，或者发动什么技能的时候闪一下
     */
    {
        float currTime = 0;
        float lerp;
        while (currTime < wantTime)
        {
            yield return new WaitForSeconds(delayTime);
            lerp = currTime / wantTime;
            currTime += delayTime;
            parameter.spriteRenderer.color = Color.Lerp(Color.white, targetColor, lerp);
        }
        parameter.spriteRenderer.color = Color.white;
        if (fun != null) fun();
    }
}