﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public string nextSegmentName = "";
    public string[] nextSegements;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            //nextSegmentName = ChooseNextSegment();
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
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
            }
        }
    }

    string ChooseNextSegment() {
        int limit = nextSegements.Length;
        int index = Random.Range(0, limit);

        string name = nextSegements[index];
        Debug.Log(name);

        return name;
    }
}