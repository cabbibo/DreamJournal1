	using UnityEngine;
using System.Collections;

public class vBuffer : MonoBehaviour {

	public Mesh m;

	public int vertCount;
	public ComputeBuffer _buffer;	
	public float[] vertValues;

	public Vector3[] vertices;
	public Vector3[] normals;
	public Vector2[] uvs;


	public int SIZE = 8;

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

	struct Vert{
		public float used;
	    public Vector3 pos;
	    public Vector3 vel;
	    public Vector3 nor;
	    public Vector2 uv;

	    public Vector3 targetPos;

	    public Vector3 debug;
	};



	private int vertStructSize = 1 + 3 + 3 + 3+2+3+3;

	// Use this for initialization
	void Awake () {

		threadX = 8;
		threadY = 8;
		threadZ = 8;

		strideX = 8;
		strideY = 8;
		strideZ = 8;

		//meshObject.GetComponent<SkinnedMeshRenderer>().BakeMesh( m );
		//Mesh m = meshObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		vertices = m.vertices;
		uvs = m.uv;
		normals = m.normals;

		vertCount = vertices.Length;

		print( "VERTCOUNT");
		print( vertCount);
		print( fullVertCount );


		_buffer = new ComputeBuffer( fullVertCount , vertStructSize * sizeof(float) );

		vertValues = new float[ vertStructSize * fullVertCount ];

		int index = 0;
		for( int i = 0; i < vertCount; i++ ){

			// used 
			vertValues[ index++ ] = 1;

			// positions
			vertValues[ index++ ] = vertices[i].x;
			vertValues[ index++ ] = vertices[i].y;
			vertValues[ index++ ] = vertices[i].z;

			// vel
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;

			// normals
			vertValues[ index++ ] = normals[i].x;
			vertValues[ index++ ] = normals[i].y;
			vertValues[ index++ ] = normals[i].z;

			// uvs
			vertValues[ index++ ] = 0;//uvs[i].x;
			vertValues[ index++ ] = 0;//uvs[i].y;


			// target pos
			vertValues[ index++ ] = vertices[i].x;
			vertValues[ index++ ] = vertices[i].y;
			vertValues[ index++ ] = vertices[i].z;


			// Debug
			vertValues[ index++ ] = 1;
			vertValues[ index++ ] = 0;
			vertValues[ index++ ] = 0;

		} 
		
		_buffer.SetData(vertValues);

	
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
