using System.Collections;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public GameObject Potion;

    private Animator m_Anim;
    private SpriteRenderer m_SpriteRenderer;

    public int maxHealth = 4;
    private int currentHealth;

    public bool isDead = false;

    private float m_LastHit;
    private float m_ImmunityDelay = 1.0f;

    private Coroutine m_FlashCR;

    private void Start()
    {
        currentHealth = maxHealth;
        m_Anim = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Knight t_Knight = collision.gameObject.GetComponent<Knight>();

        if (t_Knight && m_LastHit + m_ImmunityDelay <= Time.time && !t_Knight.isImmune && !isDead ) // si t_Player est null
        {
            Debug.Log("Player hit!");
            t_Knight.TakeDamage(1);
            m_LastHit = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {
        if(m_LastHit + m_ImmunityDelay <= Time.time && !isDead)
        {
            m_LastHit = Time.time;
            currentHealth -= damage;

            m_Anim.SetTrigger("Hurt");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        // Visual flash
        if (m_FlashCR != null)
            StopCoroutine(m_FlashCR);
        m_FlashCR = StartCoroutine(CR_Flash());
    }

    void Die()
    {
        Debug.Log(name + " slayed");
        isDead = true;
        Instantiate(Potion, transform.position, Quaternion.identity);
        m_Anim.SetBool("Dead", true);
        Destroy(gameObject, 1f);
        if(gameObject.name == "SlimeKing")
        {
            StartCoroutine(WaitBeforeWin());
            Debug.Log("Gagne la partie");
            Knight t_Knight = GameObject.FindGameObjectWithTag("Player").GetComponent<Knight>();
            t_Knight.Win();
        }
    }

    IEnumerator CR_Flash()
    {
        for (int i = 0; i < 4; i++)
        {
            m_SpriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            m_SpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator WaitBeforeWin()
    {
        yield return new WaitForSeconds(2.0f);
    }

}