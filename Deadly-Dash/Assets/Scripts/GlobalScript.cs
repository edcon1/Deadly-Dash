using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalScript
{
    public static string FinalPlayer = "";
    public static float WorldSpeed = 10;

    private static string tableTag = "HS1_";
    private static string nameTag = "player";
    private static string scoreTag = "score";
    private static string defaultName = "RandomSanta";
    private static float defaultWorldSpeed = 10;
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

    public static string DefaultName
    {
        get { return defaultName; }
        private set { defaultName = value; }
    }

    public static void SetDefaultWorldSpeed()
    {
        WorldSpeed = defaultWorldSpeed;
    }
}
