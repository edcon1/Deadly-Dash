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
    public float timeToDecelerate = 2;
    public int? initialBlanks = 10;

    private List<float> highScores = new List<float>();
    private ScoreSystem scoreSys = null;
    private ProceduralGenerator pgInstance = null;
    private ButtonMovement bmInstance = null;
    private GameObject startPoint = null;
    private bool introInitiated = false;
    private float? deceleratioThreshold;
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

        scoreSys.Start();

        GameObject[] rootGameObjects = gameObject.scene.GetRootGameObjects();
        foreach (GameObject go in rootGameObjects)
        {
            if (pgInstance == null)
            {
                pgInstance = go.GetComponentInChildren<ProceduralGenerator>();
            }
            if (bmInstance == null)
            {
                bmInstance = go.GetComponentInChildren<ButtonMovement>();
            }

            if (pgInstance != null && bmInstance != null) { break; }
        }

        DeathMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (initialBlanks != null && startPoint == null && pgInstance.blankCount <= initialBlanks)
        {
            startPoint = pgInstance.LoadedPrefabs.Last.Value.obj;
            initialBlanks = null;
        }
        else if (startPoint != null)
        {
            GlobalScript.WorldSpeed = bmInstance.maxSpeed;
        }
        else if (initialBlanks == null && introInitiated == false)
        {
            if (deceleratioThreshold == null)
            {
                deceleratioThreshold = Time.time + timeToDecelerate;
            }

            GlobalScript.WorldSpeed = Mathf.Lerp(bmInstance.maxSpeed, GlobalScript.DefaultWorldSpeed, Time.time / (float)deceleratioThreshold);

            if (GlobalScript.WorldSpeed <= GlobalScript.DefaultWorldSpeed)
            {
                introInitiated = true;
            }
        }
        else if (introInitiated == true)
        {
            scoreSys.Update();
        }


        for (int i = 0; i < 10; ++i)
            if (scoreSys.Score > highScores[i])
            {
                if (i <= 0)
                    nextHS.text = "New record for Santa Kind!: " + (int)scoreSys.Score + " KM";
                else
                    nextHS.text = highScores[i - 1] + " KM";

                break;
            }
            else
                nextHS.text = highScores[9] + " KM";
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
