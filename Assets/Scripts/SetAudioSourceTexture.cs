using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAudioSourceTexture : MonoBehaviour {

	public AudioSourceTexture ast;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Renderer>().material.SetTexture("_MainTex", ast.AudioTexture);
	}
}
