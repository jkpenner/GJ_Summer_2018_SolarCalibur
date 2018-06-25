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

    void ResetTimer()
    {
        tilNextSpawn = spawnInterval + Random.Range(-randomness, randomness);
    }

    public void Spawn()
    {
        var go = (GameObject)Instantiate(toSpawn, transform.position + Random.insideUnitSphere * spawnPointRandomness, Quaternion.identity);
        go.transform.rotation = Quaternion.LookRotation(go.transform.position - transform.position);
    }

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
