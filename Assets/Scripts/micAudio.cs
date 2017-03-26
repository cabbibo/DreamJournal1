using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class micAudio : MonoBehaviour {

	// Use this for initialization
	void Start () {

		
		AudioSource aud = GetComponent<AudioSource>();
		print( Microphone.devices[1] );
		
     	aud.clip = Microphone.Start(Microphone.devices[1], true, 1, 44100);
     // \/
     	//yield WaitForSeconds(1);
     	aud.Play();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
