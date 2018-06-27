using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage = 1;          //The damage this projectile does
    public float SpeedMin = 7.0f;  //The min speed this projectile will move at 
    public float SpeedMax = 12.0f; //The max speed this projectile will move at 

    protected float m_Speed;       //The projectile's velocity
    protected Rigidbody m_RigidBody;

    public virtual void Start ()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        SetVelocity(Random.Range(SpeedMin, SpeedMax));
    }

    public virtual void SetVelocity(float Speed)
    {
        if (m_RigidBody != null)
        {
            m_Speed = Speed;
            m_RigidBody.velocity = transform.forward * m_Speed;
        }
    }

    public virtual void OnCollisionEnter(Collision collision) { }
}
