using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSRoommate : BOSSFSM
{
    public static GameObject attackArea;
    public static GameObject music1;
    public static GameObject music2;
    public List<Sprite> bulletSprites = new List<Sprite>();

    public float bulletSpeed;
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
        attackArea = transform.Find("AttackArea").gameObject;
        music1 = transform.Find("music1").gameObject;
        music2 = transform.Find("music2").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(GetHpPercent()>0.5f) collision.GetComponent<HorizontalControl>().Hurt(parameter.attack, transform.localScale.x);
            else collision.GetComponent<HorizontalControl>().Hurt(parameter.attack * 1.5f, transform.localScale.x);
        }
    }

    public void AttackMethod2()
    {
        parameter.attackAudio2.Play();
        FlipTo(parameter.target.position.x);
        if(GetHpPercent()>0.5f)
            PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSRoommate_bullet1", (o) =>
            {
                o.transform.position = transform.position + new Vector3(0.5f * transform.localScale.x, 0.85f, 0);
                o.GetComponent<BOSSRoommate_bullet1>().Init(parameter.attack, new Vector2(transform.localScale.x * 1f, 1f), bulletSpeed, transform.localScale.x, parameter.target.position,bulletSprites[0]);
                o.transform.SetParent(GameObject.Find("BulletManager").transform);
            });
        else
            PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSRoommate_bullet1", (o) =>
            {
                o.transform.position = transform.position + new Vector3(0.5f * transform.localScale.x, 0.85f, 0);
                o.GetComponent<BOSSRoommate_bullet1>().Init(parameter.attack * 1.3f, new Vector2(transform.localScale.x * 1f, 1f), bulletSpeed, transform.localScale.x, parameter.target.position, bulletSprites[1]);
                o.transform.SetParent(GameObject.Find("BulletManager").transform);
            });
    }
    public void AttackMethod3()
    {
        parameter.attackAudio3.Play();
        FlipTo(parameter.target.position.x);
        PoolManager.GetInstance().GetObj("Prefab/Bullet/BOSSRoommate_bullet2", (o) =>
        {
            o.transform.position = transform.position + new Vector3(0.66f * transform.localScale.x, 0.09f, 0);
            o.GetComponent<BOSSRoommate_bullet2>().Init(parameter.attack * 1.3f, new Vector2(transform.localScale.x * 1f, 1f), bulletSpeed * 5, transform.localScale.x, parameter.target.position);
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

        private float timer;
        private float attackTime = 1.5f;
        public AttackState(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
            parameter.attackAudio1.Play();
            if (manager.GetHpPercent() > 0.5f) music1.SetActive(true);
            else music2.SetActive(true);
            attackArea.SetActive(true);
            manager.FlipTo(parameter.target.position.x);
            timer = 0f;
            parameter.animator.Play("Attack1");
            parameter.currentAttackTime = 0f;
        }
        public void OnExit()
        {
            music1.SetActive(false);
            music2.SetActive(false);
            attackArea.SetActive(false);
        }
        public void OnUpdate()
        {
            timer += Time.deltaTime;
            if (timer > attackTime) manager.TransitionState(BossStateType.Idle);
            
            if(manager.GetHpPercent()>0.5f)
                manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                    manager.transform.position + new Vector3(manager.transform.localScale.x, 0, 0), parameter.moveSpeed * 2 * Time.deltaTime);
            else
                manager.transform.position = Vector2.MoveTowards(manager.transform.position,
                    manager.transform.position + new Vector3(manager.transform.localScale.x, 0, 0), parameter.moveSpeed * 3 * Time.deltaTime);
        }
    }

    public class AttackState2 : StateBase
    {
        private BOSSFSM manager;
        private BossParameter parameter;

        private float timer = 0f;
        private float attackTime = 2f;
        public AttackState2(BOSSFSM manager)
        {
            this.manager = manager;
            this.parameter = manager.parameter;
        }
        public void OnEnter()
        {
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
            attackArea.SetActive(true);
            music2.SetActive(true);
            timer = 0f;
            parameter.animator.Play("Attack3");
            parameter.currentAttackTime = 0f;
        }
        public void OnExit()
        {
            attackArea.SetActive(false);
            music2.SetActive(false);
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