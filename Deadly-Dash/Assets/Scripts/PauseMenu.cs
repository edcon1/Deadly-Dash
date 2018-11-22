using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool IsGamePause = false;
    public GameObject pauseMenuUI;

	// Use this for initialization
	void Start ()
    {
        foreach (Image ri in gameObject.GetComponentsInChildren<Image>())
        {
            if (ri.gameObject.name == "Resume" || ri.gameObject.name == "HighScore" || ri.gameObject.name == "Quit" || ri.gameObject.name == "Restart")
                ri.alphaHitTestMinimumThreshold = 0.5f;
        }
        pauseMenuUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsGamePause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
	}
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePause = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePause = true;
    }

    public void LoadHighScore()
    {
        SceneManager.LoadScene("HighScore");
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main scene");
        Time.timeScale = 1f;
    }
}


