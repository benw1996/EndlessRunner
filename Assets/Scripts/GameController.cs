using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<GameObject> levelSegments;
    private int currentSegment = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GetNextSegment() {
        if (currentSegment != levelSegments.Count - 1) {
            currentSegment++;
        } else {
            currentSegment = 0;
        }

        return levelSegments[currentSegment];
    }
}
