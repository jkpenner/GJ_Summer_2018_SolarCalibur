using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
	}
    Rigidbody rb;

    public Transform target;
    public float accel;
    public float turnSpeed;

    public float lifeSpan;
	
	// Update is called once per frame
	void FixedUpdate () {
        //rb.AddForce(transform.forward * accel * Time.fixedDeltaTime, ForceMode.Acceleration);

        lifeSpan -= Time.fixedDeltaTime;
        if (lifeSpan <= 0f)
        {
            Destroy(gameObject);
        }

        accel += Time.fixedDeltaTime;

        transform.Translate(Vector3.forward * accel * Time.fixedDeltaTime,transform);

        Vector3 targetDir = target.position - transform.position;
        // The step size is equal to speed times frame time.
        float step = turnSpeed * Time.fixedDeltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);
        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
