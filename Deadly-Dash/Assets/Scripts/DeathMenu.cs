using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class DeathMenu : MonoBehaviour
{

    public GameObject DeathMenuUI;
    public Text nextHS;
    public static bool IsPlayerDead = false;

    private List<float> highScores = new List<float>();
    private ScoreSystem scoreSys = null;
    //private RectTransform deathPanel = null;

    // Use this for initialization
    void Start ()
    {
        foreach (Image ri in gameObject.GetComponentsInChildren<Image>())
            if (ri.gameObject.name == "Restart" || ri.gameObject.name == "HighScore" || ri.gameObject.name == "Quit")
                ri.alphaHitTestMinimumThreshold = 0.5f;

        foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
            foreach (Transform child in go.GetComponentsInChildren<Transform>())
                if (scoreSys == null && child.name == "KMsText")
                    scoreSys = new ScoreSystem(child.GetComponent<Text>());

        for (int i = 0; i < 10; ++i)
            highScores.Add(PlayerPrefs.GetFloat(GlobalScript.TableTag + GlobalScript.ScoreTag + i, float.NaN));

        DeathMenuUI.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        scoreSys.Update();

        for (int i = 0; i < 10; ++i)
            if (scoreSys.Score > highScores[i])
            {
                if (i <= 0)
                    nextHS.text = "New record for Santa Kind!: " + (int)scoreSys.Score + " KM";
                else
                    nextHS.text = "Next stop: " + highScores[i - 1] + " KM";

                break;
            }
            else
                nextHS.text = "Next stop: " + highScores[9] + " KM";
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
            GlobalScript.SetDefaultWorldSpeed();
            GlobalScript.FinalScore = scoreSys.Score;
            SceneManager.LoadScene("NewHScore",  LoadSceneMode.Single);
        }
    }
}
