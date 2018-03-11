using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVector : MonoBehaviour {

    public Transform m_pos1;
    public Transform m_pos2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            Debug.Log("This Works");
            LevelController.current.UpdateVelocity(m_pos1, m_pos2);
        }
    }
}
