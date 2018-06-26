using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public GameObject toSpawn;

    public float spawnInterval;
    public float randomness;

    public float spawnPointRandomness;

	// Use this for initialization
	void Start () {
        ResetTimer();

	}

    // Update is called once per frame

    // void projectileDirection() {
    //     var heading = (Player?)transform.position - (Missle?)transform.position;
    //     need to do something here to get the radius of the sphere maybe?

    //     var distance = heading.magnitude;
    //     var direction = heading / distance; // This is now the normalized direction.

    // }

    void ResetTimer()
    {
        tilNextSpawn = spawnInterval + Random.Range(-randomness, randomness);
    }

    public void Spawn()
    {
        var go = (GameObject)Instantiate(toSpawn, transform.position + Random.insideUnitSphere * spawnPointRandomness, Quaternion.identity);
        go.transform.rotation = Quaternion.LookRotation(go.transform.position - transform.position);
    }

    // public void Spawn()
    // {
    //     var go = (GameObject)Instantiate(toSpawn);
    //     var heading = go.transform.position - transform.position;
    //     var distance = heading.magnitude;
    //     var direction = heading / distance;
    //     // go.transform.rotation = Quaternion.LookRotation(direction);
    //     go.transform.rotation = LookRotation(direction);
    // }

    float tilNextSpawn;
	void Update () {
        tilNextSpawn -= Time.deltaTime;
        if(tilNextSpawn <= 0f)
        {
            ResetTimer();
            Spawn();
        }
	}
}
