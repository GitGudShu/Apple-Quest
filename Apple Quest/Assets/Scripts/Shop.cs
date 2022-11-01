using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    Knight knight;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Knight")
            collision.gameObject.GetComponent<Knight>().Win();
    }
}
