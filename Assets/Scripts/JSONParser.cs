using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONParser {

    public static JSONParser parser;

    //The file path for the JSON file.
    private string filePath = Application.dataPath + "/Resources/playerdata.json";

    protected JSONParser() {
        ReadJson();
        //Debug.Log("New instance made.");
    }

    public static JSONParser Instance() {
        if (parser == null) {
            parser = new JSONParser();
        }

        return parser;
    }

    public Data playerData;

    [System.Serializable]
    public class Data {
        public float highscore;
        public int coinscollected;
        public float[] settings;
    }

    public void ReadJson() {
        //Debug.Log(filePath);
        //Checks to see if the JSON file exists, and if so then the file is read in and the player data object is populated.
        if (File.Exists(filePath)) {
            //string data = File.ReadAllText(filePath);
            playerData = new Data();

            TextAsset fileText = (TextAsset)Resources.Load("playerdata", typeof(TextAsset));
            string data = fileText.text;

            playerData = JsonUtility.FromJson<Data>(data);
        } else {
            Debug.LogError("JSON File does not exist!");
        }
    }

    public void SaveJson() {
        //Checks to see if the JSON file exists, and if so then the file is saved to overriding the current data held.
        if (File.Exists(filePath)) {
            string data = JsonUtility.ToJson(playerData);

            File.WriteAllText(filePath, data);

            //Debug.Log("JSON saved!");
        } else {
            Debug.LogError("JSON File does not exist!");
        }
    }

    //Public getter for the players high score.
    public float GetHighScore() {
        return playerData.highscore;
    }

    //Public setter for the high score.
    public void SetHighScore(float score) {
        playerData.highscore = score;
    }

    //Public getter for the total coins collected.
    public int GetCoinsCollected() {
        return playerData.coinscollected; 
    }

    //Public method for adding coins to the total collected.
    public void AddCoinsCollected(int coins) {
        playerData.coinscollected += coins;
    }

    //public method for reseting the coins collected.
    public void ResetCoinsCollected() {
        playerData.coinscollected = 0;
    }

    //Public getter for the players settings.
    public float[] GetSettings() {
        return playerData.settings;
    }

    //Public setter for the music volume.
    public void SetMusicVolume(float musicVolume) {
        playerData.settings[0] = musicVolume;
    }
    
    //Public setter for the sounds volume.
    public void SetSoundsVolume(float soundsVolume) {
        playerData.settings[1] = soundsVolume;
    }
}
