using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject inGameMenu;
	public GameObject pauseMenu;

	void Awake(){
		HidePauseMenu();
	}

    public void Pause(){
    	Time.timeScale = 0f;
    	DisplayPauseMenu();
    }

    public void unPause(){
    	Time.timeScale = 1f;
    	HidePauseMenu();
    }

    public void Leave(){
    	unPause();
    	Scene active = SceneManager.GetActiveScene();
    	SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    	SceneManager.UnloadScene(active);
    }

    public void Reset(){
    	unPause();
    	Scene active = SceneManager.GetActiveScene();
    	SceneManager.UnloadSceneAsync(active);
    	SceneManager.LoadScene(active.buildIndex, LoadSceneMode.Additive);
    	
    }

    void DisplayPauseMenu(){
    	pauseMenu.SetActive(true);
    	inGameMenu.SetActive(false);
    }

    void HidePauseMenu(){
    	pauseMenu.SetActive(false);
    	inGameMenu.SetActive(true);
    }


}
