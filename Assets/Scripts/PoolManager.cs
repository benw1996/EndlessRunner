using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    public static PoolManager current;


    public string NamingConvention = "[Type] / [Name]";
    public string[] names;
    public GameObject[] pooledObjects;
    public int[] poolAmounts;

    private Dictionary<string, List<GameObject>> mainPool = new Dictionary<string, List<GameObject>>();
    private List<GameObject> tempList;

    private List<List<string>> levelComponentNames = new List<List<string>>();
    private List<string> obstacleNames;

    private Helper helper = new Helper();

    void Awake() {
        current = this;
    }

	// Use this for initialization
	void Start () {
        tempList = new List<GameObject>();

        for (int i = 0; i < names.Length; i++) {
            List<GameObject> objList = new List<GameObject>();

            for (int j = 0; j < poolAmounts[i]; j++) {
                GameObject obj = Instantiate(pooledObjects[i]);

                obj.SetActive(false);
                objList.Add(obj);
            }

            mainPool.Add(names[i], objList);
        }

        List<string> tempNames = new List<string>(names);

        //Here is where the list of level components is populated
        PopulateListsofLevelComponents();

        obstacleNames = helper.FilterArray(tempNames, "obstacle", 0);
	}
	
	public GameObject GetPooledObject(string name) {
        if (mainPool.ContainsKey(name)) {
            tempList = mainPool[name] as List<GameObject>;
            
            for(int i = 0; i < tempList.Count; i++) {
                if(tempList[i] != null) {
                    if (!tempList[i].activeInHierarchy) {
                        return tempList[i];
                    }
                }
            }
        }

        return null;
    }

    public List<GameObject> GetPooledObjects(string tag) {
        tempList = new List<GameObject>();

        foreach(KeyValuePair<string, List<GameObject>> poolObject in mainPool) {
            string key = poolObject.Key.Split('/')[0].ToLower();

            if(key == tag.ToLower()) {
                tempList.AddRange(poolObject.Value);
            }
        }

        return tempList;
    }

    public void PopulateListsofLevelComponents() {
        List<string> tempNames = new List<string>();
        tempNames.AddRange(names);

        levelComponentNames.Add(helper.FilterArray(tempNames, "slope", 1));
        levelComponentNames.Add(helper.FilterArray(tempNames, "high", 1));
        levelComponentNames.Add(helper.FilterArray(tempNames, "low", 1));
    }

    //This is the public function for getting the list
    public List<List<string>> GetLevelComponentNames() {
        return levelComponentNames; 
    }

    public List<string> GetObstacleNames() {
        return obstacleNames;
    }

    //Public function for moving an item of a list to the end once it has been used
    public void ShuffleList(int index, string item) {

        levelComponentNames[index].Remove(item);
        levelComponentNames[index].Add(item);
    }

    public void ResetPool() {
        for(int i = 0; i < names.Length; i++) {
            tempList = mainPool[ names[i] ] as List<GameObject>;

            for(int j = 0; j < tempList.Count; j++) {
                if(tempList[j] != null) {
                    if (tempList[j].activeInHierarchy) {
                        tempList[j].SetActive(false);
                    }
                }
            }
        }
    }
}
