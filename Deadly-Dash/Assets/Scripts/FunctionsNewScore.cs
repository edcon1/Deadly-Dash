using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FunctionsNewScore : MonoBehaviour
{
    private InputField nameInput;

	// Use this for initialization
	void Start ()
    {
        nameInput = GetComponentInChildren<InputField>();

        foreach (Text txt in gameObject.GetComponentsInChildren<Text>())
            if (txt.gameObject.name == "ScoreText")
                txt.text = GlobalScript.FinalScore.ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
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
}
