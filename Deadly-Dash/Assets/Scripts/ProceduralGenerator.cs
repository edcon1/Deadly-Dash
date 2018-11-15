using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    struct ObjInstance
    {
        public ObjInstance(GameObject g, Bounds b)
        {
            obj = g;
            bounds = b;
        }

        public GameObject obj;
        public Bounds bounds;
    }

    [Tooltip("This sets the speed of the map.")]
    public float tileSpeed = 10;
    public GameObject[] tileArray;

    private LinkedList<ObjInstance> loadedPrefabs = new LinkedList<ObjInstance>();

    private GameObject nextObject;
    private GameObject lastObject;
    private Bounds nextBounds = new Bounds(Vector3.zero, Vector3.zero);
    private Bounds lastBounds = new Bounds(Vector3.zero, Vector3.zero);
    private Vector3 centerMass;
    private float spawnZone;
    private System.Random rand = new System.Random();

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (lastObject == null || lastObject.transform.position.z <= spawnZone)
            SpawnTile();

        // Snippet below deletes any tiles that go out of camera.
        var it = loadedPrefabs.First;
        while (it != null)
        {
            it.Value.obj.transform.position -= transform.forward * tileSpeed * Time.deltaTime;

            if (Camera.main.WorldToScreenPoint(it.Value.obj.transform.position).z + it.Value.bounds.extents.z < 0)
            {
                var itNext = it.Next;

                Destroy(it.Value.obj);
                loadedPrefabs.Remove(it);
                it = itNext;
            }
            else
                it = it.Next;
        }
	}

    /// <summary>
    /// Creates a tile.
    /// </summary>
    public void SpawnTile()
    {
        if (nextObject == null)
            NextObject();

        GameObject temp;
        float objSpacing; 
        
        temp = Instantiate(nextObject, gameObject.transform);
        CenterOrigin(temp);
        
        if (lastObject == null)
        {
            objSpacing = nextBounds.extents.z + nextBounds.extents.z;
            lastObject = gameObject;
        }
        else
            objSpacing = nextBounds.extents.z + lastBounds.extents.z;

        temp.transform.position = lastObject.transform.position;
        temp.transform.position += new Vector3(0, 0, objSpacing);

        lastObject = temp;
        lastBounds = nextBounds;

        loadedPrefabs.AddLast(new ObjInstance(lastObject, lastBounds));

        NextObject();

        spawnZone = transform.position.z - objSpacing;
    }



    /// <summary>
    /// Sets 'nextObject' to a random prefab in 'tileArray'.
    /// </summary>
    private void NextObject()
    {
        int randIndex = rand.Next(0, tileArray.Length);
        nextObject = tileArray[randIndex];
    }

    /// <summary>
    /// This function moves the origin of the GameObject to the average center of all its children, if it has any.
    /// </summary>
    /// <param name="obj"></param>
    private void CenterOrigin(GameObject obj)
    {
        var rArray = obj.GetComponentsInChildren<Renderer>();
        centerMass = Vector3.zero;

        foreach (Renderer r in rArray)
        {
            centerMass += r.transform.position;
            r.transform.parent = null;
        }

        centerMass /= rArray.Length;
        nextBounds = new Bounds(centerMass, Vector3.zero);

        foreach (Renderer r in rArray)
            nextBounds.Encapsulate(r.bounds);

        obj.transform.position = new Vector3(centerMass.x, centerMass.y - nextBounds.extents.y, centerMass.z);

        foreach (Renderer r in rArray)
            r.transform.parent = obj.transform;
    }
}
