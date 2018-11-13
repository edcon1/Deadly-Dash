using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FunctionsHS : MonoBehaviour
{
    [Tooltip("Distance between the top of the screen and the title.")]
    public int titleSpacing = 5;
    [Tooltip("Distance between the bottom of the title and the top of the score table.")]
    public int tableSpacing = 10;
    [Tooltip("Spacing between each row of scores.")]
    public int rowSpacing = 15;
    [Tooltip("Shifts the position coloumn.")]
    public int shiftPositions = 0;
    [Tooltip("Shifts the name coloumn.")]
    public int shiftNames = 0;
    [Tooltip("Shifts the score coloumn.")]
    public int shiftScores = 0;
    [Tooltip("How long you have to hold the menu button down to reset the HS table.")]
    public float timeHeldToReset = 5;

    private Text posTemplate;
    private Text nameTemplate;
    private Text scoreTemplate;
    private Score[] tableHS = new Score[10];
    private List<Text> textArray = new List<Text>();
    private Text title;
    private float canvasHeight;
    private float canvasWidth;
    private string tableTag = "HS1_";
    private string nameTag = "player";
    private string scoreTag = "score";
    private float resetTimer = 0;
    private bool menuDown = false;

    // Use this for initialization
    void Start ()
    {
        title = GameObject.Find("TitleHS").GetComponent<Text>();
        posTemplate = GameObject.Find("PosText").GetComponent<Text>();
        nameTemplate = GameObject.Find("NameText").GetComponent<Text>();
        scoreTemplate = GameObject.Find("ScoreText").GetComponent<Text>();

        canvasHeight = gameObject.GetComponent<RectTransform>().rect.height;
        canvasWidth = gameObject.GetComponent<RectTransform>().rect.width;

        title.gameObject.transform.position = new Vector2(canvasWidth / 2, canvasHeight - titleSpacing);

        float checkExists = PlayerPrefs.GetFloat(tableTag + scoreTag + 0, float.NaN);

        if (float.IsNaN(checkExists))
            InitiateTable();
        else
            LoadTable();

        DrawTable();

        posTemplate.gameObject.SetActive(false);
        nameTemplate.gameObject.SetActive(false);
        scoreTemplate.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        SaveTable();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKeyDown(KeyCode.E) && Input.GetKeyDown(KeyCode.L))
        {
            InitiateTable();
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }

        if (menuDown == true)
            resetTimer += Time.deltaTime;

        if (resetTimer >= timeHeldToReset)
        {
            InitiateTable();
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }
    }

    private void DrawTable()
    {
        float spacing = posTemplate.fontSize + rowSpacing;
        float baseHeight = title.gameObject.transform.localPosition.y - title.rectTransform.rect.height - posTemplate.fontSize - tableSpacing;

        for (int i = 0; i < 10; ++i)
        {
            Text pos = Instantiate(posTemplate, transform);
            Text pla = Instantiate(nameTemplate, transform);
            Text sco = Instantiate(scoreTemplate, transform);

            pos.text = (i + 1) + ".";
            pos.transform.localPosition = new Vector3(pos.transform.localPosition.x + shiftPositions, baseHeight - spacing * i);

            pla.text = tableHS[i].playerName;
            pla.transform.localPosition = new Vector3(pla.transform.localPosition.x + shiftNames, baseHeight - spacing * i);

            sco.text = tableHS[i].playerScore.ToString() + " KM";
            sco.transform.localPosition = new Vector3(sco.transform.localPosition.x + shiftScores, baseHeight - spacing * i);

            textArray.Add(pos);
            textArray.Add(pla);
            textArray.Add(sco);
        }
    }

    private void InitiateTable()
    {
        for (int i = 0; i < 10; ++i)
        {
            PlayerPrefs.SetString(tableTag + nameTag + i, "Player" + i);
            PlayerPrefs.SetFloat(tableTag + scoreTag + i, 0);

            tableHS[i] = new Score("Player" + i, 0);
        }
    }

    private void LoadTable()
    {
        for (int i = 0; i < 10; ++i)
        {
            string n = PlayerPrefs.GetString(tableTag + nameTag + i, "ERROR");
            float s = PlayerPrefs.GetFloat(tableTag + scoreTag + i, float.NaN);
            tableHS[i] = new Score(n, s);
        }
    }

    private void SaveTable()
    {
        for (int i = 0; i < 10; ++i)
        {
            PlayerPrefs.SetString(tableTag + nameTag + i, tableHS[i].playerName);
            PlayerPrefs.SetFloat(tableTag + scoreTag + i, tableHS[i].playerScore);
        }
    }

    struct Score
    {
        public Score(string name, float score)
        {
            playerName = name;
            playerScore = Mathf.Round(score);
        }

        public string playerName;
        public float playerScore;
    }

    public void OnClickMenuButton()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void OnButtonDown()
    {
        menuDown = true;
    }
}
