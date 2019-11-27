using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayScript : MonoBehaviour
{
    
    public void loadLevel1(){
    	SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    	SceneManager.UnloadScene(SceneManager.GetSceneByName("Menu"));
    }
}
