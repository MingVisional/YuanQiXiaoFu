using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class badminton_white : EnemyFSM
{
    private static float attackAreaR = 1f;//攻击半径
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Death, new DeathState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        parameter.animator = GetComponent<Animator>();
        parameter.spriteRenderer = transform.GetComponent<SpriteRenderer>();
        parameter.rigidbody2D = GetComponent<Rigidbody2D>();
        attackAreaR = 0.5f;
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackAreaR);
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
            if (parameter.target != null && Vector3.Distance(parameter.target.position, manager.transform.position) > 2f)
            {
                manager.FlipTo(parameter.target.position.x);
                manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                    parameter.target.position, parameter.moveSpeed * Time.deltaTime);
            }
            else manager.TransitionState(StateType.Idle);//跟丢

            //攻击间隔时间到了，进行攻击
            if (parameter.target != null && parameter.currentAttackTime >= parameter.attackTime)
            {
                manager.TransitionState(StateType.Attack);
            }
        }
    }
    public class AttackState : StateBase
    {
        private EnemyFSM manager;
        private Parameter parameter;

        private float attackTime = 2f;
        private float timer = 0f;
        private Vector3 targetPosition;

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
            parameter.animator.Play("Attack");
            parameter.currentAttackTime = 0f;

            if (parameter.target != null) targetPosition = parameter.target.position;
            else targetPosition = manager.transform.position + new Vector3(manager.transform.localScale.x * parameter.moveSpeed * 2f,0,0);

            parameter.moveSpeed = parameter.moveSpeed * 3f;
        }
        public void OnExit()
        {
            parameter.moveSpeed = parameter.moveSpeed / 3f;
        }
        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > attackTime)
            {
                manager.TransitionState(StateType.Idle);
            }
            //Debug.Log(Vector3.Distance(manager.transform.position, targetPosition));
            if (Vector3.Distance(manager.transform.position, targetPosition) < 0.5f)
            {
                manager.TransitionState(StateType.Chase);
            }

            manager.FlipTo(targetPosition.x);
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                targetPosition, parameter.moveSpeed * Time.deltaTime);

            if (Physics2D.OverlapCircle(manager.transform.position, attackAreaR, parameter.targetLayer))
            {
                manager.DoHurt(parameter.attack, manager.transform.localScale.x);
                manager.TransitionState(StateType.Chase);
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
            parameter.animator.Play("Death");
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
