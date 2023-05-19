using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour 
{
	public void LoadScene(string sceneName)
	{
		if (sceneName == null)
			Debug.Log("<color=orange>"+gameObject.name+": No Scene Name Was given for LoadScene function!</color>");
		SceneManager.LoadScene(sceneName); //load a scene
	}

	public void QuitGame()
	{
		Application.Quit();
		Debug.Log("Quit!");
	}
	//not actually used but here when needed for easy access
	//private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape)) {
    //        //SceneManager.LoadScene("StartMenu");
    //        Application.Quit();
	//	    Debug.Log("Quit!");
    //    }
    //}
}
