using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovePlaceholder : MonoBehaviour {

    float timeTilChangeDirection;
    float timer;
    Vector3 moveDirection = Vector3.zero;

    public float maxSpeed;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Time.time > 0) {
            if (Time.time > timeTilChangeDirection + timer) {
                timer = Time.time;
                moveDirection = Random.insideUnitSphere.normalized;
                //Enemy can only move in x direction
                moveDirection.y = 0f;
                moveDirection.z = 0f;
                timeTilChangeDirection = Random.Range(1f, 4f);
            }

            var speed = Mathf.PingPong(maxSpeed * 2f, Time.time) - maxSpeed;

            transform.Translate(moveDirection * speed * Time.fixedDeltaTime);

            timeTilChangeDirection = Random.Range(1f, 4f);
        }
    }
}
