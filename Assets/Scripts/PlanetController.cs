﻿#define HANDLE_OWN_MOVEMENT

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    public Transform moveTarget;
    [SerializeField]
    public float distanceFromMoveTarget = 3f;

    [Header("Asteroids")]
    [SerializeField]
    private Asteroid asteroidPrefab = null;
    [SerializeField]
    private int asteroidCount = 1;

    // List containing all asteroids controlled by this planet
    private List<Asteroid> asteroids;

    // Linked List containing the asteroids that can be launched
    private LinkedList<Asteroid> asteroidQueue;

    void Start() {
        asteroids = new List<Asteroid>();
        //asteroidQueue = new Queue<Asteroid>();
        asteroidQueue = new LinkedList<Asteroid>();

        // Spawn asteroids evenly spaced around the planet with random orbit directions.
        for (int i = 0; i < asteroidCount; i++) {
            var asteroid = SpawnAsteroid(asteroidPrefab);
            asteroid.orbit_direction = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
            asteroid.orbit_angle = ((float)i / asteroidCount) * 360f;
        }
        UpdateActiveAsteroid();
    }

    /// <summary>
    /// Moves the planet around the its target by its move speed.
    /// </summary>
    /// <param name="input">Range [-1.0, 1.0]</param>
    public void MoveAroundTarget(float input) {
        // Snap the planet to the correct distance from the target
        if (Vector3.Distance(moveTarget.transform.position, transform.position) != distanceFromMoveTarget) {
            Vector3 fromTarget = (transform.position - moveTarget.transform.position).normalized;
            transform.position = moveTarget.transform.position + (fromTarget * distanceFromMoveTarget);
        }

        // Calculate the degree of movment around the target, based on distance
        float rotateAmount = (-Mathf.Clamp(input, -1f, 1f) * moveSpeed * Time.deltaTime)
            / (2.0f * Mathf.PI * distanceFromMoveTarget) * 360f;

        // Rotate the planet around the target's position
        transform.RotateAround(moveTarget.transform.position, Vector3.up, rotateAmount);
        transform.rotation = Quaternion.LookRotation((moveTarget.position - transform.position).normalized);
    }

    /// <summary>
    /// Spawns an asteroid and sets up all required values and listeners
    /// </summary>
    public Asteroid SpawnAsteroid(Asteroid prefab) {
        var new_asteroid = Instantiate(asteroidPrefab);

        // Setup the asteroid's parent
        new_asteroid.orbit_target = this.transform;

        // Setup lisenters for the asteroid's event
        new_asteroid.EventLaunched += OnAsteroidLauncehd;
        new_asteroid.EventReturned += OnAsteroidReturned;
        new_asteroid.EventCollided += OnAsteroidCollided;

        asteroids.Add(new_asteroid);
        //asteroidQueue.Enqueue(new_asteroid);
        asteroidQueue.AddLast(new_asteroid);
        return new_asteroid;
    }

    /// <summary>
    /// Destroys an asteroid owned by this planet.
    /// </summary>
    public void DestroyAsteroid(Asteroid instance) {
        if (asteroids.Contains(instance)) {
            instance.EventLaunched -= OnAsteroidLauncehd;
            instance.EventReturned -= OnAsteroidReturned;
            instance.EventCollided -= OnAsteroidCollided;

            asteroids.Remove(instance);
            asteroidQueue.Remove(instance);

            Destroy(instance.gameObject);
        }
    }

    /// <summary>
    /// Start charging for the nex launch attack
    /// </summary>
    public void StartCharge() {
        if (asteroidQueue.Count > 0) {
            asteroidQueue.First.Value.StartCharge();
        }
    }

    /// <summary>
    /// Launch the first available asteroid from the planet
    /// </summary>
    public void LaunchAttack() {
        if (asteroidQueue.Count > 0) {
            var asteroid = asteroidQueue.First.Value;

            asteroidQueue.RemoveFirst();

            asteroid.Launch();
            UpdateActiveAsteroid();
        }
    }

    /// <summary>
    /// Call when the asteroid queue is modified
    /// </summary>
    private void UpdateActiveAsteroid() {
        if (asteroidQueue.Count > 0) {
            asteroidQueue.First.Value.isActive = true;
        }
    }

    /// <summary>
    /// Is Triggered when one of the planet's asteroids is launched
    /// </summary>
    /// <param name="asteroid"></param>
    private void OnAsteroidLauncehd(Asteroid asteroid) {
        asteroid.isActive = false;
    }

    /// <summary>
    /// Is Triggered when one of the planet's asteroids returns to orbit
    /// </summary>
    private void OnAsteroidReturned(Asteroid asteroid) {
        asteroidQueue.AddLast(asteroid);
        UpdateActiveAsteroid();
    }

    /// <summary>
    /// Is Triggered when one of the planet's asteroids collides with another object
    /// </summary>
    private void OnAsteroidCollided(Asteroid asteroid) {

    }
}
