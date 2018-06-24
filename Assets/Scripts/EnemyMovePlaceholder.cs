using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovePlaceholder : MonoBehaviour {

    float timeTilChangeDirection;
    float timer;
    Vector3 moveDirection;

    public float maxSpeed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Time.time > timeTilChangeDirection + timer)
        {
            timer = Time.time;
            moveDirection = Random.insideUnitSphere.normalized;
            moveDirection.y = moveDirection.y / 2f;
            timeTilChangeDirection = Random.Range(1f, 4f);
        }

        var speed = Mathf.PingPong(maxSpeed * 2f, Time.time) - maxSpeed;

        transform.Translate(moveDirection * speed * Time.fixedDeltaTime);
    }
}
