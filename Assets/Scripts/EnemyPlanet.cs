using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlanet : Planet {
    public GameObject Projectile;     //The object this planet will fire at the Player
    public float FireTimerMin = 1.5f; //The min time this enemy planet will take before firing it's next shot
    public float FireTimerMax = 5.0f; //The max time this enemy planet will take before firing it's next shot
    public float MaxSpeed = 6.0f;       //The max speed this enemy planet can move
    public float MoveDistanceMax = 1.8f;       //The max move distance this planet is allowed to move in either direction
    public bool CanMove = false;
    public float MaxMove = 3f;        //The maximum the enemy planet can move from center
    //TODO:: Add player object here so enemy can target the player

    private float fireTimer;    //The timer that gets reset after this enemy fires at their target
    private float timeTilChangeDirection;
    private float timer;
    private Vector3 moveDirection;
    private float initialZPos;
    private GameObject target;
    private float MoveTimer;
    private bool RIGHT = true;
    private bool LEFT = false;
    private bool direction;
    private int axis;
    private float maxMove;
    private bool wait;

    void Start ()
    {
        direction = LEFT;
        axis = 0;
        UpdateMoveVars();
        MsgRelay.EventGameComplete += OnGameComplete;
        fireTimer = Random.Range(FireTimerMin, FireTimerMax);
        initialZPos = transform.position.z;
        target = GameObject.FindGameObjectWithTag("Player");
        RotateTowardsTarget();
    }

    private void OnDestroy() {
        if (MsgRelay.Exists) {
            MsgRelay.EventGameComplete -= OnGameComplete;
        }
    }

	void Update ()
    {
        MoveTimer -= Time.deltaTime;

        if (!wait) Move();
        float position;
        switch (axis)
        {
            case 0: position = transform.position.x; break;
            default: position = transform.position.z; break;
        }

        if (Mathf.Abs(position) >= MaxMove) direction = !direction;
        if (MoveTimer <= 0.0f) UpdateMoveVars();

        fireTimer -= Time.deltaTime;

        //Once the fire timer hits zero, fire the asteroid and reset the timer
        if (fireTimer <= 0.0f) {
            Fire();
            fireTimer = Random.Range(FireTimerMin, FireTimerMax);
        }
    }

    void Move()
    {
        float speed = .1f;
        if (!direction) speed = -speed;
        Vector3 transformDir;
        switch (axis)
        {
            case 0: transformDir = transform.right; break;
            default: transformDir = transform.forward; break;
        }
        transform.position += transformDir * speed;
    }

    void UpdateMoveVars()
    {
        MoveTimer = Random.Range(.5f, 2f);
        direction = !direction;
        axis = Random.Range(0, 1);
        wait = Random.value > 0.5f;
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

            if (Mathf.Abs(transform.localPosition.x) > MoveDistanceMax && MoveDistanceMax > 0f) {
                float new_x = (transform.localPosition.x >= 0 ? 1 : -1) * MoveDistanceMax;

                transform.localPosition = new Vector3(new_x, 0f, initialZPos);
            }
        }

        RotateTowardsTarget();
    }

    void RotateTowardsTarget() {
        if (target != null) {
            float step = 1.0f * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, target.transform.position, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    void Fire() {
        if (target != null) {
            //Make new projectile that fires from just outside the planet's radius
            Vector3 dir = (target.transform.position - transform.position).normalized;
            SphereCollider collider = transform.GetChild(0).GetComponent<SphereCollider>();
            Vector3 spawnPos = transform.position + (dir * collider.radius);
            Quaternion rotation = Quaternion.LookRotation(spawnPos - transform.position);
            GameObject projectile = (GameObject)Instantiate(Projectile, spawnPos, rotation);
            projectile.SetActive(true);
        }
    }

    private void OnGameComplete() {
        Destroy(gameObject);
    }
}
