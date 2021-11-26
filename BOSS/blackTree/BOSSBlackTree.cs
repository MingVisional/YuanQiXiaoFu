using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSBlackTree : BOSSFSM
{
    public static GameObject attackArea;
    private float xMin = -10f;
    private float xMax = 10f;
    private float yMin = -13.6f;
    void Start()
    {
        states.Add(BossStateType.Idle, new IdleState(this));
        states.Add(BossStateType.Attack, new AttackState(this));
        states.Add(BossStateType.Attack2, new AttackState2(this));
        states.Add(BossStateType.Attack3, new AttackState3(this));
        states.Add(BossStateType.Attack4, new AttackState4(this));
        states.Add(BossStateType.Death, new DeathState(this));
        parameter.animator = GetComponent<Animator>();
        parameter.spriteRenderer = transform.GetComponent<SpriteRenderer>();
        parameter.rigidbody2D = GetComponent<Rigidbody2D>();

        attackArea = transform.Find("AttackArea").gameObject;
    }
    public void OpenAttackArea()
    {
        attackArea.SetActive(true);
    }
    public void CloseAttackArea()
    {
        attackArea.SetActive(false);
    }
    public void attackMethod2()
    {
        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSBlackTree_bullet", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.8f * transform.localScale.x, 1.8f, 0);
            o.GetComponent<BOSSBlackTree_bullet>().Init(parameter.attack, new Vector2(transform.localScale.x, 1), transform.localScale.x, parameter.target.position);
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
    }
    public void attackMethod4()
    {
        float x;
        float scale;
        x = Random.Range(transform.position.x+xMin, transform.position.x+xMax);
        if (x < transform.position.x) scale = -1;
        else scale = 1;
        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSBlackTree_bullet", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.8f * transform.localScale.x, 1.8f, 0);
            o.GetComponent<BOSSBlackTree_bullet>().Init(parameter.attack, new Vector2(scale, 1), transform.localScale.x, new Vector2(x,yMin));
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
        x = Random.Range(transform.position.x + xMin, transform.position.x + xMax);
        if (x < transform.position.x) scale = -1;
        else scale = 1;
        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSBlackTree_bullet", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.8f * transform.localScale.x, 1.8f, 0);
            o.GetComponent<BOSSBlackTree_bullet>().Init(parameter.attack, new Vector2(scale, 1), transform.localScale.x, new Vector2(x, yMin));
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
        x = Random.Range(transform.position.x + xMin, transform.position.x + xMax);
        if (x < transform.position.x) scale = -1;
        else scale = 1;
        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSBlackTree_bullet", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.8f * transform.localScale.x, 1.8f, 0);
            o.GetComponent<BOSSBlackTree_bullet>().Init(parameter.attack, new Vector2(scale, 1), transform.localScale.x, new Vector2(x, yMin));
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
        x = Random.Range(transform.position.x + xMin, transform.position.x + xMax);
        if (x < transform.position.x) scale = -1;
        else scale = 1;
        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSBlackTree_bullet", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.8f * transform.localScale.x, 1.8f, 0);
            o.GetComponent<BOSSBlackTree_bullet>().Init(parameter.attack, new Vector2(scale, 1), transform.localScale.x, new Vector2(x, yMin));
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
        x = Random.Range(transform.position.x + xMin, transform.position.x + xMax);
        if (x < transform.position.x) scale = -1;
        else scale = 1;
        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSBlackTree_bullet", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.8f * transform.localScale.x, 1.8f, 0);
            o.GetComponent<BOSSBlackTree_bullet>().Init(parameter.attack, new Vector2(scale, 1), transform.localScale.x, new Vector2(x, yMin));
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HorizontalControl>().Hurt(parameter.attack, transform.localScale.x);
        }
    }
    public class IdleState : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        public IdleState(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.rigidbody2D.velocity = new Vector2(0, 0);
            parameter.animator.Play("Idle");
        }
        public void OnExit()
        {
        }
        public void OnUpdate()
        {
            float dis = System.Math.Abs(parameter.target.position.x - manager.transform.position.x);
            manager.FlipTo(parameter.target.position.x);

            if (dis > 2.5f) parameter.currentAttackTime += Time.deltaTime;

            if(parameter.currentAttackTime > parameter.attackTime)
            {
                
                int attackMethod;
                if (manager.GetHpPercent() >= 0.5f)
                {
                    
                    if(dis <= 2.5f)
                    {
                        attackMethod = Random.Range(1, 3);
                        if (attackMethod == 1) manager.TransitionState(BossStateType.Attack);
                        else manager.TransitionState(BossStateType.Attack2);
                    }
                    else
                    {
                        manager.TransitionState(BossStateType.Attack2);
                    }
                }
                else
                {
                    if (dis <= 2.5f)
                    {
                        attackMethod = Random.Range(1, 5);
                        if (attackMethod == 1) manager.TransitionState(BossStateType.Attack);
                        else if (attackMethod == 2) manager.TransitionState(BossStateType.Attack2);
                        else if (attackMethod == 3) manager.TransitionState(BossStateType.Attack3);
                        else manager.TransitionState(BossStateType.Attack4);
                    }
                    else
                    {
                        attackMethod = Random.Range(1, 3);
                        if (attackMethod == 1) manager.TransitionState(BossStateType.Attack2);
                        else manager.TransitionState(BossStateType.Attack4);
                    }
                }
            }
        }
    }

    public class AttackState : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        private AnimatorStateInfo info;
        public AttackState(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.attackAudio1.Play();
            parameter.animator.Play("Attack1");
            parameter.currentAttackTime = 0f;
        }
        public void OnExit()
        {
            attackArea.SetActive(false);
        }
        public void OnUpdate()
        {
            info = parameter.animator.GetCurrentAnimatorStateInfo(0);

            if (info.normalizedTime >= 0.95f)
            {
                manager.TransitionState(BossStateType.Idle);
            }
        }
    }

    public class AttackState2 : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        private AnimatorStateInfo info;
        public AttackState2(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.attackAudio1.Play();
            parameter.animator.Play("Attack4");
            parameter.currentAttackTime = 0f;
        }
        public void OnExit()
        {
        }
        public void OnUpdate()
        {
            info = parameter.animator.GetCurrentAnimatorStateInfo(0);

            if (info.normalizedTime >= 0.95f)
            {
                manager.TransitionState(BossStateType.Idle);
            }
        }
    }

    public class AttackState3 : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        //private AnimatorStateInfo info;
        private float timer;
        private float attackTime = 1f;
        public AttackState3(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.attackAudio1.Play();
            timer = 0f;
            parameter.animator.Play("Attack2");
            parameter.currentAttackTime = 0f;
        }
        public void OnExit()
        {
            attackArea.SetActive(false);
        }
        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > attackTime)
            {
                manager.TransitionState(BossStateType.Idle);
            }
        }
    }

    public class AttackState4 : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        private float timer;
        private float attackTime = 1.5f;
        public AttackState4(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.attackAudio1.Play();
            timer = 0;
            parameter.animator.Play("Attack3");
            parameter.currentAttackTime = 0f;
        }
        public void OnExit()
        {
            attackArea.SetActive(false);
        }
        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > attackTime) manager.TransitionState(BossStateType.Idle);
        }
    }

    public class DeathState : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        private AnimatorStateInfo info;
        public DeathState(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.animator.Play("Death");
        }
        public void OnExit()
        {
        }
        public void OnUpdate()
        {
            /*info = parameter.animator.GetCurrentAnimatorStateInfo(0);

            if (info.normalizedTime >= 0.95f)
            {
                manager.Death();
            }*/
        }
    }
}