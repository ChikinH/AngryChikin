using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundButton : MonoBehaviour
{
	public Button button;
	public Sprite soundOnSprite;
	public Sprite soundOfSprite;

	void Awake(){
		if(!SceneManager.GetSceneByName("MusicScene").isLoaded) SceneManager.LoadScene("MusicScene", LoadSceneMode.Additive);
	}
    
    public void turnSound(){
    	if(!SceneManager.GetSceneByName("MusicScene").GetRootGameObjects()[0].GetComponent<AudioManager>().getSoundOn()){
    		button.image.sprite = soundOnSprite;
    		SceneManager.GetSceneByName("MusicScene").GetRootGameObjects()[0].GetComponent<AudioManager>().turnSoundOn();
    	}
    	else{
    		button.image.sprite = soundOfSprite;
    		SceneManager.GetSceneByName("MusicScene").GetRootGameObjects()[0].GetComponent<AudioManager>().turnSoundOf();
    	}
    }
}
