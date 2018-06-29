#define HANDLE_OWN_MOVEMENT

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Planet))]
public class PlanetController : MonoBehaviour {
    private Planet _planet;

    [Header("Movement")]
    [SerializeField]
    private float speedMovement = 5f;
    [SerializeField]
    public Transform moveTarget;
    [SerializeField]
    public float distanceFromMoveTarget = 10f;

    public float minDistanceToMove = 0.1f;

    public float speedRotation = 45f;

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
        // Setup event liseners on the planet being controlled
        _planet = GetComponent<Planet>();
        _planet.EventTargetChanged += OnTargetChanged;
        // Setup trigger the OnTargetChange if there is already a target
        if (_planet.TargetPlanet != null) {
            OnTargetChanged(_planet.TargetPlanet);
        }

        asteroids = new List<Asteroid>();
        //asteroidQueue = new Queue<Asteroid>();
        asteroidQueue = new LinkedList<Asteroid>();

        // Spawn asteroids evenly spaced around the planet with random orbit directions.
        for (int i = 0; i < asteroidCount; i++) {
            var asteroid = SpawnAsteroid(asteroidPrefab);
            asteroid.orbit_direction = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
            asteroid.orbit_angle = ((float)i / asteroidCount) * 360f;
            // Setup the asteroid's parent
            asteroid.orbit_target = _planet;

        }
        UpdateActiveAsteroid();
    }

    private void OnDestroy() {
        foreach (var a in asteroids) {
            if (a != null) {
                Destroy(a.gameObject);
            }
        }
    }

    private void OnTargetChanged(Planet newTarget) {
        this.moveTarget = newTarget.transform;
    }

    /// <summary>
    /// Moves the planet around the its target by its move speed.
    /// </summary>
    /// <param name="input">Range [-1.0, 1.0]</param>
    public void MoveAroundTarget(float input) {
        if (moveTarget != null) {
            // Check if the planet is not at the right distance from the target
            float distanceFromTarget = Vector3.Distance(moveTarget.position, transform.position);
            if (Mathf.Abs(distanceFromTarget - distanceFromMoveTarget) >= minDistanceToMove) {
                //Debug.Log("Moving");
                Vector3 toTarget = (moveTarget.position - transform.position).normalized;
                Vector3 targetPosition = moveTarget.position - (toTarget *
                    (distanceFromMoveTarget + minDistanceToMove));

                // Lerp towards the target's position
                transform.position = Vector3.Lerp(transform.position,
                    targetPosition, speedMovement * Time.deltaTime);

                // Update the distanceFromTarget value
                distanceFromTarget = Vector3.Distance(
                    moveTarget.position, transform.position);
            }

            // Calculate the degree of movment around the target, based on distance
            float rotateAmount = (-Mathf.Clamp(input, -1f, 1f) * speedMovement * Time.deltaTime)
                / (2.0f * Mathf.PI * distanceFromTarget) * 360f;

            // Rotate the planet around the target's position
            transform.RotateAround(moveTarget.transform.position, Vector3.up, rotateAmount);

            // Get the new target rotation towards the target
            Quaternion targetRotation = Quaternion.LookRotation(
                (moveTarget.position - transform.position).normalized);

            // Slerp towards the new target rotation to give a bit of delay
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRotation,
                speedRotation * Time.deltaTime);
        }
    }

    /// <summary>
    /// Spawns an asteroid and sets up all required values and listeners
    /// </summary>
    public Asteroid SpawnAsteroid(Asteroid prefab) {
        var new_asteroid = Instantiate(asteroidPrefab);

        // Setup lisenters for the asteroid's event
        new_asteroid.EventLaunched += OnAsteroidLaunched;
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
            instance.EventLaunched -= OnAsteroidLaunched;
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
    private void OnAsteroidLaunched(Asteroid asteroid) {
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
    private void OnAsteroidCollided(Asteroid asteroid, Planet hitPlanet) {
        StartCoroutine(TeleportAsteroid(asteroid));

        if (hitPlanet != null) {
            Debug.LogFormat("[{0}] Hit {1}", this.name, hitPlanet.name);

            hitPlanet.Damage(asteroid.Damage);

        } else {
            Debug.LogFormat("[{0}] Hit something that was not a planet");
        }

    }

    private IEnumerator TeleportAsteroid(Asteroid asteroid) {
        var lr = asteroid.GetComponent<LineRenderer>();
        if (lr != null) lr.enabled = false;

        asteroid.gameObject.SetActive(false);
        asteroid.orbit_direction = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;


        float angle_offset = Vector3.Angle(Vector3.right, transform.right);
        asteroid.orbit_angle = asteroid.orbit_direction == 1 ? angle_offset : angle_offset + 180f;



        yield return new WaitForSeconds(1f);

        asteroid.transform.position = transform.position - (transform.forward * asteroid.return_distance);
        if (lr != null) lr.enabled = true;
        asteroid.gameObject.SetActive(true);
        asteroid.StartReturn();
    }
}
