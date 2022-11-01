using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttack : MonoBehaviour
{
    private Animator m_Anim;

    private int attackDamage = 2;
    private float attackDelay = 0f;

    Knight t_knight;

    [SerializeField] private Transform attackArea;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackRate = 2f;

    [SerializeField] private LayerMask Monster;

    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        t_knight = gameObject.GetComponent<Knight>();
    }

    private void Update()
    {
        if (Time.time > attackDelay)
        {
            if ((Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonDown(0)) && t_knight.isGrounded())
            {
                Attack();
                attackDelay = Time.time + 1f / attackRate;
            }
        }
    }

    private void Attack()
    {
        // Animation
        m_Anim.SetTrigger("Attack");

        // Detect enemies in range
        Collider2D[] hitMonster = Physics2D.OverlapCircleAll(attackArea.position, attackRange, Monster);

        // Damage enemies
        foreach(Collider2D monster in hitMonster)
        {
            Debug.Log("Hit : " + monster.name);
            monster.GetComponent<Slime>().TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackArea == null)
            return;
        Gizmos.DrawWireSphere(attackArea.position, attackRange);
    }

}
