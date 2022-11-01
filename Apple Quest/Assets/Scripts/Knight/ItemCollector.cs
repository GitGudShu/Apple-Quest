using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    private int apples = 0;

    private Health m_Health;

    [SerializeField] private TextMeshProUGUI appleText;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        m_Health = GetComponent<Health>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            Destroy(collision.gameObject);
            apples++;
            appleText.text = "x " + apples;
            scoreText.text = "x " + apples;
        }

        if (collision.gameObject.CompareTag("Heart"))
        {
            Destroy(collision.gameObject);
            m_Health.numberOfHearts ++;
            m_Health.health ++;
        }

        if (collision.gameObject.CompareTag("Potion"))
        {
            Destroy(collision.gameObject);
            m_Health.health++;
        }
    }
}
