using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* manage the level selector menu*/
public class LevelSelectManager : MonoBehaviour
{

	public GameObject levelSelectMenu;
	public GameObject menu;

	/* indicate the build index of the level to declenche on click for button 1 */
	private int levelIndicator;

	void Awake(){
		levelIndicator = 2;
	}

	/* set the text on the buttons */
	public void initiateMenu(){
		Scene scene;

		menu.SetActive(false);
		levelSelectMenu.SetActive(true);

		int i = 0;

		for(; levelIndicator + i < SceneManager.sceneCountInBuildSettings && i < 4; ++i){
			levelSelectMenu.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = (levelIndicator - 1 + i).ToString();
			levelSelectMenu.transform.GetChild(i).gameObject.SetActive(true);
		}

		for(; i < 4; ++i) levelSelectMenu.transform.GetChild(i).gameObject.SetActive(false);

		if(levelIndicator + 4 <= SceneManager.sceneCountInBuildSettings) levelSelectMenu.transform.GetChild(5).gameObject.SetActive(true);
		else levelSelectMenu.transform.GetChild(5).gameObject.SetActive(false);

		if(levelIndicator - 4 >= 2) levelSelectMenu.transform.GetChild(4).gameObject.SetActive(true);
		else levelSelectMenu.transform.GetChild(4).gameObject.SetActive(false);
	}

	/* on click to show the 4 nexts choise */
	public void nextMenu(){
		levelIndicator += 4;
		initiateMenu();
	}

	/* same but previous */
	public void prevMenu(){
		levelIndicator -= 4;
		initiateMenu();
	}

	/* on click, back to main menu */
	public void backToMenu(){
		levelIndicator = 2;
		menu.SetActive(true);
		levelSelectMenu.SetActive(false);
	}

	/* the 4 next load the level corresponding */
	public void loadLevel1(){
    	SceneManager.LoadScene(levelIndicator, LoadSceneMode.Additive);
    	SceneManager.UnloadScene(SceneManager.GetSceneByName("Menu"));
    }

    public void loadLevel2(){
    	SceneManager.LoadScene(levelIndicator + 1, LoadSceneMode.Additive);
    	SceneManager.UnloadScene(SceneManager.GetSceneByName("Menu"));
    }

    public void loadLevel3(){
    	SceneManager.LoadScene(levelIndicator + 2, LoadSceneMode.Additive);
    	SceneManager.UnloadScene(SceneManager.GetSceneByName("Menu"));
    }

    public void loadLevel4(){
    	SceneManager.LoadScene(levelIndicator + 3, LoadSceneMode.Additive);
    	SceneManager.UnloadScene(SceneManager.GetSceneByName("Menu"));
    }
}
