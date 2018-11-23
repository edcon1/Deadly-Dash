using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem 
{
    private Text scoreDisplay;
    private float score = 0;
    private int truncate;

    public ScoreSystem(Text scoreTxt) { scoreDisplay = scoreTxt; }

    public float Score
    {
        get { return score; }
        private set { score = value; }
    }
	
	// Update is called once per frame
	public void Update ()
    {
        Score += GlobalScript.WorldSpeed * Time.deltaTime;
        truncate = (int)Score;
        scoreDisplay.text = truncate.ToString() + " KM";
	}
}
