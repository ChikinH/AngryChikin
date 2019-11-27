using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayScript : MonoBehaviour
{
    /* on click load the first level */
    public void loadLevel1(){
    	SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    	SceneManager.UnloadScene(SceneManager.GetSceneByName("Menu"));
    }
}
