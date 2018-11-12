using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    [Tooltip("This sets the speed of the map.")]
    public float tileSpeed = 10;
    public GameObject[] tileArray;

    private LinkedList<GameObject> loadedPrefabs = new LinkedList<GameObject>();

    private GameObject nextObject;
    private GameObject lastObject;
    private float spawnZone;
    private System.Random rand = new System.Random();

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (lastObject != null && lastObject.transform.position.z <= spawnZone)
            SpawnTile();
        else if (lastObject == null)
        {
            int randIndex = rand.Next(0, tileArray.Length - 1);

            nextObject = tileArray[randIndex];
            lastObject = gameObject;

            SpawnTile();
        }

        var it = loadedPrefabs.First;

        while (it != null)
        {
            it.Value.transform.position -= transform.forward * tileSpeed * Time.deltaTime;

            if (Camera.main.WorldToScreenPoint(it.Value.transform.position).z < 0)
            {
                var itNext = it.Next;

                Destroy(it.Value);
                loadedPrefabs.Remove(it);
                it = itNext;
            }
            else
                it = it.Next;
        }
	}

    public void SpawnTile()
    {
        GameObject temp;
;       int randIndex = rand.Next(0, tileArray.Length - 1);

        temp = Instantiate(nextObject, gameObject.transform);
        temp.transform.position = lastObject.transform.position;
        temp.transform.position += new Vector3(0, 0, temp.GetComponent<MeshRenderer>().bounds.size.z);

        lastObject = temp;
        loadedPrefabs.AddLast(lastObject);

        nextObject = tileArray[randIndex];
        spawnZone = transform.position.z - nextObject.GetComponent<MeshRenderer>().bounds.size.z / 2 - lastObject.GetComponent<MeshRenderer>().bounds.size.z / 2;
    }
}
