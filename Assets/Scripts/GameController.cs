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
    private string scoreDisplay = "00000000";
    public float scoreMultiplyer = 7;

    public Text scoreText;
    public Image pauseScreen;
    public Button pauseButton;
    public GameObject startScreen;
    public Image settingsScreen;

    private bool playing = false;
    private bool paused = false;

    void Awake() {
        current = this;
    }

    // Use this for initialization
    void Start () {
        SetScoreText();
        
        LevelController.GameOverDelegate += GameOver;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("return") ){
            playing = true;

            scoreText.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(true);
            startScreen.SetActive(false);

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

    void GameOver() {
        playing = false;
    }

    public void UpdateGameState(bool newState) {
        playing = newState;
    }

    void SetScoreText() {
        if ((int)Mathf.Floor(Mathf.Log10(score)) >= scoreDisplay.Length) {
            scoreDisplay += "0";
            Transform transform = scoreText.transform;
            Vector3 tempVector = new Vector3(transform.position.x - 20, transform.position.y, transform.position.z);

            scoreText.transform.position = tempVector;
        }

        scoreText.text = score.ToString(scoreDisplay);
    }

    public void ShowSettings() {
        startScreen.SetActive(false);
        settingsScreen.gameObject.SetActive(true);
        settingsScreen.transform.position = new Vector3(593.5f, 354f, 0);
    }

    public void HideSettings() {
        startScreen.SetActive(true);
        settingsScreen.gameObject.SetActive(false);
    }

    public void PauseButtonPressed() {
        if (!paused) {
            paused = true;
            pauseScreen.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            settingsScreen.gameObject.SetActive(true);
            settingsScreen.transform.position = new Vector3(774.5f, 354, 0);

            PauseDelegate();
        } else {
            paused = false;
            pauseScreen.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            settingsScreen.gameObject.SetActive(false);

            UnPauseDelegate();
        }
    }
}
