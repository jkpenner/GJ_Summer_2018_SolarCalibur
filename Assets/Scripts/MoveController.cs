using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {

    //the enemy planet's transform
    public Transform enemy;

    //a helper transform that follows the enemy
    public Transform target;

    //child of target, rotates around target and determines x and z position of the planet
    Transform pivot;

	// Use this for initialization
	void Start () {
        transform.LookAt(target);
        pivot = target.GetChild(0);
        pivot.position = target.position + (Vector3.forward * 15f);
        pivot.LookAt(target);
    }

    public float orbitSpeed; //speed for rotating around enemy

    float hInput; //input for rotating around enemy
    float vInput; //input for vertical movement
    float fInput; //input for moving towards and away from enemy

    float vDamp;

    public bool canMoveForwardAndBack;

    // Update is called once per frame
    void FixedUpdate () {

        target.position = Vector3.Lerp(target.position, enemy.position, Time.deltaTime * (1 + Vector3.Distance(target.position,enemy.position)));
        pivot.LookAt(target);

        hInput = Input.GetAxis("Horizontal");
        //vInput = Input.GetAxis("Vertical");

        //up and down arrows mapped to separate axis
        fInput = Input.GetAxis("Vertical");

        //turn slower when you're not moving
        var turnSpeed = Mathf.Abs(hInput) > .1 ? 5:3f;

        //clamp vertical distance from enemy

        /*
        var yDist = Mathf.Abs((target.position - transform.position).y);
        vDamp = yDist > 3f && Mathf.Sign(vInput) != Mathf.Sign((target.position - transform.position).y) ? Mathf.Lerp(vDamp,yDist,Time.deltaTime * 3f) : 1f;
        transform.Translate(Vector3.up * vInput * Time.fixedDeltaTime * (10f / Mathf.Pow(vDamp,2f)));
        if(yDist > 4f)
        {
            var newPos = transform.position;
            newPos.y = target.position.y - Mathf.Clamp(target.position.y - transform.position.y, -4f, 4f);
            transform.position = newPos;
        }
        */

        //move forward and back
        if (canMoveForwardAndBack)
        {
            pivot.Translate(Vector3.forward * fInput * Time.fixedDeltaTime * 10f, transform);
        }

        //orbit around enemy
        target.Rotate(new Vector3(0, -hInput * orbitSpeed, 0));

        var p = pivot.position;
        transform.position = new Vector3(p.x, transform.position.y, p.z);

        //look at enemy
        Vector3 direction = enemy.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform t)
    {
        target.position = t.position;
    }
}
