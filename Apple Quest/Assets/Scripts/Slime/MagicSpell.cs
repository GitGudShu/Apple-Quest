using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpell : MonoBehaviour
{
    public float force;
    public GameObject target;
    public GameObject spell;

    private float m_LastHit;
    private float m_ImmunityDelay = 2f;

    Rigidbody2D m_RB;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        m_RB = GetComponent<Rigidbody2D>();

        Vector3 direction = target.transform.position - transform.position;
        m_RB.velocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Knight t_Knight = null;

        if(other.name == "Knight")
            t_Knight = other.gameObject.GetComponent<Knight>();

        if (other.gameObject.CompareTag("Deflect"))
        {
            Destroy(this.gameObject, 0.02f);
        }

        if (!other.gameObject.CompareTag("Monster")
            && !other.gameObject.CompareTag("IgnoreCast")
            && !other.gameObject.CompareTag("Apple")
            && !other.gameObject.CompareTag("Potion")
            && !other.gameObject.CompareTag("Room"))
        {
            Debug.Log(other.name);
            if (t_Knight && m_LastHit + m_ImmunityDelay <= Time.time) // si t_Robot est null
            {
                Debug.Log("Player Kaboomed");
                t_Knight.TakeDamage(1);
                m_LastHit = Time.time;
            }
            GameObject newSpell = Instantiate(spell, transform.position, Quaternion.identity);
            Destroy(newSpell, 0.5f);
            Destroy(this.gameObject, 0.02f);
        }
    }
}
