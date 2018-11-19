using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [Tooltip("This generator will spawn a random object from this list.")]
    public GameObject[] objectArray;

    private System.Random randIndex = new System.Random();

	// Use this for initialization
	void Start ()
    {
		foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
            if (t.gameObject.name.Contains("Node"))
            {
                GameObject newObj = Instantiate(objectArray[randIndex.Next(0, objectArray.Length)], gameObject.transform);
                newObj.transform.position = t.transform.position;
                newObj.transform.rotation *= Quaternion.Euler(0, 180, 0);
            }
	}
}
