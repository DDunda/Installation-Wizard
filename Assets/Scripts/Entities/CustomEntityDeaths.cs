using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomEntityDeaths : MonoBehaviour
{

    public void PlayerDeath()
	{
		GameObject.Find("GameManager").GetComponent<GlobalStatTransfer>().UpdateStats();
        SceneManager.LoadScene("GameOver");
	}

    public void NextLevel()
	{
		GameObject.Find("GameManager").GetComponent<GlobalStatTransfer>().UpdateStats();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

    public void LoadScene(string sceneName)
	{
		GameObject.Find("GameManager").GetComponent<GlobalStatTransfer>().UpdateStats();
		if (sceneName == null)
			Debug.Log("<color=orange>"+gameObject.name+": No Scene Name Was given for LoadScene function!</color>");
		SceneManager.LoadScene(sceneName); //load a scene
	}
}
