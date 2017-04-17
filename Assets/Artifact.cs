using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour {

	public DREAMJOURNAL journal;

	public Material material;

	public MeshRenderer[] renderers;

	public Vector3 targetPos;

	private Rigidbody rb;
	private bool alive = false;

	private Vector3 v;
	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		v = transform.parent.TransformPoint(targetPos) - transform.position;
		rb.AddForce( v * .1f );
		
	}

	void OnRenderObject(){

		if( alive == true ){
			// Render the Triangles	
			material.SetPass(0);
			material.SetFloat( "smoothedSection" , journal.smoothedSection);
			material.SetBuffer( "_vertBuffer", journal.dreamBuffer._buffer );
			Graphics.DrawProcedural(MeshTopology.Triangles, journal.dreamBuffer.fullVertCount );
		}
	}

	public void MakeAlive(){

		GetComponent<Collider>().enabled = true;

	/*	for( int i =0; i< renderers.Length; i++){
			renderers[i].enabled = true;
		}*/

		alive = true;

	}

	public void MakeDead(){



		GetComponent<Collider>().enabled = false;

		for( int i =0; i< renderers.Length; i++){

			renderers[i].enabled = false;
		}

		alive = false;

	}

	public void MoveUp(){

		targetPos = new Vector3( 0 , 1 , 0);

	}

	public void MakeLight(){

		targetPos = new Vector3( 0 , 20 , 0);

	}

	void OnCollisionEnter(Collision c){

		//print("watt");
		Touched1();


	}

	void Touched1(){
		journal.ArtifactTouched();
	}
}
