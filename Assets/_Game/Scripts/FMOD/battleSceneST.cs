using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleSceneST : MonoBehaviour {

    //UI
    [FMODUnity.EventRef]
	//"musicEvent" stores event path
	public string musicEvent;
	
	//Event instance
	FMOD.Studio.EventInstance music;
	
	//intensity VARIABLE HERE (substitute number)
	int intensityPar = 1;

private void OnEnable()
    {
		//Instances "music" and enables it
		music = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        music.start();
		//Attaches instance to object
		FMODUnity.RuntimeManager.AttachInstanceToGameObject(music, GetComponent<Transform>(), GetComponent<Rigidbody>());

		//starts "intensity" as 0
		music.setParameterValue("intensity", 0);
    }

    private void OnDisable()
    {
		//Releases "music" resources
		music.release();
    }
	
	void Update () {
		//uncomment following:
		//intensityPar = new time variable thing <----------
	}
}
