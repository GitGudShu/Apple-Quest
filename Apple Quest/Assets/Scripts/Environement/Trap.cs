using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private float m_LastHit;
    private float m_ImmunityDelay = 2.0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Knight t_Knight = collision.gameObject.GetComponent<Knight>();

        if (t_Knight && m_LastHit + m_ImmunityDelay <= Time.time && !t_Knight.isImmune) // si t_Player est null
        {
            Debug.Log("Player hit!");
            t_Knight.TakeDamage(1);
            m_LastHit = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Knight t_Knight = collision.gameObject.GetComponent<Knight>();

        if (t_Knight && m_LastHit + m_ImmunityDelay <= Time.time && !t_Knight.isImmune) // si t_Player est null
        {
            Debug.Log("Player hit!");
            t_Knight.TakeDamage(1);
            m_LastHit = Time.time;
        }
    }
}
