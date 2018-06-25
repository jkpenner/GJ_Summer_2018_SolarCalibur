using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float VelocityMin = 7.0f;  //The min velocity this projectile will move at 
    public float VelocityMax = 12.0f; //The max velocity this projectile will move at 

    private float Velocity;           //The projectile's velocity

    void Start ()
    {
        // Add velocity
        Rigidbody rigidBody = GetComponent<Rigidbody>();
        if(rigidBody != null)
        {
            Velocity = Random.Range(VelocityMin, VelocityMax);
            rigidBody.velocity = transform.forward * Velocity;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Asteroid")))
        {
            Destroy(gameObject);
        }
    }
}
