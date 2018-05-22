using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController current;

    public delegate void OnPauseDelegate();
    public static event OnPauseDelegate PauseDelegate;

    public delegate void OnUnPauseDelegate();
    public static event OnUnPauseDelegate UnPauseDelegate;

    public delegate void OnStartDelegate();
    public static event OnStartDelegate StartDelegate;

    private float score = 0;
    public float scoreMultiplyer = 7;
    public Text scoreText;

    private bool playing = false;
    private bool paused = false;

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
            playing = true;

            StartDelegate();
        }

        if (Input.GetKeyDown("p") && playing) {
            PauseButtonPressed();
        }
	}

    void FixedUpdate() {
        if (playing && !paused) {
            score += (Time.deltaTime * scoreMultiplyer);
            SetScoreText();
        }
    }

    public void UpdateGameState(bool newState) {
        playing = newState;
    }

    void SetScoreText() {
        scoreText.text = score.ToString("00000000");
    }

    public void PauseButtonPressed() {
        if (!paused) {
            paused = true;

            PauseDelegate();
        } else {
            paused = false;

            UnPauseDelegate();
        }
    }
}
