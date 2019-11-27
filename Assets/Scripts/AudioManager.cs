using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* manage the sound for the game */
public class AudioManager : MonoBehaviour
{
	private bool soundOn;

	void Start(){
		soundOn = true;
	}

    /* turn the sound on, called by SoundButton script */
    public void turnSoundOn(){
    	soundOn = true;
    	GetComponent<AudioSource>().Play();
    }

    /* turn the soun off */
    public void turnSoundOf(){
    	soundOn = false;
    	GetComponent<AudioSource>().Stop();
    }

    public bool getSoundOn(){
    	return soundOn;
    }
}
