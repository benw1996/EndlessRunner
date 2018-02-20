using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour {

    //Transform of child object with location for the next component
    private Transform nextLocation;

    private GameController gameController;

	// Use this for initialization
	void Start () {
        nextLocation = gameObject.transform.GetChild(0);

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        DisplayNextSegment();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Method for creating the next level component.
    /// </summary>
    void DisplayNextSegment() {
        GameObject nextSegment = gameController.GetNextSegment();

        Instantiate(nextSegment, nextLocation.position, nextLocation.rotation);
    }
}
