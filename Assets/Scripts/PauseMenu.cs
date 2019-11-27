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

    /* set the game in pause and display a menu */
    public void Pause(){
    	Time.timeScale = 0f;
    	DisplayPauseMenu();
    }

    /* un pause the game */
    public void unPause(){
    	Time.timeScale = 1f;
    	HidePauseMenu();
    }

    /* going back to main menu */
    public void Leave(){
    	unPause();
    	Scene active = SceneManager.GetActiveScene();
    	SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    	SceneManager.UnloadScene(active);
    }


    /* reset the level */
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
