using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Transform[] spawnPoints;

    public string nextSegmentName = "";
    public string[] nextSegements;
    private Transform spawnPoint;

    private Helper helper = new Helper();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            nextSegmentName = ChooseNextSegment();
            spawnPoint = ChooseNextSpawnPoint();
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
        if (nextSegements.Length > 0) {
            int limit = nextSegements.Length;
            int index = helper.RandomNumberGenerator(limit);

            string name = nextSegements[index];
            Debug.Log(name);

            return name;
        } else {
            string[] names = PoolManager.current.GetLevelCompomentNames().ToArray();
            int limit = names.Length;
            int index = helper.RandomNumberGenerator(limit);

            string name = names[index];

            Debug.Log(name);

            return name;
        }
    }

    Transform ChooseNextSpawnPoint() {
        int limit = nextSegements.Length;
        int index = helper.RandomNumberGenerator(limit);

        Transform spawnPoint = spawnPoints[index];
        return spawnPoint;
    }
}
