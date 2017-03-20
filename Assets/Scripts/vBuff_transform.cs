using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vBuff_transform : MonoBehaviour {

	public vBuffUpdater updater;

	Matrix4x4 matrix;

	// Use this for initialization
	void OnEnable() {
		if( updater == null){
			updater = GetComponent<vBuffUpdater>();
		}
		updater.OnBeforeDispatch += updateTransform;
	}

	void OnDisable(){
		updater.OnBeforeDispatch -= updateTransform;
	}


	void updateTransform(ComputeShader computeShader , int _kernel){


		matrix = transform.localToWorldMatrix; 
		float[] matrixFloats = new float[] 
		{ 
		matrix[0,0], matrix[1, 0], matrix[2, 0], matrix[3, 0], 
		matrix[0,1], matrix[1, 1], matrix[2, 1], matrix[3, 1], 
		matrix[0,2], matrix[1, 2], matrix[2, 2], matrix[3, 2], 
		matrix[0,3], matrix[1, 3], matrix[2, 3], matrix[3, 3] 
		}; 

		computeShader.SetFloats( "transform" , matrixFloats );

	}
	

}
