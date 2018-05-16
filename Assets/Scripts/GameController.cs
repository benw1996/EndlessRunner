using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController current;

    private float score = 0;
    public float scoreMultiplyer = 7;

    private bool playing = false;

    void Awake() {
        current = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("return") ){
            LevelController.current.StartGame();
            playing = true;
        }
	}

    void FixedUpdate() {
        if (playing) {
            score += (Time.deltaTime * scoreMultiplyer);
        }
    }

    public void UpdateGameState(bool newState) {
        playing = newState;
    }
}
