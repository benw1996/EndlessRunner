using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper {

	public  Helper() {

    }

    //Public function for randomly selcting an index given the limit of the array
    public int RandomNumberGenerator(int limit) {
        int index = 0;

        float r = Random.Range(0.0f, 1.0f);

        index = (int)(limit * (1 - Mathf.Pow(r, 0.5f)));

        return index;
    }

    //Public function for filtering an List for a given tag and where in the string to search for the tag.
    public List<string> FilterArray(List<string> array, string tag, int index) {
        List<string> tempList = new List<string>();

        for (int i = 0; i < array.Count; i++) {
            if (array[i].Split('/')[index].ToLower() == tag) {
                tempList.Add(array[i]);
                //Debug.Log(array[i]);
            }
        }

        return tempList;
    }
}
