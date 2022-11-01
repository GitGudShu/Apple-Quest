using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public Rigidbody2D body2D;
    public float leftPushRange;
    public float rightPushRange;
    public float velocityThreshold;

    // Start is called before the first frame update
    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        body2D.angularVelocity = velocityThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        Push(); 
    }

    public void Push()
    {
        if (transform.rotation.z > 0
            && transform.rotation.z < rightPushRange
            && (body2D.angularVelocity > 0)
            && (body2D.angularVelocity < velocityThreshold))
            body2D.angularVelocity = velocityThreshold;
        else if (transform.rotation.z < 0
            && transform.rotation.z > leftPushRange
            && (body2D.angularVelocity < 0)
            && (body2D.angularVelocity > velocityThreshold * -1))
            body2D.angularVelocity = velocityThreshold * -1;
    }
}
