using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSMuscleMan : BOSSFSM
{
    public static GameObject attackArea;
    void Start()
    {
        states.Add(BossStateType.Idle, new IdleState(this));
        states.Add(BossStateType.Walk, new WalkState(this));
        states.Add(BossStateType.Chase, new ChaseState(this));
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

        private float idleTime = Random.Range(0.3f, 0.6f);
        private float timer = 0;
        public IdleState(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
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
                if (manager.GetHpPercent() >= 0.5f) manager.TransitionState(BossStateType.Walk);
                else manager.TransitionState(BossStateType.Chase);
            }
        }
    }

    public class WalkState : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;
        private float moveTime = 1.5f;
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
        }
        public void OnExit()
        {
            parameter.rigidbody2D.velocity = new Vector2(0, 0);
        }
        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > moveTime) manager.TransitionState(BossStateType.Idle);
            float dis = System.Math.Abs(parameter.target.position.x - manager.transform.position.x);
            if (dis < 1f)
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

        private float moveTime = 1.5f;
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
            if (timer > moveTime) manager.TransitionState(BossStateType.Idle);
            float dis = System.Math.Abs(parameter.target.position.x - manager.transform.position.x);
            if (dis < 1f)
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
            parameter.attackAudio2.Play();
            parameter.animator.Play("Attack2");
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

    public class AttackState3 : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        //private AnimatorStateInfo info;
        private float timer;
        private float attackTime = 2f;
        public AttackState3(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.attackAudio3.Play();
            timer = 0f;
            parameter.animator.Play("Attack3");
            parameter.currentAttackTime = 0f;
        }
        public void OnExit()
        {
            attackArea.SetActive(false);
        }
        public void OnUpdate()
        {
            /*info = parameter.animator.GetCurrentAnimatorStateInfo(0);

            if (info.normalizedTime >= 0.95f)
            {
                manager.TransitionState(BossStateType.Idle);
            }*/
            timer += Time.deltaTime;
            if(timer > attackTime)
            {
                manager.TransitionState(BossStateType.Attack4);
            }
        }
    }

    public class AttackState4 : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        private float timer;
        private float attackTime = 2.4f;
        public AttackState4(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.attackAudio4.Play();
            attackArea.SetActive(true);
            manager.FlipTo(parameter.target.position.x);
            timer = 0f;
            parameter.animator.Play("Attack4");
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
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                manager.transform.position + new Vector3(manager.transform.localScale.x,0,0), parameter.moveSpeed * 3 * Time.deltaTime);
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