using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalScript
{ 
    private static string tableTag = "HS1_";
    private static string nameTag = "player";
    private static string scoreTag = "score";
    private static float finalScore = float.NaN;

    public static string TableTag
    {
        get { return tableTag; }
        private set { tableTag = value; }
    }

    public static string NameTag
    {
        get { return nameTag; }
        private set { nameTag = value; }
    }

    public static string ScoreTag
    {
        get { return scoreTag; }
        private set { scoreTag = value; }
    }

    public static float FinalScore
    {
        get { return (Mathf.Round(finalScore)); }
        set { finalScore = value; }
    }
}
