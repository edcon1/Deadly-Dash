using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class DeathMenu : MonoBehaviour
{

    public GameObject DeathMenuUI;
    public static bool IsPlayerDead = false;

    private ScoreSystem scoreSys = null;
    //private RectTransform deathPanel = null;

    // Use this for initialization
    void Start ()
    {
        foreach (Image ri in gameObject.GetComponentsInChildren<Image>())
            if (ri.gameObject.name == "Restart" || ri.gameObject.name == "HighScore" || ri.gameObject.name == "Quit")
                ri.alphaHitTestMinimumThreshold = 0.5f;

        foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            
            foreach (Transform child in go.GetComponentsInChildren<Transform>())
            {
                if (scoreSys == null && child.name == "KMsText")
                    scoreSys = new ScoreSystem(child.GetComponent<Text>());

                //if (deathPanel == null && child.name == "DeathPanel")
                //    deathPanel = child.GetComponent<RectTransform>();
            }
        }

        DeathMenuUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        scoreSys.Update();
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
            GlobalScript.FinalScore = scoreSys.Score;
            SceneManager.LoadScene("NewHScore",  LoadSceneMode.Single);
        }
    }
}
