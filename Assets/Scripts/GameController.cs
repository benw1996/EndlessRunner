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

    /// <summary>
    /// This function takes the players score and displays it on screen.
    /// </summary>
    void SetScoreText() {
        //Check that if the score is greater than 12 digits then "alot" is displayed instead of continuing to display the score.
        if ((int)Mathf.Floor(Mathf.Log10(score)) < 12) {
            //If statement to check if the length of score is greater than the length of the score in the text box.
            if ((int)Mathf.Floor(Mathf.Log10(score)) >= scoreDisplay.Length) {
                //If the length is equal to or greater than the length of the score in the text box then an additional 0 is added 
                scoreDisplay += "0";
                //The text box is then moved along to fit the extra digit in.
                Transform transform = scoreText.transform;
                Vector3 tempVector = new Vector3(transform.position.x - 20, transform.position.y, transform.position.z);

                scoreText.transform.position = tempVector;
            }
            //The score is then displayed in the text box.
            scoreText.text = score.ToString(scoreDisplay);
        } else {
            scoreText.text = "a lot";
        }
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

    //Function called when the pause button has been pressed
    public void PauseButtonPressed() {
        if (!paused) {
            //Paused boolean set to true
            paused = true;
            //Paused UI displayed on screen
            pauseScreen.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            settingsScreen.gameObject.SetActive(true);
            //Settings screen displayed beside the paused menu
            settingsScreen.transform.position = new Vector3(774.5f, 354, 0);
            //Pause delegate is called
            PauseDelegate();
        } else {
            //Paused boolean set to true
            paused = false;
            //Paused UI taken off screen
            pauseScreen.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            settingsScreen.gameObject.SetActive(false);
            //Unpause delegate is called
            UnPauseDelegate();
        }
    }
}
