using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Knight t_Knight = collision.gameObject.GetComponent<Knight>();

        if (t_Knight && !t_Knight.isImmune) // si t_Player est null
        {
            Debug.Log("Player kaboboom!");
            t_Knight.TakeDamage(1);
        }
    }
}
