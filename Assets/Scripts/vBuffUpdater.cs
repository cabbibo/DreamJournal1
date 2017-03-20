using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vBuffUpdater : MonoBehaviour {

	public ComputeShader computeShader;
	public vBuffer vertBuffer;

	public bool update;

	private int _kernel;

	public delegate void BeforeDispatch(ComputeShader shader, int kernel);
  	public event BeforeDispatch OnBeforeDispatch;


	// Use this for initialization
	void Start () {

		_kernel = computeShader.FindKernel("CSMain");


	   	
	   	//computeShader.Dispatch( _kernel, vertBuffer.SIZE , vertBuffer.SIZE , vertBuffer.SIZE );
		
	}

	public void DispatchComputeShader(){

		

		computeShader.SetFloat( "_DeltaTime"    , Time.deltaTime );
        computeShader.SetFloat( "_Time"         , Time.time      );

		computeShader.SetInt( "StrideX" , vertBuffer.strideX );
		computeShader.SetInt( "StrideY" , vertBuffer.strideY );
		computeShader.SetInt( "StrideZ" , vertBuffer.strideZ );


		if(OnBeforeDispatch != null) OnBeforeDispatch( computeShader , _kernel);


		computeShader.SetBuffer( _kernel , "vertBuffer"     , vertBuffer._buffer );
	   	computeShader.Dispatch( _kernel, vertBuffer.SIZE , vertBuffer.SIZE , vertBuffer.SIZE );

	}
	
	// Update is called once per frame
	void FixedUpdate () {


		
		if( update == true ){ DispatchComputeShader(); }
		
	}
		
	
}
