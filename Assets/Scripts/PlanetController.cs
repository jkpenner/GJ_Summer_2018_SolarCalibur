using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour {
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float moveDistanceMax = 1.8f;

    [Header("Asteroids")]
    [SerializeField]
    private Asteroid asteroidPrefab = null;
    [SerializeField]
    private int asteroidCount = 1;

    // List containing all asteroids controlled by this planet
    private List<Asteroid> asteroids;

    // Queue of asteroids available to be launched
    private Queue<Asteroid> asteroidQueue;

    void Start() {
        asteroids = new List<Asteroid>();
        asteroidQueue = new Queue<Asteroid>();
        for (int i = 0; i < asteroidCount; i++) {
            asteroids.Add(Instantiate(asteroidPrefab));
            asteroids[i].orbit_target = this.transform;
            asteroids[i].orbit_angle = ((float)i / asteroidCount) * 360f;
            //asteroids[i].orbit_radius = 1f;
            asteroids[i].orbit_direction = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

            //asteroids[i].transform.SetParent(this.transform, false);
            asteroids[i].EventLaunched += OnAsteroidLauncehd;
            asteroids[i].EventReturned += OnAsteroidReturned;


            asteroidQueue.Enqueue(asteroids[i]);
        }
    }

    private void OnAsteroidLauncehd(Asteroid asteroid) {
        asteroid.isActive = false;
    }

    private void OnAsteroidReturned(Asteroid asteroid) {
        asteroidQueue.Enqueue(asteroid);
    }

    void Update() {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.localPosition += input * moveSpeed * Time.deltaTime;
        if (Mathf.Abs(transform.localPosition.x) > moveDistanceMax && moveDistanceMax > 0f) {
            float new_x = (transform.localPosition.x >= 0 ? 1 : -1) * moveDistanceMax;

            transform.localPosition = new Vector3(new_x, 0f, 0f);
        }




        if (asteroidQueue.Count > 0) {
            asteroidQueue.Peek().isActive = true;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (asteroidQueue.Count > 0) {
                asteroidQueue.Peek().StartCharge();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (asteroidQueue.Count > 0) {
                asteroidQueue.Dequeue().Launch();
            }



            //ShootAvalibleAsteroid();
        }
    }

    private void ShootAvalibleAsteroid() {
        for (int i = 0; i < asteroids.Count; i++) {
            if (asteroids[i].IsOrbitting)
                continue;

            asteroids[i].isActive = false;
            asteroids[i].Launch();
            break;
        }
    }
}
