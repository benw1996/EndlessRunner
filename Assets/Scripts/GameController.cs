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

    private float highScore;
    private float score = 0;
    private string scoreDisplay = "00000000";
    public float scoreMultiplyer = 7;

    private JSONParser parser;

    public Text scoreText;
    public Text highscoreText;
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

        parser = JSONParser.Instance();
        highScore = parser.GetHighScore();

        SetHighscoreText();
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

        if(score > highScore) {
            parser.SetHighScore(score);
            parser.SaveJson();
        }
    }

    public void UpdateGameState(bool newState) {
        playing = newState;
    }

    /// <summary>
    /// This function takes the players score and displays it on screen.
    /// </summary>
    void SetScoreText() {
        string tempScore = NormaliseScore(score);

        if ((int)Mathf.Floor(Mathf.Log10(score)) >= scoreDisplay.Length) {
            //The text box is then moved along to fit the extra digit in.
            Transform transform = scoreText.transform;
            Vector3 tempVector = new Vector3(transform.position.x - 20, transform.position.y, transform.position.z);

            scoreText.transform.position = tempVector;
        }

        scoreText.text = tempScore;
    }

    /// <summary>
    /// This function displays the high score to the text field on the start screen
    /// </summary>
    void SetHighscoreText() {
        string text = "High score: ";
        string highScoreString = NormaliseScore(highScore);
        Debug.Log(highScoreString);

        text += highScoreString;

        highscoreText.text = text;
    }

    /// <summary>
    /// This function normalises the score to be displayed as a string,
    /// If it is bellow 12 but above the length of the string then a zero is added to the string
    /// and the score is converted to a string, if it is above 12 digits then "a lot" is displayed instead.
    /// </summary>
    /// <param name="scoreToNormalise">The score to be normalised.</param>
    /// <returns></returns>
    string NormaliseScore(float scoreToNormalise) {
        string tempString;

        if ((int)Mathf.Floor(Mathf.Log10(scoreToNormalise)) < 12) {
            //If statement to check if the length of score is greater than the length of the score in the text box.
            if ((int)Mathf.Floor(Mathf.Log10(scoreToNormalise)) >= scoreDisplay.Length) {
                //If the length is equal to or greater than the length of the score in the text box then an additional 0 is added 
                scoreDisplay += "0";
            }
            //The score is then displayed in the text box.
            tempString = scoreToNormalise.ToString(scoreDisplay);
        } else {
            tempString = "a lot";
        }

        return tempString;
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

    /// <summary>
    /// Simple function that resets the high score to 0 then saves the high score to the json file.
    /// </summary>
    public void ResetStatistics() {
        //Highscore is reset to 0 and put to 0 in the JSON file.
        highScore = 0;
        parser.SetHighScore(highScore);
        //The new high score is saved to the file.
        parser.SaveJson();
        //The new highscore is displayed.
        SetHighscoreText();
    }
}
