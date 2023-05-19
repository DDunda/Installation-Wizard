using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("StartMenu");

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SceneManager.LoadScene("Level01");

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SceneManager.LoadScene("Level02");

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SceneManager.LoadScene("Level03");

        }

    }
}
