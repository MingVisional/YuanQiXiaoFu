using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garbage_black : EnemyFSM
{
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Death, new DeathState(this));
        parameter.animator = GetComponent<Animator>();
        parameter.spriteRenderer = transform.GetComponent<SpriteRenderer>();
        parameter.rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void AttackMethod()
    {
        if (parameter.target != null)
        {
            parameter.attackAudio.Play();
            FlipTo(parameter.target.position.x);
            PoolManager.GetInstance().GetObj("Prefab/Bullet/garbage_bullet", (o) =>
            {
                o.transform.position = transform.position;
                o.GetComponent<garbage_bullet>().Init(parameter.attack, transform.position, parameter.target.position);
                o.transform.SetParent(GameObject.Find("BulletManager").transform);
            });
        }
    }
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
                if (System.Math.Pow(parameter.target.transform.position.y - manager.transform.position.y,2)
                    + System.Math.Pow(parameter.target.transform.position.x - manager.transform.position.x, 2) < 25f)
                {
                    manager.TransitionState(StateType.Attack);
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
            parameter.animator.Play("Attack");
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