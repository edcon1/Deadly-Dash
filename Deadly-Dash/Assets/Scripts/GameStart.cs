using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void OnClickPlay()
    {
        SceneManager.LoadScene("Main scene", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void OnClickHighScore()
    {
        SceneManager.LoadScene("HighScore", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
