﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAI : MonoBehaviour
{
    public GameObject Asteroid;       //The object this planet will fire at the Player
    public float FireTimerMin = 1.5f; //The min time this enemy planet will take before firing it's next shot
    public float FireTimerMax = 5.0f; //The max time this enemy planet will take before firing it's next shot
    public float MaxMove = 3f;        //The maximum the enemy planet can move from center

    //TODO:: Add player object here so enemy can target the player

    private Planet player;

    private float FireTimer;    //The timer that gets reset after this enemy fires at their target
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
        FireTimer = Random.Range(FireTimerMin, FireTimerMax);
        player = GameManager.PlayerPlanet;
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

        FireTimer -= Time.deltaTime;

        //Once the fire timer hits zero, fire the asteroid and reset the timer
        if (FireTimer <= 0.0f)
        {
            Fire();
            FireTimer = Random.Range(FireTimerMin, FireTimerMax);
        }

        
    }

	void Move ()
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

	void Fire()
    {
        //Make new asteroid that fires from this planet's position
        GameObject asteroid = (GameObject)Instantiate(Asteroid, transform.position, transform.rotation); //TODO:: Add offset positions
        asteroid.SetActive(true);

        // Destroy the asteroid after 4 seconds
        //TODO:: Make a projectile manager to take care of this!?
        Destroy(asteroid, 4.0f);
    }
}
