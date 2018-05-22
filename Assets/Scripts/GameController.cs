using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController current;

    private float score = 0;
    public float scoreMultiplyer = 7;
    public Text scoreText;

    private bool playing = false;

    void Awake() {
        current = this;
    }

    // Use this for initialization
    void Start () {
        SetScoreText();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("return") ){
            LevelController.current.StartGame();
            playing = true;
            UpdateScrolling(true);
        }
	}

    void FixedUpdate() {
        if (playing) {
            score += (Time.deltaTime * scoreMultiplyer);
            SetScoreText();
        }
    }

    public void UpdateGameState(bool newState) {
        playing = newState;
        UpdateScrolling(newState);
    }

    public void UpdateScrolling(bool scroll) {
        GameObject[] backgroundObjs;
        backgroundObjs = GameObject.FindGameObjectsWithTag("Background");

        for(int i = 0; i < backgroundObjs.Length; i++) {
            backgroundObjs[i].SendMessage("UpdateScrolling", scroll);
        }
    }

    void SetScoreText() {
        scoreText.text = score.ToString("00000000");
    }
}
