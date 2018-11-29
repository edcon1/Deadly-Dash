using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    public AudioSource engineRevAudio;

    private bool switchingScene = false;

 
    public void OnClickPlay()
    {
        if (switchingScene) return;
        StartCoroutine(StartGame());
    }

    public void OnClickHighScore()
    {
        if (switchingScene) return;
        SceneManager.LoadScene("HighScore", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }


    IEnumerator StartGame()
    {
        switchingScene = true;

        EngineRevAudioController controller = engineRevAudio.GetComponent<EngineRevAudioController>();
        controller.Test();

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Main scene", LoadSceneMode.Single);
        Time.timeScale = 1f;
        //engineRevAudio.Stop();
        switchingScene = false;
       

    }

}
