using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* implements the leave game button */
public class Leave : MonoBehaviour
{
	/* on click, leave the game */
    public void LeaveGame(){
    	Application.Quit();
    }
}
