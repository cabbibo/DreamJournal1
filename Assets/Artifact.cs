using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour {

	public DREAMJOURNAL journal;

	public MeshRenderer[] renderers;

	public Vector3 targetPos;

	private Rigidbody rb;

	private Vector3 v;
	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		v = targetPos - transform.position;
		rb.AddForce( v * .1f );
		
	}

	public void MakeAlive(){

		GetComponent<Collider>().enabled = true;

		for( int i =0; i< renderers.Length; i++){

		
			renderers[i].enabled = true;
		}

	}

	public void MakeDead(){



		GetComponent<Collider>().enabled = false;

		for( int i =0; i< renderers.Length; i++){

			renderers[i].enabled = false;
		}

	}

	public void MoveUp(){

		targetPos = new Vector3( 0 , 1 , 0);

	}

	void OnCollisionEnter(Collision c){

	}

	void Touched1(){
		journal.NextSection();
	}
}
