using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DREAMJOURNAL : MonoBehaviour {

	public GameObject CameraRig;

	public GameObject Artifact;

	public Material material;

	public Material rayMaterial;

	public dreamVertBuffer  dreamBuffer;

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

		computeShader.SetInt("roomLength", roomBuffer.numVerts);
		computeShader.SetInt("starLength", starBuffer.numVerts);
		computeShader.SetInt("spacePuppyLength", spacePuppyBuffer.numVerts);


		computeShader.SetInt("sectionCanIncrease" , currentSection.sectionCanIncrease);
		computeShader.SetInt("sectionMustIncrease" , currentSection.sectionMustIncrease);
	
		computeShader.SetFloat("sectionIncreaseSpeed" , currentSection.sectionIncreaseSpeed );
		computeShader.SetFloat("sectionIncreaseRadius" , currentSection.sectionIncreaseRadius );
		computeShader.SetFloat("sectionIncreaseMax" , currentSection.sectionIncreaseMax );
		computeShader.SetFloat("sectionIncreaseNoise" , currentSection.sectionIncreaseNoise );

	}



	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("space")){
			NextSection();
		}

		if( currentSectionID >= 5 ){
			print("ss");
		}
		
	}
	void FixedUpdate(){

		// move camera away
		if( currentSectionID == 5){
			CameraRig.transform.position += Vector3.forward * .02f;
		}

		if( currentSectionID >= 3 ){
			raysOn += .01f;
			raysOn = Mathf.Clamp( raysOn , 0 , 1 );
		}


		smoothedSection = Mathf.Lerp( smoothedSection , (float)currentSectionID , .1f );

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

		Graphics.DrawProcedural(MeshTopology.Lines, (spacePuppyBuffer.numVerts / 16) * 2 );


		// Render the Rays



	}




}
