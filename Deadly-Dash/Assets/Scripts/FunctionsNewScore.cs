using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FunctionsNewScore : MonoBehaviour
{
    [Tooltip("How long it takes to fade from white flash to crimson.")]
    public float fadeDuration = 1f;
    [Tooltip("How long it takes to for the fade to end.")]
    public float fadeEndDuration = 2f;
    [Tooltip("How long you are staring at a dead Santa for.")]
    public float deadSanta = 2f;
    [Tooltip("Menu fade in duration.")]
    public float menuFadeDuration = 1f;
    [Tooltip("New HighScore Menu.")]
    public GameObject menuNHS = null;
    [Tooltip("Game Over Menu.")]
    public GameObject menuGO = null;
    public GameObject PanelNHSGO = null;

    private CanvasGroup cGroup;
    private InputField nameInput;
    private Image deathScreen;
    private float timer = 0;
    private int switchCounter = 0;

	// Use this for initialization
	void Start ()
    {
        cGroup = GetComponentInChildren<CanvasGroup>();
        nameInput = GetComponentInChildren<InputField>();

        foreach (Text txt in gameObject.GetComponentsInChildren<Text>())
        {
            if (txt.gameObject.name == "ScoreGO")
            {
                txt.text = GlobalScript.FinalScore.ToString() + " KM";
            }

            if (txt.gameObject.name == "ScoreNHS")
            {
                txt.text = GlobalScript.FinalScore.ToString() + " KM";
            }
        }

        foreach (RectTransform rect in gameObject.GetComponentsInChildren<RectTransform>())
        {
            if (rect.gameObject.name == "DeathScreen")
            {
                deathScreen = rect.gameObject.GetComponent<Image>();
            }
        }

        foreach (Image img in menuGO.GetComponentsInChildren<Image>())
        {

            img.alphaHitTestMinimumThreshold = 0.5f;
        }
        foreach (Image img in menuNHS.GetComponentsInChildren<Image>())
        {

            img.alphaHitTestMinimumThreshold = 0.5f;
        }

        if (PlayerPrefs.GetFloat(GlobalScript.TableTag + GlobalScript.ScoreTag + 9, float.NaN) < GlobalScript.FinalScore)
        {
            menuGO.SetActive(false);
        }
        else
        {
            menuNHS.SetActive(false);
        }

        cGroup.alpha = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        switch(switchCounter)
        {
            case 0:
                {
                    deathScreen.color = Color.Lerp(new Color(1, 1, 1, 1), new Color(102f / 255f, 0, 0, 1), timer / fadeDuration);

                    UpdateFadeIn(fadeDuration);
                    break;
                }
            case 1:
                {
                    deathScreen.color = Color.Lerp(new Color(102f / 255f, 0, 0, 1), new Color(0, 0, 0, 0), timer / fadeEndDuration);

                    UpdateFadeIn(fadeEndDuration);
                    break;
                }
            case 2:
                {
                    UpdateFadeIn(deadSanta);
                    break;
                }
            case 3:
                {
                    cGroup.alpha = timer / menuFadeDuration;

                    UpdateFadeIn(menuFadeDuration);
                    break;
                }
            default: { break; }
        }

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



    private void UpdateFadeIn(float duration)
    {
        timer += Time.deltaTime;

        if (timer > duration)
        {
            ++switchCounter;
            timer = 0;
        }
    }



    public void OnClickHighScore() { SceneManager.LoadScene("HighScore", LoadSceneMode.Single); }
    
    public void OnClickQuit() { SceneManager.LoadScene("Main Menu", LoadSceneMode.Single); }

    public void OnClickRestart() { SceneManager.LoadScene("Main scene", LoadSceneMode.Single); }
}
