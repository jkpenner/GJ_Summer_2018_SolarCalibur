using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Projectile {
    public Transform target;
    public float TurnSpeed;
    public float LifeSpan;
    [Range(0.0f, 1.0f)]
    public float HomingChance;
/*
public AudioSource[] sounds;
public AudioSource   hit1;
public AudioSource   hit2;
 */

    public CameraAudio Audio;

    private bool IsHoming;

    // Use this for initialization
    public override void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();

        IsHoming = false;

        //If we're not a homing missile, we go the max speed
        if (Random.Range(0.0f, 1.0f) <= HomingChance) {
            IsHoming = true;
        } else {
            SetVelocity(SpeedMax);
        }
        Audio = FindObjectOfType<CameraAudio>();
        /*
        sounds = GetComponents<AudioSource>();
        hit1 = sounds[0];
        hit2 = sounds[1];
         */
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (target != null) {
            LifeSpan -= Time.fixedDeltaTime;

            if (LifeSpan <= 0f) {
                Destroy(gameObject);
            }

            if (IsHoming) {
                RotateTowardsTarget();

                if (m_RigidBody != null) {
                    m_RigidBody.velocity = transform.forward * m_Speed;
                }
            }
        }
    }

    void RotateTowardsTarget() {
        if (target != null) {
            transform.LookAt(target.transform);
        }
    }

    public override void OnCollisionEnter(Collision collision) {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag.Equals("Enemy")) {
            return;
        }

        Planet planet = collision.gameObject.GetComponent<Planet>();

        if (planet == null) {
            planet = collision.gameObject.GetComponentInParent<Planet>();
        }

        if (planet == null && collision.gameObject.CompareTag("Asteroid")) {
            // Makes the asteroid's planet take damage if the asteroid is hit
            // Don't think this should happend. Asteroids should be used to destroy
            // rockets
            //Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            //if(asteroid != null)
            //{
            //    planet = asteroid.orbit_target;
            //}
        }

        if (planet != null) //TODO:: Change if we have multiple players
        {
            Audio.PlutoHitAud();
            planet.Damage(this.Damage);
        }

// Destroy asteroid on collision:
        Destroy(gameObject);
    }
}
