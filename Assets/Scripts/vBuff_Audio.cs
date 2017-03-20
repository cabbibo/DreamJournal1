using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vBuff_Audio : MonoBehaviour {

	public vBuffUpdater updater;
	public AudioSourceTexture audioSource;

	Matrix4x4 matrix;
			
	// Use this for initialization
	void OnEnable() {
		if( updater == null){
			updater = GetComponent<vBuffUpdater>();
		}

		if( audioSource == null){
			audioSource = GetComponent<AudioSourceTexture>();
		}

		updater.OnBeforeDispatch += updateAudio;
	}

	void OnDisable(){
		updater.OnBeforeDispatch -= updateAudio;
	}


	void updateAudio(ComputeShader computeShader , int _kernel){

		computeShader.SetInt( "_AudioLength" , audioSource.size);
		computeShader.SetBuffer( _kernel , "audioBuffer" , audioSource._buffer );

	}
	

}
