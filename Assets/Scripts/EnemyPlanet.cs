using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlanet : MonoBehaviour
{
    public GameObject Projectile;     //The object this planet will fire at the Player
    public float FireTimerMin = 1.5f; //The min time this enemy planet will take before firing it's next shot
    public float FireTimerMax = 5.0f; //The max time this enemy planet will take before firing it's next shot
    public float MaxSpeed = 6.0f;       //The max speed this enemy planet can move
    public float MoveDistanceMax = 1.8f;       //The max move distance this planet is allowed to move in either direction
    public bool CanMove = false;
    public float Health = 100;
    //TODO:: Add player object here so enemy can target the player

    private float fireTimer;    //The timer that gets reset after this enemy fires at their target
    private float timeTilChangeDirection;
    private float timer;
    private Vector3 moveDirection;
    private float initialZPos;

    void Start ()
    {
        fireTimer = Random.Range(FireTimerMin, FireTimerMax);
        initialZPos = transform.position.z;
    }
	
	void Update ()
    {
        fireTimer -= Time.deltaTime;

        //Once the fire timer hits zero, fire the asteroid and reset the timer
        if (fireTimer <= 0.0f)
        {
            Fire();
            fireTimer = Random.Range(FireTimerMin, FireTimerMax);
        }
    }

    void FixedUpdate()
    {
        if (CanMove && Time.time > 0)
        {
            if (Time.time > timeTilChangeDirection + timer)
            {
                timer = Time.time;
                moveDirection = Random.insideUnitSphere.normalized;
                //Enemy can only move in x direction
                moveDirection.y = 0f;
                moveDirection.z = 0f;
                timeTilChangeDirection = Random.Range(1f, 4f);
            }

            var speed = Mathf.PingPong(MaxSpeed * 2f, Time.time) - MaxSpeed;

            transform.Translate(moveDirection * speed * Time.fixedDeltaTime);

            if (Mathf.Abs(transform.localPosition.x) > MoveDistanceMax && MoveDistanceMax > 0f)
            {
                float new_x = (transform.localPosition.x >= 0 ? 1 : -1) * MoveDistanceMax;

                transform.localPosition = new Vector3(new_x, 0f, initialZPos);
            }
        }
    }

    void Fire()
    {
        //Make new projectile that fires from this planet's position   
        GameObject projectile = (GameObject)Instantiate(Projectile, transform.position + Random.insideUnitSphere * 3.0f, Quaternion.identity); //TODO:: Add offset positions
        projectile.SetActive(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            //Health -= asteroid.Damage;
        }
    }
}
