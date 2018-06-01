using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONParser {

    public static JSONParser parser;

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

        if (File.Exists(filePath)) {
            string data = File.ReadAllText(filePath);
            playerData = new Data();

            playerData = JsonUtility.FromJson<Data>(data);
        } else {
            Debug.LogError("JSON File does not exist!");
        }
    }

    public void SaveJson() {
        if (File.Exists(filePath)) {
            string data = JsonUtility.ToJson(playerData);

            File.WriteAllText(filePath, data);

            //Debug.Log("JSON saved!");
        } else {
            Debug.LogError("JSON File does not exist!");
        }
    }

    public float GetHighScore() {
        return playerData.highscore;
    }

    public void SetHighScore(float score) {
        playerData.highscore = score;
    }

    public int GetCoinsCollected() {
        return playerData.coinscollected; 
    }

    public void AddCoinsCollected(int coins) {
        playerData.coinscollected += coins;
    }

    public void ResetCoinsCollected() {
        playerData.coinscollected = 0;
    }

    public float[] GetSettings() {
        return playerData.settings;
    }

    public void SetMusicVolume(float musicVolume) {
        playerData.settings[0] = musicVolume;
    }
    
    public void SetSoundsVolume(float soundsVolume) {
        playerData.settings[1] = soundsVolume;
    }
}
