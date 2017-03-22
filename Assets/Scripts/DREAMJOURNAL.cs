using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DREAMJOURNAL : MonoBehaviour {

	public GameObject CameraRig;

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

	void OnEnable() {

		roomBuffer.PopulateBuffer();
		spacePuppyBuffer.PopulateBuffer();
		starBuffer.PopulateBuffer();
		
		updater.OnBeforeDispatch += updateDream;

		currentSectionID = -1;
		NextSection();
		NextSection();
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


	void OnRenderObject(){
		//print("hello");
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


	}



	void NextSection(){
		print( "newSection");
		currentSectionID ++;
		currentSection = sections[currentSectionID];
	}



}
