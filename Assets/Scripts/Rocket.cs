﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile
{
    public Transform target;
    public float TurnSpeed;
    public float LifeSpan;
    [Range(0.0f, 1.0f)]
    public float HomingChance;

    private bool IsHoming;

    // Use this for initialization
    public override void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();

        IsHoming = false;

        //If we're not a homing missile, we go the max speed
        if (Random.Range(0.0f, 1.0f) <= HomingChance)
        {
            IsHoming = true;
        }
        else
        {
            SetVelocity(SpeedMax);
        }
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        if (target != null)
        {
            LifeSpan -= Time.fixedDeltaTime;

            if (LifeSpan <= 0f)
            {
                Destroy(gameObject);
            }

            if(IsHoming)
            {
                RotateTowardsTarget();

                if (m_RigidBody != null)
                {
                    m_RigidBody.velocity = transform.forward * m_Speed;
                }
            }
        }
    }

    void RotateTowardsTarget()
    {
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        string collisionTag = collision.gameObject.tag;
        if(collisionTag.Equals("Enemy"))
        {
            return;
        }

        Planet planet = collision.gameObject.GetComponent<Planet>();

        if (planet == null)
        {
            planet = collision.gameObject.GetComponentInParent<Planet>();
        }

        if(planet == null && collision.gameObject.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if(asteroid != null)
            {
                planet = asteroid.orbit_target;
            }
        }

        if(planet != null) //TODO:: Change if we have multiple players
        {
            planet.Damage(this.Damage);
            Destroy(gameObject);
        }
    }
}
