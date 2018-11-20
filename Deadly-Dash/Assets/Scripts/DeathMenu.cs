using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public GameObject DeathMenuUI;

    public static bool IsPlayerDead = false;

	// Use this for initialization
	void Start ()
    {
	 
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void triggerDeath()
    {
        IsPlayerDead = true;
    }
    public void Death()
    {
        DeathMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResetGame()
    {
        SceneManager.LoadScene("Main scene");
        IsPlayerDead = false;
        Time.timeScale = 1f;
    }
    public void HighScoreBTN()
    {
        SceneManager.LoadScene("HighScore");
    }
    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Damage")
        {
            Death();
        }
        Debug.Log("good");
    }
}
