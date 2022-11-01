using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity = new Vector3(0f, -20f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
