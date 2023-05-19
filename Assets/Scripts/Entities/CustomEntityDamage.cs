using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomEntityDamage : MonoBehaviour
{
	private GameObject gameManager;
	private GlobalStatTransfer globalStat;

	void Start()
	{
		gameManager = GameObject.Find("GameManager");
		globalStat = gameManager.GetComponent<GlobalStatTransfer>();
	}

    public void PlayerDamage()
	{
       globalStat.localDamageDealt = 3;
	}

    public void EnemyDamage()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
