using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Slime_Behaviour : MonoBehaviour
{
    public GameObject MagicSpell;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform leftLimit;
    public Transform rightLimit;
    public Transform target;
    public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;
    public float spellCooldown;

    private Animator m_Anim;
    private Slime slime;

    private float distance;
    private bool attackMode;
    private bool cooling;
    private float intTimer;
    private float lastSpellShot;

    void Awake()
    {
        SelectTarget();
        intTimer = timer;
        m_Anim = GetComponent<Animator>();
        slime = GetComponent<Slime>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if (!InsideOfLimits() && !inRange && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            SelectTarget();
        }
        
        if(inRange)
        {
            SlimeLogic();
        }
    }

    void SlimeLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);
        if(distance > attackDistance)
        {
            StopAttack();
        }
        else if(attackDistance >= distance && cooling == false && !slime.isDead)
        {
            Attack();
        }
        if (cooling)
        {
            Cooldown();
            m_Anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        m_Anim.SetBool("canWalk", true);
        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = intTimer;
        attackMode = true;

        m_Anim.SetBool("canWalk", false);
        m_Anim.SetBool("Attack", true);

        if (Time.time - lastSpellShot < spellCooldown)
            return;
        Instantiate(MagicSpell, transform.position, Quaternion.identity);
        lastSpellShot = Time.time;
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        m_Anim.SetBool("Attack", false);
    }
    public void TriggerCooling()
    {
        cooling = true;
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    private bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        }
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }
}

