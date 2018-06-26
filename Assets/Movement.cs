using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float MaxDistance = 5.0f;
    public float MinDistance = 1.0f;

    private float xPositionThreshold = 10.0f;
    private float initialXPos;
	// Use this for initialization
	void Start ()
    {
        initialXPos = transform.position.x;
        //Debug.Log("Max Distance =" + MaxDistance);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("Max Distance =" + MaxDistance);

        //Time.time; //total eplased time 


        Vector3 position = transform.position;

        if(position.x > initialXPos + xPositionThreshold)
        {
            position.x -= Random.Range(MinDistance, MaxDistance);
        }
        else
        {
            position.x += Random.Range(MinDistance, MaxDistance);
        }

        transform.position = position;

    }
}
