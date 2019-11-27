using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private bool soundOn;

	void Start(){
		soundOn = true;
	}

    public void turnSoundOn(){
    	soundOn = true;
    	GetComponent<AudioSource>().Play();
    }

    public void turnSoundOf(){
    	soundOn = false;
    	GetComponent<AudioSource>().Stop();
    }

    public bool getSoundOn(){
    	return soundOn;
    }
}
