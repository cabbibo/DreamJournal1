using UnityEngine;
using System.Collections;

public class tBuffer : MonoBehaviour {

	public Mesh m;

	public int triCount;
	public ComputeBuffer _buffer;	
	public int[] triangles;

	// Use this for initialization
	void Awake () {

		triangles =  m.triangles;
		triCount = m.triangles.Length;

		_buffer = new ComputeBuffer( triCount , sizeof(int) ); 
		_buffer.SetData(triangles);
	
	}

	void OnDisable(){
		ReleaseBuffer();
	}

	public void ReleaseBuffer(){
	  _buffer.Release(); 
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
