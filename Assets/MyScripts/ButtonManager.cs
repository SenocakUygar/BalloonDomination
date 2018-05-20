using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void newGameBtn(){
		SceneManager.LoadScene ("BalloonDomination");
	}

	public void exitGameBtn(){
		Application.Quit();
	}

    public void menuBtn()
    {
        SceneManager.LoadScene("Menu");
    }
}
