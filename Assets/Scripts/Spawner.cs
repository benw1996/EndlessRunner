using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Transform[] spawnPoints;

    public string nextSegmentName = "";
    public List<string> nextSegments;
    private Transform spawnPoint;
    private int componentIndex;

    private Helper helper = new Helper();

    // Use this for initialization
    void Start() {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            spawnPoint = ChooseNextSpawnPoint();
            nextSegmentName = ChooseNextSegment();
            DisplayNextSegment();
        }
    }

    /// <summary>
    /// Method for displaying the next level component.
    /// </summary>
    void DisplayNextSegment() {
        if (nextSegmentName != "") {
            GameObject obj = PoolManager.current.GetPooledObject(nextSegmentName);

            if (obj != null) {
                obj.transform.position = spawnPoint.position;
                obj.transform.rotation = spawnPoint.rotation;
                obj.SetActive(true);
            }
        }
    }

    string ChooseNextSegment() {
        if (nextSegments.Count > 0) {
            int limit = nextSegments.Count;
            int index = helper.RandomNumberGenerator(limit);

            string name = nextSegments[index];

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

    Transform ChooseNextSpawnPoint() {
        int limit = spawnPoints.Length;
        int index = helper.RandomNumberGenerator(limit);

        Transform spawnPoint = spawnPoints[index];

        if(index == 0) {
            componentIndex = 0;
            nextSegments = PoolManager.current.GetLevelComponentNames()[componentIndex];
            Debug.Log(nextSegments.Count);
        }else if( index == 1 || index == 2) {
            componentIndex = 1;
            nextSegments = PoolManager.current.GetLevelComponentNames()[componentIndex];
            Debug.Log(nextSegments.Count);
        }else if( index == 3) {
            componentIndex = 2;
            nextSegments = PoolManager.current.GetLevelComponentNames()[componentIndex];
            Debug.Log(nextSegments.Count);
        }

        return spawnPoint;
    }
}
