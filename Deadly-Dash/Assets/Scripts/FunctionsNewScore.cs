using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FunctionsNewScore : MonoBehaviour
{
    [Tooltip("How long it takes to completely fade the death screen, in seconds.")]
    public float fadeDuration = 5f;
    [Tooltip("New HighScore Menu.")]
    public GameObject menuNHS = null;
    [Tooltip("Game Over Menu.")]
    public GameObject menuGO = null;

    private InputField nameInput;
    private Image deathScreen;
    private float timer = 0;

	// Use this for initialization
	void Start ()
    {
        nameInput = GetComponentInChildren<InputField>();

        foreach (Text txt in gameObject.GetComponentsInChildren<Text>())
            if (txt.gameObject.name == "ScoreText")
                txt.text = GlobalScript.FinalScore.ToString() + " KM";

        foreach (RectTransform rect in gameObject.GetComponentsInChildren<RectTransform>())
            if (rect.gameObject.name == "DeathScreen")
                deathScreen = rect.gameObject.GetComponent<Image>();

        foreach (Image img in menuGO.GetComponentsInChildren<Image>())
            img.alphaHitTestMinimumThreshold = 0.5f;

        if (PlayerPrefs.GetFloat(GlobalScript.TableTag + GlobalScript.ScoreTag + 9, float.NaN) < GlobalScript.FinalScore)
            menuGO.SetActive(false);
        else
            menuNHS.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        timer = Mathf.Min(timer, fadeDuration);

        deathScreen.color = Color.Lerp(new Color(102f / 255f, 0, 0, 1), new Color(0, 0, 0, 0), timer / fadeDuration);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            Submit();
	}

    public void Submit()
    {
        if (string.IsNullOrEmpty(nameInput.text))
            GlobalScript.FinalPlayer = GlobalScript.DefaultName;
        else
            GlobalScript.FinalPlayer = nameInput.text;

        SceneManager.LoadScene("HighScore", LoadSceneMode.Single);
    }



    public void OnClickHighScore() { SceneManager.LoadScene("HighScore", LoadSceneMode.Single); }
    
    public void OnClickQuit() { SceneManager.LoadScene("Main Menu", LoadSceneMode.Single); }

    public void OnClickRestart() { SceneManager.LoadScene("Main scene", LoadSceneMode.Single); }
}
