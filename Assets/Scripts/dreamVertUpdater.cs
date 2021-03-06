﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dreamVertUpdater : MonoBehaviour {
	
	public ComputeShader computeShader;
	public dreamVertBuffer vertBuffer;

	public bool update;

	private int _kernel;

	public delegate void BeforeDispatch(ComputeShader shader, int kernel);
 	public event BeforeDispatch OnBeforeDispatch;


	// Use this for initialization
	void Start () {

		_kernel = computeShader.FindKernel("CSMain");

		SET();


	   	
	   	//computeShader.Dispatch( _kernel, vertBuffer.SIZE , vertBuffer.SIZE , vertBuffer.SIZE );
		
	}

	public void DispatchComputeShader(int SET){

		computeShader.SetInt("_SET" ,SET);

		computeShader.SetFloat( "_Delta"    , Time.deltaTime );
    	computeShader.SetFloat( "_Time"         , Time.time      );

		computeShader.SetInt( "StrideX" , vertBuffer.strideX );
		computeShader.SetInt( "StrideY" , vertBuffer.strideY );
		computeShader.SetInt( "StrideZ" , vertBuffer.strideZ );

		if(OnBeforeDispatch != null) OnBeforeDispatch( computeShader , _kernel);

		computeShader.SetBuffer( _kernel , "vertBuffer"     , vertBuffer._buffer );
	  	computeShader.Dispatch( _kernel, vertBuffer.threadX , vertBuffer.threadY , vertBuffer.threadZ );

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if( update == true ){ DispatchComputeShader(0); }
		
	}

	public void updateOnce(){
		DispatchComputeShader(0);
	}

	public void SET(){
		
		DispatchComputeShader(1);
		//computeShader.SetInt("_SET" ,0);
	}
		
	
}
