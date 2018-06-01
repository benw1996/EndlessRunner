using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<Transform> spawnPoints;

    public string nextSegmentName = "";
    public List<string> nextSegments;
    private Transform spawnPoint;
    private bool hasSpawnedObject = false;

    private int componentIndex;
    private int spawnPointIndex;

    private Helper helper = new Helper();

    // Use this for initialization
    void Start() {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDisable() {
        hasSpawnedObject = false;
        //Debug.Log("hello");
    }

    /// <summary>
    /// When the player enters the trigger the next spawn point is chosen, the next segement is chosen and then it is displayed.
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player" && !hasSpawnedObject) {
            spawnPoint = ChooseNextSpawnPoint();
            nextSegmentName = ChooseNextSegment();
            DisplayNextSegment();
            hasSpawnedObject = true;
        }
    }

    /// <summary>
    /// Method for displaying the next level component.
    /// </summary>
    void DisplayNextSegment() {
        if (nextSegmentName != "") {
            //The next component is grabbed from the pool manager.
            GameObject obj = PoolManager.current.GetPooledObject(nextSegmentName);

            if (obj != null) {
                obj.GetComponentInChildren<Spawner>().ShuffleSpawnPoints(spawnPointIndex);

                obj.transform.position = spawnPoint.position;
                obj.transform.rotation = spawnPoint.rotation;
                obj.SetActive(true);

                //Debug.Log("Object Spawned");
            }
        }
    }

    /// <summary>
    /// The next segment is chosen using this method.
    /// If there is not a given list of next segements to choose from then the next segment is chosen depending on the spawn point chosen.
    /// The list of names is chosen from and then is shuffled to ensure the same component is not always chosen.
    /// </summary>
    /// <returns></returns>
    string ChooseNextSegment() {
        if (nextSegments.Count > 0) {
            int limit = nextSegments.Count;
            int index = helper.RandomNumberGenerator(limit);

            string name = nextSegments[index];

            //Debug.Log(name);

            PoolManager.current.ShuffleList(componentIndex, name);

            return name;
        } else {
            string[] names = PoolManager.current.GetLevelComponentNames()[1].ToArray();

            int limit = names.Length;
            int index = helper.RandomNumberGenerator(limit);

            string name = names[index];

            //Debug.Log(name);

            return name;
        }
    }

    /// <summary>
    /// The next spawn point is chosen at random from the given list of spawn points.
    /// Then depending on the spawn point chosen the index is set which detemines which component is chosen.
    /// </summary>
    /// <returns></returns>
    Transform ChooseNextSpawnPoint() {
        if (spawnPoints.Count != 1) {
            int limit = spawnPoints.Count;
            spawnPointIndex = helper.RandomNumberGenerator(limit);

            Transform spawnPoint = spawnPoints[spawnPointIndex];

            if (spawnPointIndex == 0) {
                componentIndex = 0;
                nextSegments = PoolManager.current.GetLevelComponentNames()[componentIndex];
            } else if (spawnPointIndex == 1 || spawnPointIndex == 2) {
                componentIndex = 1;
                nextSegments = PoolManager.current.GetLevelComponentNames()[componentIndex];
            } else if (spawnPointIndex == 3) {
                componentIndex = 2;
                nextSegments = PoolManager.current.GetLevelComponentNames()[componentIndex];
            }

            return spawnPoint;
        } else {
            return spawnPoints[0];
        }
    }

    //Public method for shuffling the spawn points to ensure the same spawn point doesnt keep getting chosen.
    public void ShuffleSpawnPoints(int index) {
        Transform tempSpawnPoint = spawnPoints[index];

        spawnPoints.RemoveAt(index);
        spawnPoints.Add(tempSpawnPoint);

        //Debug.Log("List shuffled!");
    }
}
