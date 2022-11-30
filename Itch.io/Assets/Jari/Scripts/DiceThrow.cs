using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceThrow : MonoBehaviour
{
    public static Vector3 Velocity;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw()
    {
        Velocity = rb.velocity;

        float x = Random.Range(0, 250);
        float y = Random.Range(0, 250);
        float z = Random.Range(0, 250);

        transform.rotation = Quaternion.identity;

        rb.AddForce(transform.up * 500);
        rb.AddTorque(x, y, z);
    }
}
