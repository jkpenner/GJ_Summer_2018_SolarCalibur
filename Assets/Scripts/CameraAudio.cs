using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAudio : MonoBehaviour {
    public AudioClip PHit1;
    public AudioClip PHit2;
    public AudioClip EHit1;
    public AudioClip EHit2;
    
    private AudioSource source;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
        
	}
	
    void EarthHitAud()
    {
        if (Random.Range(0,1)==0)
        {
            source.PlayOneShot(EHit1, 1f);
        }else
        {
            source.PlayOneShot(EHit2, 1f);
        }
    }
    void PlutoHitAud()
    {
        if (Random.Range(0,1)==0)
        {
            source.PlayOneShot(PHit1, 1f);
        }else
        {
            source.PlayOneShot(PHit2, 1f);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
