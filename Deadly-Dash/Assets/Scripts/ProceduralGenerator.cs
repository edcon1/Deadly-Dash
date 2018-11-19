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

    class TileCooldown
    {
        public TileCooldown(GameObject goType, int num)
        {
            gameObject = goType;
            cooldown = num;
        }

        public GameObject gameObject;
        public int cooldown;
    }

    public GameObject[] tileArray;
    [Tooltip("How many blank filler tiles are placed between the object tiles.")]
    public int tileSpacing = 0;
    [Tooltip("Doesn't allow an object tile to spawn again until this amount of other object tiles have spawned.")]
    public int tileDownTime = 0;

    private LinkedList<ObjInstance> loadedPrefabs = new LinkedList<ObjInstance>();
    private List<GameObject> avaliableTiles;
    private List<TileCooldown> onDowntimeTiles = new List<TileCooldown>();
    private GameObject nextObject;
    private GameObject lastObject;
    private Bounds nextBounds = new Bounds(Vector3.zero, Vector3.zero);
    private Bounds lastBounds = new Bounds(Vector3.zero, Vector3.zero);
    private Vector3 centerMass;
    private float spawnZone;
    private int blankCount = 10;
    private System.Random rand = new System.Random();

	// Use this for initialization
	void Start ()
    {
        NextObject();
        avaliableTiles = new List<GameObject>(tileArray);
        avaliableTiles.RemoveAt(0);

        tileDownTime *= tileSpacing + 1;

        for (int i = 0; i < blankCount; ++i)
            SpawnTile();
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
            it.Value.obj.transform.position -= transform.forward * GlobalScript.WorldSpeed * Time.deltaTime;
            Vector3 objPos = Camera.main.WorldToScreenPoint(it.Value.obj.transform.position);

            if (objPos.z + it.Value.bounds.extents.z < 0)
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
        GameObject temp;
        float objSpacing;

        temp = Instantiate(nextObject, gameObject.transform);
        CenterOrigin(temp);
        
        if (lastObject == null)
        {
            objSpacing = nextBounds.extents.z + nextBounds.extents.z;

            GameObject goTemp = new GameObject();
            lastObject = goTemp;
            lastObject.transform.position = transform.position;
            lastObject.transform.position -= new Vector3(0, 0, nextBounds.size.z * 10);
            Destroy(goTemp, 3);
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
        IterateCooldowns();

        if (blankCount > 0 || avaliableTiles.Count == 0)
        {
            nextObject = tileArray[0];
            blankCount--;
        }
        else
        {
            int randIndex = rand.Next(0, avaliableTiles.Count);

            nextObject = avaliableTiles[randIndex];
            blankCount = tileSpacing;

            onDowntimeTiles.Add(new TileCooldown(nextObject, tileDownTime));
            avaliableTiles.Remove(nextObject);
        }
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

    private void IterateCooldowns()
    {
        if (onDowntimeTiles.Count == 0)
            return;

        for (int i = 0; i < onDowntimeTiles.Count;)
        {
            onDowntimeTiles[i].cooldown--;

            if (onDowntimeTiles[i].cooldown <= 0)
            {
                avaliableTiles.Add(onDowntimeTiles[i].gameObject);
                onDowntimeTiles.RemoveAt(i);
            }
            else
                ++i;
        }
    }
}
