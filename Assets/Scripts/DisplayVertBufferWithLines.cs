using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayVertBufferWithLines : MonoBehaviour {

	public Material m;
	public tBuffer  tBuf;
	public vBuffer  vBuf;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//print("hmm");
		
	}

	void OnRenderObject(){

		//print("ss");
		m.SetPass(0);

		m.SetBuffer( "_vertBuffer", vBuf._buffer );
		m.SetBuffer( "_triBuffer", tBuf._buffer );

		//Graphics.DrawProcedural(MeshTopology.Points, vBuf.vertCount );
		Graphics.DrawProcedural(MeshTopology.Lines, tBuf.triCount * 2 );


	}
}
