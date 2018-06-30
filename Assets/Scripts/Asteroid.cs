using System;
using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    public event System.Action<Asteroid> EventLaunched;
    public event System.Action<Asteroid> EventReturned;
    public event System.Action<Asteroid, Planet> EventCollided;

    [Header("Setup")]
    public RectTransform targeting;
    public new Rigidbody rigidbody;
    public int Damage;

    [Header("Orbit")]
    public float orbit_speed = 6f;
    public float orbit_radius = 1f;

    [HideInInspector] // Assigned by PlanetController
    public Planet orbit_target;
    [HideInInspector] // Assigned by PlanetController
    public float orbit_angle = 0f;
    [HideInInspector] // Assigned by PlanetController
    public float orbit_direction = 1f;

    [Header("Launch")]
    public float launch_speed = 15f;
    public float return_distance = 12f;

    [Header("Charging")]
    public float charge_time = 1f;
    public float min_charge_mod = 1f;
    public float max_charge_mod = 3f;

    [NonSerialized]
    public float active_charge_mod = 1f;
    private Coroutine chargeCoroutine = null;

    [HideInInspector]
    public bool isActive = false;
    private bool isOrbitting = true;
    private bool isReturning = false;
    private float returningPercent = 0f;
/*
    private AudioSource[] sounds;
    public AudioSource   hit1;
    public AudioSource   hit2;

*/
    public CameraAudio Audio;
    

    public bool IsOrbitting { get { return isOrbitting; } }

    public float OrbitSpeed {
        get { return isActive ? orbit_speed : orbit_speed * 0.75f; }
    }

    public float OrbitRadius {
        get { return isActive ? orbit_radius : orbit_radius * 0.75f; }
    }

    private void Awake() {
        this.rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null) {
            rigidbody = gameObject.AddComponent<Rigidbody>();
        }
    
        rigidbody.isKinematic = true;
        Audio = MainCameraOverride.Instance.GetComponent<CameraAudio>();// FindObjectOfType<CameraAudio>();
    /*
        sounds = GetComponents<AudioSource>();
        hit1 = sounds[0];
        hit2 = sounds[1];
     */
}


    public Vector3 TargetOrbitPosition {
        get {
            Vector3 toPosition = new Vector3(Mathf.Cos(orbit_angle), 0f, Mathf.Sin(orbit_angle));
            if (orbit_target != null) {
                return orbit_target.transform.position + (toPosition * OrbitRadius);
            }
            return toPosition * OrbitRadius;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Hit " + collision.transform.name);

        isReturning = true;
        returningPercent = 1f;

        var planet = collision.gameObject.GetComponent<Planet>();
        if (planet == null) {
            planet = collision.gameObject.GetComponentInParent<Planet>();
        }

        if (planet != null && EventCollided != null) {
            //hit1.Play();
            Audio.EarthHitAud();
            EventCollided.Invoke(this, planet);
            
        }
    }


    public void FixedUpdate() {

        if (orbit_target == null || orbit_target.IsAlive == false) {
            return;
            //Destroy(this); //TODO:: put in a better place

        }

        targeting.gameObject.SetActive(isActive && isOrbitting);


        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders) {
            collider.enabled = isActive || isOrbitting == false;
        }


        if (isOrbitting) {
            float percent_of_cir = (OrbitSpeed * active_charge_mod * Time.deltaTime)
                / (2.0f * Mathf.PI * OrbitRadius);
            float move_in_degrees = 360f * percent_of_cir * orbit_direction;


            // Modify the oribit angle by how much we moved across the circumference
            this.orbit_angle += (move_in_degrees * Mathf.Deg2Rad);

            // Determine the vector towards the position based off the current angle
            Vector3 toPosition = new Vector3(Mathf.Cos(orbit_angle), 0f, Mathf.Sin(orbit_angle));

            // Get the forward vector for the asteroid, adjusted for move direction
            Vector3 forward = Vector3.Cross(toPosition, Vector3.up) * orbit_direction;

            // Update the rigidbody's position and rotation values
            rigidbody.MovePosition(orbit_target.transform.position + (toPosition * OrbitRadius));
            rigidbody.MoveRotation(Quaternion.LookRotation(forward));
        } else {
            float move_amount = launch_speed * active_charge_mod * Time.deltaTime;

            CheckIfAsteroidNeedsToReturn();

            Vector3 next_moved_position = rigidbody.position + transform.forward * move_amount;

            float distToTarget = Vector3.Distance(TargetOrbitPosition, rigidbody.position);

            Vector3 next_return_position;
            if (Mathf.Max(distToTarget - move_amount, 0f) == 0) {
                next_return_position = TargetOrbitPosition;
                OnReturned();
            } else {
                next_return_position = rigidbody.position +
                    ((TargetOrbitPosition - rigidbody.position).normalized * move_amount);
                next_return_position = Vector3.Lerp(next_moved_position,
                    next_return_position, returningPercent);
            }
            rigidbody.MovePosition(next_return_position);
        }
    }

    private void OnReturned() {
        isReturning = false;
        isOrbitting = true;
        returningPercent = 0f;
        active_charge_mod = min_charge_mod;

        if (EventReturned != null) {
            EventReturned.Invoke(this);
        }
    }

    private void CheckIfAsteroidNeedsToReturn() {
        // Asteroid is already returning exit early
        if (isReturning) return;

        if (Vector3.Distance(this.rigidbody.position, TargetOrbitPosition) > return_distance) {
            StartReturn();
        }
    }

    public void StartReturn() {
        if (isReturning) return;

        isReturning = true;
        isOrbitting = false;
        StartCoroutine(StartReturning(1f));
    }

    private IEnumerator StartReturning(float transition_time) {
        if (transition_time <= 0f) yield break;

        returningPercent = 0f;

        float counter = 0f;
        while (counter < transition_time) {
            counter += Time.deltaTime;

            returningPercent = counter / transition_time;

            yield return null;
        }
        returningPercent = 1f;
    }

    public void StartCharge() {
        if (chargeCoroutine != null) {
            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
        }
        chargeCoroutine = StartCoroutine(StartCharging());
    }

    private IEnumerator StartCharging() {
        if (charge_time <= 0f) yield break;

        active_charge_mod = min_charge_mod;

        float counter = 0f;
        while (counter < charge_time && isOrbitting) {
            counter = Mathf.Clamp(counter + Time.deltaTime, 0f, charge_time);

            active_charge_mod = ((max_charge_mod - min_charge_mod) * (counter / charge_time)) + min_charge_mod;
            yield return null;
        }

        chargeCoroutine = null;
    }

    public void Launch() {
        if (isOrbitting) {
            isOrbitting = false;

            if (chargeCoroutine != null) {
                StopCoroutine(chargeCoroutine);
                chargeCoroutine = null;
            }

            // When launching the asteroid from one side of the planet we want to 
            // return it to the opposite side. Update the orbit angle
            this.orbit_angle += (180f * Mathf.Deg2Rad);

            // Trigger the asteroid launch event
            if (this.EventLaunched != null) {
                this.EventLaunched.Invoke(this);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(TargetOrbitPosition, 0.5f);
    }
}
