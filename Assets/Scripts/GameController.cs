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

    public delegate void OnHomeDelegate();
    public static event OnHomeDelegate HomeDelegate;

    public delegate void OnRestartDelegate();
    public static event OnRestartDelegate RestartDelegate;

    private float highScore;
    private float score = 0;
    private string scoreDisplay = "00000000";
    public float scoreMultiplyer = 7;

    private int coinsCollected = 0;
    private int totalCoinsCollected = 0;

    private JSONParser parser;

    //All the UI elements
    public Text scoreText;
    public Text coinCounterText;
    public Text highscoreText;
    public Text totalCoinsText;
    public Image pauseScreen;
    public Button pauseButton;
    public GameObject startScreen;
    public Image settingsScreen;
    public Slider volumeSlider;
    public Image gameOverScreen;
    public Text newHighscoreText;
    public Text gameoverScoreText;

    public AudioSource music;

    private bool playing = false;
    private bool paused = false;

    private PlayerController player;

    void Awake() {
        current = this;
    }

    // Use this for initialization
    void Start () {
        //The score text is set to the score.
        SetScoreText();
        
        LevelController.GameOverDelegate += GameOver;
        LevelController.GameReadyToRestart += Restart;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //The JSON parser is created and the json file is read and the UI is updated with the values read from the JSON file
        parser = JSONParser.Instance();
        highScore = parser.GetHighScore();
        totalCoinsCollected = parser.GetCoinsCollected();
        SetTotalCoinsCollectedText();
        //The settings read from the JSON file are given to the sliders in the settings to update the volumes.
        volumeSlider.value = parser.GetSettings()[0];
        player.volumeSlider.value = parser.GetSettings()[1];
        //The high score text is set to the high score.
        SetHighscoreText();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("return") ){
            //The playing boolean is set to true, the relevant UI is displayed and the start event is called.
            playing = true;

            DisplayGameUI();
            HideStartScreen();

            StartDelegate();
        }

        if (Input.GetKeyDown("p") && playing) {
            //The pause event is called.
            PauseButtonPressed();
        }
	}

    void FixedUpdate() {
        if (playing && !paused) {
            //At fixed intervals if the game is being played then the score is incremented.
            score += (Time.deltaTime * scoreMultiplyer);
            SetScoreText();
        }
    }
    /// <summary>
    /// Method called when the game is over.
    /// The in game UI is hidden and the game over screen is displayed.
    /// The relevant UI is updated with new values.
    /// The JSON is updated with the new coins collected and the new high score if it has been achieved.
    /// The JSON file is then saved.
    /// </summary>
    void GameOver() {
        playing = false;

        HideGameUI();

        DisplayGameOverScreen();

        string text = "Score: ";
        string gameOverScore = NormaliseScore(score);
        text += gameOverScore;
        gameoverScoreText.text = text;

        parser.AddCoinsCollected(coinsCollected);
        totalCoinsCollected += coinsCollected;

        SetTotalCoinsCollectedText();

        if (score > highScore) {
            parser.SetHighScore(score);
            highScore = score;

            newHighscoreText.gameObject.SetActive(true);
        }
        SetHighscoreText();

        parser.SaveJson();
    }

    //Public method for updating the game state.
    public void UpdateGameState(bool newState) {
        playing = newState;
    }

    //Public method for incrementing the coins collected.
    public void IncrementCoinsCollected() {
        coinsCollected++;
        //The coins collected counter is updated.
        coinCounterText.text = coinsCollected.ToString("00000000");
    }

    //The total coins collected counter is updated using this method.
    void SetTotalCoinsCollectedText() {
        totalCoinsText.text = totalCoinsCollected.ToString("00000000");
    }

    /// <summary>
    /// This function takes the players score and displays it on screen.
    /// </summary>
    void SetScoreText() {
        string tempScore = NormaliseScore(score);
        //If the score is longer than the current number of digits then the text box is moved along to fit the new digit on screen.
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

    //The public method for displaying the settings screen in the middle of the screen.
    public void ShowSettings() {
        HideStartScreen();
        DisplaySettings();
        settingsScreen.transform.position = new Vector3(593.5f, 354f, 0);
    }

    //The public method for hiding the settings and displaying the home screen.
    public void SettingsBackButtonPressed() {
        DisplayStartScreen();
        HideSettings();
    }

    //Function called when the pause button has been pressed
    public void PauseButtonPressed() {
        if (!paused) {
            //Paused boolean set to true
            paused = true;
            //Paused UI displayed on screen
            DisplayPauseScreen();
            HideGameUI();
            DisplaySettings();
            //Settings screen displayed beside the paused menu
            settingsScreen.transform.position = new Vector3(774.5f, 354, 0);
            //Pause delegate is called
            PauseDelegate();
        } else {
            //Paused boolean set to true
            paused = false;
            //Paused UI taken off screen
            HidePauseScreen();
            DisplayGameUI();
            HideSettings();
            //Unpause delegate is called
            UnPauseDelegate();
        }
    }

    /// <summary>
    /// Simple function that resets the high score and coins collected to 0 then saves the json file.
    /// </summary>
    public void ResetStatistics() {
        //Highscore is reset to 0 and put to 0 in the JSON file.
        highScore = 0;
        parser.SetHighScore(highScore);
        totalCoinsCollected = 0;
        parser.ResetCoinsCollected();
        //The new high score is saved to the file.
        parser.SaveJson();
        //The new highscore is displayed.
        SetHighscoreText();
        //The new coins collected is displayed.
        SetTotalCoinsCollectedText();
    }

    /// <summary>
    /// Public method for controlling the music via the slider in the settings.
    /// </summary>
    public void VolumeController() {
        music.volume = volumeSlider.value;

        parser.SetMusicVolume(music.volume);
        parser.SaveJson();
    }

    /// <summary>
    /// Public method for controlling the sounds via the slider in the settings.
    /// </summary>
    public void SoundsVolumeChanged(float newVolume) {
        parser.SetSoundsVolume(newVolume);
        parser.SaveJson();
    }

    /// <summary>
    /// Public method for calling the home delegate.
    /// </summary>
    public void HomeButtonPressed() {
        playing = false;
        score = 0;
        coinsCollected = 0;

        HomeDelegate();

        HidePauseScreen();
        HideGameOverScreen();
        HideSettings();

        DisplayStartScreen();
    }

    /// <summary>
    /// Public method for calling the restart delegate.
    /// </summary>
    public void RestartButtonPressed() {
        playing = false;
        score = 0;
        coinsCollected = 0;

        RestartDelegate();

        HidePauseScreen();
        HideGameOverScreen();
        HideSettings();
    }

    //Public method for restarting the game.
    public void Restart() {
        DisplayGameUI();
        playing = true;
    }

    //All the methods for displaying and hiding the various UI screens.

    //Method for displaying the game UI
    private void DisplayGameUI() {
        scoreText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        coinCounterText.gameObject.SetActive(true);
    }

    //Method for Hiding the game UI
    private void HideGameUI() {
        scoreText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        coinCounterText.gameObject.SetActive(false);
    }

    //Method for displaying the settings
    private void DisplaySettings() {
        settingsScreen.gameObject.SetActive(true);
    }

    //Method for hiding the settings
    private void HideSettings() {
        settingsScreen.gameObject.SetActive(false);
    }

    //Method for displaying the pause screen
    private void DisplayPauseScreen() {
        pauseScreen.gameObject.SetActive(true);
    }

    //Method for hiding the pause screen
    private void HidePauseScreen() {
        pauseScreen.gameObject.SetActive(false);
    }

    //Method for displaying the start screen
    private void DisplayStartScreen() {
        startScreen.gameObject.SetActive(true);
    }

    //Method for hiding the start screen
    private void HideStartScreen() {
        startScreen.gameObject.SetActive(false);
    }

    //Method for displaying the gameover screen
    private void DisplayGameOverScreen() {
        gameOverScreen.gameObject.SetActive(true);
    }

    //Method for hiding the gameover screen
    private void HideGameOverScreen() {
        gameOverScreen.gameObject.SetActive(false);
    }
}
