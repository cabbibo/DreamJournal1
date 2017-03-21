﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDreamTriangles : MonoBehaviour {

	public Material m;
	public dreamVertBuffer vBuf;

	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnRenderObject(){

		m.SetPass(0);
		m.SetBuffer( "_vertBuffer", vBuf._buffer );
		Graphics.DrawProcedural(MeshTopology.Triangles, vBuf.fullVertCount );

	}
}