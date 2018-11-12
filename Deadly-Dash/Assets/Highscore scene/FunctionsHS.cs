using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionsHS : MonoBehaviour
{
    [Tooltip("Spacing between the rows of the HS table.")]
    public int tableSpacing = 5;

    private List<Text> tableHS = new List<Text>();
    private Text title;
    private float canvasHeight;
    private float canvasWidth;

	// Use this for initialization
	void Start ()
    {
        title = GameObject.Find("TitleHS").GetComponent<Text>();
        canvasHeight = gameObject.GetComponent<RectTransform>().rect.height;
        canvasWidth = gameObject.GetComponent<RectTransform>().rect.width;

        title.gameObject.transform.position = new Vector2(canvasWidth / 2, canvasHeight);
	}
	
	// Update is called once per frame
	void Update () {}
}
