using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DREAMJOURNAL : MonoBehaviour {

	public GameObject CameraRig;

	public GameObject Artifact;

	public Material material;

	public Material rayMaterial;

	public dreamVertBuffer  dreamBuffer;

	public AudioSourceTexture audioSourceTexture;
	public AudioSource audio;

	public Texture roomTexture;
	public HumanBuffer humanBuffer;

	public AudioClip pageTurnClip;

	public anchorBuffer roomBuffer;
	public anchorBuffer spacePuppyBuffer;
	public anchorBuffer starBuffer;

	public float roomAlive;
	public float starsAlive;
	public float spacePuppyAlive;

	public Section[] sections;

	//public int sections;

	public dreamVertUpdater updater;

	public int currentSectionID = -1;
	public Section currentSection;

	public float raysOn = 0;

	public float smoothedSection = 0;

	void OnEnable() {



		Artifact.GetComponent<Artifact>().MakeDead();

		roomBuffer.PopulateBuffer();
		spacePuppyBuffer.PopulateBuffer();
		starBuffer.PopulateBuffer();
		
		updater.OnBeforeDispatch += updateDream;

		currentSectionID = -1;
		NextSection();

		updater.SET();




	}

	void OnDisable(){

		roomBuffer.ReleaseBuffer();
		spacePuppyBuffer.ReleaseBuffer();
		starBuffer.ReleaseBuffer();
		
		updater.OnBeforeDispatch -= updateDream;

	}

	void updateDream(ComputeShader computeShader , int _kernel){

		computeShader.SetBuffer( _kernel , "roomBuffer" , roomBuffer._buffer );
		computeShader.SetBuffer( _kernel , "starBuffer" , starBuffer._buffer );
		computeShader.SetBuffer( _kernel , "spacePuppyBuffer" , spacePuppyBuffer._buffer );
		computeShader.SetBuffer( _kernel , "audioBuffer" , audioSourceTexture._buffer );

		computeShader.SetInt("roomLength", roomBuffer.numVerts);
		computeShader.SetInt("starLength", starBuffer.numVerts);
		computeShader.SetInt("spacePuppyLength", spacePuppyBuffer.numVerts);

		computeShader.SetInt( "_AudioLength", audioSourceTexture.size );
		computeShader.SetVector( "_SpacePuppyScale", spacePuppyBuffer.gameObject.transform.localScale );



		computeShader.SetVector( "_ArtifactPos" , Artifact.transform.position );
		//print( spacePuppyBuffer.gameObject.transform.position );
		computeShader.SetVector( "_SpacePuppyPos", spacePuppyBuffer.gameObject.transform.position );
		computeShader.SetVector( "_CameraRigPos", CameraRig.transform.position );

		computeShader.SetInt( "_AudioLength", audioSourceTexture.size );

		SetMatrix( Artifact.transform.localToWorldMatrix , "_ArtifactTransform" , computeShader );
		SetMatrix( spacePuppyBuffer.gameObject.transform.worldToLocalMatrix , "_SpacePuppyTransform" , computeShader );



		computeShader.SetInt("sectionCanIncrease" , currentSection.sectionCanIncrease);
		computeShader.SetInt("sectionMustIncrease" , currentSection.sectionMustIncrease);
	
		computeShader.SetFloat("sectionIncreaseSpeed" , currentSection.sectionIncreaseSpeed );
		computeShader.SetFloat("sectionIncreaseRadius" , currentSection.sectionIncreaseRadius );
		computeShader.SetFloat("sectionIncreaseMax" , currentSection.sectionIncreaseMax );
		computeShader.SetFloat("sectionIncreaseNoise" , currentSection.sectionIncreaseNoise );

	}

	void SetMatrix(Matrix4x4 m , string  name , ComputeShader computeShader  ){
		
		//Matrix4x4 matrix = t.localToWorldMatrix; 
		float[] matrixFloats = new float[] 
		{ 
		 m[0,0],  m[1, 0],  m[2, 0],  m[3, 0], 
		 m[0,1],  m[1, 1],  m[2, 1],  m[3, 1], 
		 m[0,2],  m[1, 2],  m[2, 2],  m[3, 2], 
		 m[0,3],  m[1, 3],  m[2, 3],  m[3, 3] 
		}; 

		computeShader.SetFloats( name , matrixFloats );

	}




	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("space")){
			NextSection();
		}

		if( currentSectionID >= 5 ){
			//print("ss");
		}
		
	}
	void FixedUpdate(){

		// move camera away
		if( currentSectionID >= 6){
			if( CameraRig.transform.position.z < 100 ){
				CameraRig.transform.position += Vector3.forward * .1f;
			}

		}

		if( currentSectionID >= 3 ){
			raysOn += .01f;
			raysOn = Mathf.Clamp( raysOn , 0 , 1 );
		}


		smoothedSection = Mathf.Lerp( smoothedSection , (float)currentSectionID , .5f );

	}






	public void NextSection(){
		print( "newSection");
		currentSectionID ++;

		if( currentSectionID == 1 ){
			print( "ya firssts");

			Artifact.GetComponent<Artifact>().MakeAlive();
		}

		if( currentSectionID == 2 ){

			Artifact.GetComponent<Artifact>().MoveUp();

		}

		if( currentSectionID == 6 ){
			audio.Play();
		}

		if( currentSectionID == 9 ){
			Artifact.GetComponent<Artifact>().MakeLight();
		}
		currentSection = sections[currentSectionID];
	}


	void OnRenderObject(){

		// Render the Triangles
		material.SetPass(0);
		material.SetFloat( "smoothedSection" , smoothedSection);
		material.SetVector( "ArtifactPos" , Artifact.transform.position );
		material.SetBuffer( "_vertBuffer", dreamBuffer._buffer );
		Graphics.DrawProcedural(MeshTopology.Triangles, dreamBuffer.fullVertCount );

	


		rayMaterial.SetPass(0);
		rayMaterial.SetBuffer( "_vertBuffer", dreamBuffer._buffer );
		rayMaterial.SetBuffer( "_artifactBuffer", spacePuppyBuffer._buffer );

		rayMaterial.SetVector( "CenterPos" , Artifact.transform.position );
		rayMaterial.SetFloat( "raysOn" , raysOn );

		float totalLines = (float)(((float)spacePuppyBuffer.numVerts / 16) * 2 * Mathf.Clamp(raysOn * 10,0,1));

		Graphics.DrawProcedural(MeshTopology.Lines, (int)totalLines );


		// Render the Rays



	}




}
