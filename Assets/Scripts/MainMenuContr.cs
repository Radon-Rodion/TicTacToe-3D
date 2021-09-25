using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuContr : MonoBehaviour
{
    public void startSingleplayer(){
		FieldControl.ai = true;
		SceneManager.LoadScene(1);
	}
	
	public void startMultiplayer(){
		FieldControl.ai = false;
		SceneManager.LoadScene(1);
	}
	
	public void restartGame(){
		SceneManager.LoadScene(1);
	}
	
	public void showSettings(){
		SceneManager.LoadScene(2);
	}
	
	public void mainMenu(){
		SceneManager.LoadScene(0);
	}
}
