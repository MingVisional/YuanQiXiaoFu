using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_black : EnemyFSM
{
    private float bulletSpeed;
    private float attackAreaR = 0.8f;
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Attack2, new AttackState2(this));
        states.Add(StateType.Death, new DeathState(this));
        parameter.animator = GetComponent<Animator>();
        parameter.spriteRenderer = transform.GetComponent<SpriteRenderer>();
        parameter.rigidbody2D = GetComponent<Rigidbody2D>();

        bulletSpeed = 3f;
        attackAreaR = 0.8f;
    }
    public void AttackMethod1()
    {
        parameter.attackAudio.Play();
        //Debug.Log("攻击");
        if (Physics2D.OverlapCircle(transform.position + new Vector3(transform.localScale.x * 0.8f, -0.3f, 0), attackAreaR, parameter.targetLayer)
            || Physics2D.OverlapCircle(transform.position + new Vector3(0, -0.3f, 0), attackAreaR, parameter.targetLayer))
        {
            DoHurt(parameter.attack, transform.localScale.x);
        }
    }
    public void AttackMethod2()
    {
        parameter.attackAudio.Play();
        PoolManager.GetInstance().GetObj("Prefab/Bullet/clock_black_bullet", (o) =>
        {
            //o.transform.position = transform.position + new Vector3(1.81f * transform.localScale.x, 0, 0);
            o.transform.position = transform.position;
            o.GetComponent<clock_black_bullet>().Init(parameter.attack, new Vector2(transform.localScale.x, 1), bulletSpeed, transform.localScale.x);
            o.transform.SetParent(GameObject.Find("BulletManager").transform);
        });
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(transform.localScale.x*0.8f,-0.3f,0), attackAreaR);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, -0.3f, 0), attackAreaR);
    }*/
    public class IdleState : StateBase
    {
        private EnemyFSM manager;
        private Parameter parameter;

        private float idleTime = Random.Range(0.3f, 0.6f);
        private float timer = 0;
        public IdleState(EnemyFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
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
                manager.TransitionState(StateType.Walk);
            }
        }
    }

    public class WalkState : StateBase
    {
        private EnemyFSM manager;
        private Parameter parameter;
        private float moveTime = 1f;
        private float timer = 0;
        public WalkState(EnemyFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            timer = 0;
            parameter.animator.Play("Walk");
            manager.MoveRandom();
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
                manager.TransitionState(StateType.Idle);
            }
            if (parameter.target != null) manager.TransitionState(StateType.Chase);
        }
    }

    public class ChaseState : StateBase
    {
        private EnemyFSM manager;
        private Parameter parameter;
        public ChaseState(EnemyFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.animator.Play("Walk");
        }
        public void OnExit()
        {

        }
        public void OnUpdate()
        {
            if (parameter.target != null)
            {
                manager.FlipTo(parameter.target.position.x);
                manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                    parameter.target.position, parameter.moveSpeed * Time.deltaTime);
            }
            else manager.TransitionState(StateType.Idle);//跟丢

            if (parameter.target != null && parameter.currentAttackTime >= parameter.attackTime)
            {
                if (System.Math.Abs(parameter.target.transform.position.y - manager.transform.position.y) < 0.7f)
                {
                    if (Physics2D.OverlapCircle(manager.transform.position + new Vector3(manager.transform.localScale.x * 0.8f, -0.3f, 0), 0.8f, parameter.targetLayer)
                        || Physics2D.OverlapCircle(manager.transform.position + new Vector3(0, -0.3f, 0), 0.8f, parameter.targetLayer))
                    {
                        manager.TransitionState(StateType.Attack);
                    }
                    else  manager.TransitionState(StateType.Attack2);
                }
            }
        }
    }

    public class AttackState : StateBase
    {
        private EnemyFSM manager;
        private Parameter parameter;

        private AnimatorStateInfo info;
        public AttackState(EnemyFSM manager)
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
                manager.TransitionState(StateType.Chase);
            }
        }
    }

    public class AttackState2 : StateBase
    {
        private EnemyFSM manager;
        private Parameter parameter;

        private AnimatorStateInfo info;
        public AttackState2(EnemyFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
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
                manager.TransitionState(StateType.Chase);
            }
        }
    }
    public class DeathState : StateBase
    {
        private EnemyFSM manager;
        private Parameter parameter;

        private AnimatorStateInfo info;
        public DeathState(EnemyFSM manager)
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