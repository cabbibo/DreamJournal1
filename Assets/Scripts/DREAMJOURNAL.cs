using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DREAMJOURNAL : MonoBehaviour {

	public anchorBuffer roomBuffer;
	public anchorBuffer spacePuppyBuffer;
	public anchorBuffer starBuffer;

	public float roomAlive;
	public float starsAlive;
	public float spacePuppyAlive;

	//public int sections;

	public dreamVertUpdater updater;

	void OnEnable() {

		roomBuffer.PopulateBuffer();
		spacePuppyBuffer.PopulateBuffer();
		starBuffer.PopulateBuffer();
		
		updater.OnBeforeDispatch += updateDream;
	}

	void OnDisable(){

		roomBuffer.ReleaseBuffer();
		spacePuppyBuffer.ReleaseBuffer();
		starBuffer.ReleaseBuffer();
		
		updater.OnBeforeDispatch -= updateDream;
	}

	void updateDream(ComputeShader computeShader , int _kernel){

		computeShader.SetBuffer( _kernel , "roomBuffer" , roomBuffer._buffer );
		computeShader.SetBuffer( _kernel , "starBuffer" , starBuffer._buffer );
		computeShader.SetBuffer( _kernel , "spacePuppyBuffer" , spacePuppyBuffer._buffer );

	}


	void OnRenderObject(){
		//print("hello");
	}


	
	// Update is called once per frame
	void Update () {
		
	}
}
