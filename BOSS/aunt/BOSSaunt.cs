using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSaunt : BOSSFSM
{
    private static float bulletSpeed;
    private static float attackMethod3BulletX = 0f;

    public List<Sprite> bulletSprites = new List<Sprite>();
    void Start()
    {
        states.Add(BossStateType.Idle, new IdleState(this));
        states.Add(BossStateType.Walk, new WalkState(this));
        states.Add(BossStateType.Chase, new ChaseState(this));
        states.Add(BossStateType.Attack, new AttackState(this));
        states.Add(BossStateType.Attack2, new AttackState2(this));
        states.Add(BossStateType.Attack3, new AttackState3(this));
        states.Add(BossStateType.Death, new DeathState(this));
        parameter.animator = GetComponent<Animator>();
        parameter.spriteRenderer = transform.GetComponent<SpriteRenderer>();
        parameter.rigidbody2D = GetComponent<Rigidbody2D>();
        bulletSpeed = 3f;
        Debug.Log(transform.parent.parent.name);
    }
    public void AttackMethod1()
    {
        parameter.attackAudio1.Play();
        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSaunt_bullet1", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.33f * transform.localScale.x, 0.33f, 0);
            o.GetComponent<BOSSaunt_bullet1>().Init(parameter.attack, new Vector2(transform.localScale.x * 0.41f, 0.41f), bulletSpeed, transform.localScale.x,parameter.target.position, transform.position.y - 1.47f);
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
    }
    public void AttackMethod2()
    {
        float bulletAttack = parameter.attack;
        Sprite sprite = bulletSprites[0];
        if(GetHpPercent() < 0.3f) { bulletAttack = parameter.attack * 2; sprite = bulletSprites[Random.Range(6,9)]; }
        else if (GetHpPercent() < 0.7f) { bulletAttack = parameter.attack * 1.5f; sprite = bulletSprites[Random.Range(3,6)]; }
        else { bulletAttack = parameter.attack; sprite = bulletSprites[Random.Range(0,3)]; }

        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSaunt_bullet2", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.3f * transform.localScale.x, 1f, 0);
            o.GetComponent<BOSSaunt_bullet2>().Init(bulletAttack, new Vector2(transform.localScale.x, 1), bulletSpeed, transform.localScale.x, parameter.target.position,sprite);
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
    }

    public void AttackMethod3()
    {
        Sprite sprite;
        attackMethod3BulletX += 10f;
        bulletSpeed += 3f;
        sprite = bulletSprites[Random.Range(6, 9)];
        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSaunt_bullet2", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.3f * transform.localScale.x, 1f, 0);
            o.GetComponent<BOSSaunt_bullet2>().Init(parameter.attack*2, new Vector2(transform.localScale.x, 1), bulletSpeed, transform.localScale.x, new Vector2((transform.position.x+ attackMethod3BulletX)*transform.localScale.x, -15f), sprite);
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
    }
    public class IdleState : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        private float idleTime = Random.Range(0.3f, 0.6f);
        private float timer = 0;
        public IdleState(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            attackMethod3BulletX = 0f;
            bulletSpeed = 3f;
            idleTime = Random.Range(0.3f, 0.6f);
            timer = 0;
            parameter.rigidbody2D.velocity = new Vector2(0, 0);
            parameter.animator.Play("Idle");
        }
        public void OnExit()
        {
        }
        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > idleTime)
            {
                if(manager.GetHpPercent() >= 0.3f) manager.TransitionState(BossStateType.Walk);
                else manager.TransitionState(BossStateType.Chase);
            }
        }
    }

    public class WalkState : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;
        private float moveTime = 1f;
        private float timer = 0;
        public WalkState(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            timer = 0;
            parameter.animator.Play("Walk");
            //manager.MoveRandom();
        }
        public void OnExit()
        {
            parameter.rigidbody2D.velocity = new Vector2(0, 0);
        }
        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > moveTime)
            {
                int attackMethod = Random.Range(1, 3);
                if (attackMethod == 1) manager.TransitionState(BossStateType.Attack);
                else manager.TransitionState(BossStateType.Attack2);
            }
            manager.FlipTo(parameter.target.position.x);
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                parameter.target.position, parameter.moveSpeed * Time.deltaTime);
        }
    }

    public class ChaseState : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        private float moveTime = 1f;
        private float timer = 0;
        public ChaseState(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            timer = 0;
            parameter.moveSpeed = parameter.moveSpeed * 1.5f;
            parameter.animator.Play("Walk");
        }
        public void OnExit()
        {
            parameter.moveSpeed = parameter.moveSpeed / 1.5f;
        }
        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > moveTime)
            {
                int attackMethod = Random.Range(1, 4);
                if (attackMethod == 1) manager.TransitionState(BossStateType.Attack);
                else if (attackMethod == 2) manager.TransitionState(BossStateType.Attack2);
                else manager.TransitionState(BossStateType.Attack3);
            }
            manager.FlipTo(parameter.target.position.x);
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                parameter.target.position, parameter.moveSpeed * Time.deltaTime);
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
            parameter.animator.Play("Attack1");
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
            parameter.attackAudio2.Play();
            parameter.animator.Play("Attack2");
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

        private AnimatorStateInfo info;
        public AttackState3(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.attackAudio3.Play();
            parameter.animator.Play("Attack3");
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