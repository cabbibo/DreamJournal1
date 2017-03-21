using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dreamVertBuffer : MonoBehaviour {

	public ComputeBuffer _buffer;

	

	public int threadX;
	public int threadY;
	public int threadZ;
	public int strideX;
	public int strideY;
	public int strideZ;
	
	private int gridX { get { return threadX * strideX; } }
	private int gridY { get { return threadY * strideY; } }
	private int gridZ { get { return threadZ * strideZ; } }
	
	public int fullVertCount { get { return gridX * gridY * gridZ; } }

	private float[] vertValues;

	struct Vert{
	  public Vector3 pos;
	  public Vector3 vel;
	  public Vector3 nor;
	  public Vector2 uv;
	  public Vector3 targetPos;
	  public Vector3 ids;
	  public float 	 section;
	  public Vector3 debug;
	};



	private int vertStructSize = 3+3+3+2+3+3+1+3;
	
	// Use this for initialization
	void OnEnable() {


	

		_buffer = new ComputeBuffer( fullVertCount , vertStructSize * sizeof(float) );

		vertValues = new float[ vertStructSize * fullVertCount ];

		int index = 0;
		for( int i = 0; i < fullVertCount; i++ ){


			int triID = i / 3;

			Random.seed = triID;

			float centerX = Random.Range(-.99f,.99f);
			float centerY = Random.Range(-.99f,.99f);
		

			// positions
			vertValues[ index++ ] = 10 * centerX;
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 10 * centerY;



			// vel
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;

			// normals
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;

			// uvs
			vertValues[ index++ ] = 0;//uvs[i].x;
			vertValues[ index++ ] = 0;//uvs[i].y;


			// target pos
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;

			// Ids
			// Here we need to get the other 3 ids of the 
			// verts in the vert buffer, so we can pull towards 
			// each other in the compute shader!
			vertValues[ index++ ] = ((i/3)*3)+(i+0)%3;
			vertValues[ index++ ] = ((i/3)*3)+(i+1)%3;
			vertValues[ index++ ] = ((i/3)*3)+(i+2)%3;

			//section
			vertValues[ index++ ] = 0;


			// Debug
			vertValues[ index++ ] = 1;
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;

		} 
		
		_buffer.SetData(vertValues);

	
	}

	void OnDisable(){
		_buffer.Release(); 
	}

	// Update is called once per frame
	void FixedUpdate () {
		
	}
}
