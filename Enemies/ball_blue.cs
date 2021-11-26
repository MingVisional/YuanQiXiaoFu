using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_blue : EnemyFSM
{
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Death, new DeathState(this));
        parameter.animator = GetComponent<Animator>();
        parameter.spriteRenderer = transform.GetComponent<SpriteRenderer>();
        parameter.rigidbody2D = GetComponent<Rigidbody2D>();
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
            idleTime = Random.Range(0.5f, 1f);
            timer = 0;
            parameter.rigidbody2D.velocity = new Vector2(0, 0);
            parameter.animator.Play("Walk");
        }
        public void OnExit()
        {
        }
        public void OnUpdate()
        {
            manager.transform.Rotate(new Vector3(0, 0, -500f * Time.deltaTime));
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
            manager.transform.Rotate(new Vector3(0, 0, -500f * Time.deltaTime));
            timer += Time.deltaTime;
            if (timer > moveTime)
            {
                manager.TransitionState(StateType.Idle);
            }
            if (parameter.target != null && parameter.currentAttackTime >= parameter.attackTime) 
                manager.TransitionState(StateType.Attack);
        }
    }
    public class AttackState : StateBase
    {
        private EnemyFSM manager;
        private Parameter parameter;

        private float attackTime = 2f;
        private float timer = 0f;
        private AnimatorStateInfo info;
        public AttackState(EnemyFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            timer = 0f;
            parameter.attackAudio.Play();
            parameter.animator.Play("Walk");
            parameter.currentAttackTime = 0f;
            parameter.moveSpeed = parameter.moveSpeed * 1.6f;
        }
        public void OnExit()
        {
            parameter.moveSpeed = parameter.moveSpeed / 1.6f;
        }
        public void OnUpdate()
        {
            manager.transform.Rotate(new Vector3(0, 0, -1000f * Time.deltaTime));
            timer += Time.deltaTime;
            if (timer > attackTime)
            {
                manager.TransitionState(StateType.Idle);
            }
            if (parameter.target != null)
            {
                manager.FlipTo(parameter.target.position.x);
                manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                    parameter.target.position, parameter.moveSpeed * Time.deltaTime);
            }
            else manager.TransitionState(StateType.Idle);//跟丢
            if (Physics2D.OverlapCircle(manager.transform.position, 0.66f, parameter.targetLayer))
            {
                manager.DoHurt(parameter.attack, manager.transform.localScale.x);
                manager.TransitionState(StateType.Idle);
            }
        }
    }

    public class DeathState : StateBase
    {
        private float deathTime = 0.5f;
        private float timer = 0f;
        private EnemyFSM manager;
        private Parameter parameter;
        public DeathState(EnemyFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            timer = 0f;
            manager.StartCoroutine(manager.ColorEF(deathTime, new Color(0.01f, 0.01f, 0.01f), 0.05f, null));
        }
        public void OnExit()
        {
        }
        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > deathTime)
            {
                manager.Death();
            }
        }
    }
}
