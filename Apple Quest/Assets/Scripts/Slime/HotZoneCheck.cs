using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private Slime_Behaviour enemyParent;
    private bool inRange;
    private Animator m_Anim;

    private void Awake()
    {
        enemyParent = GetComponentInParent<Slime_Behaviour>();
        m_Anim = GetComponentInParent<Animator>();
    }

    private void FixedUpdate()
    {
        if(inRange && !m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            enemyParent.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.inRange = inRange;
            enemyParent.SelectTarget();
        }
    }
}
