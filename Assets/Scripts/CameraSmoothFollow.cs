using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {

    public Transform m_target;
    public bool m_follow = true;

	// Use this for initialization
	void Start () {
        LevelController.GameOverDelegate += StopFollow;
        LevelController.ResetDelegate += Reset;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_follow) {
            //transform.position = new Vector3(m_target.position.x, m_target.position.y, -10);
            Vector3 target = m_target.position;
            target.z = -10;
            //If the follow boolean is true then the camera will interpolate to the players position.
            transform.position = Vector3.Slerp(transform.position, target, 0.1f);
        }
	}

    //Method for changing the follow boolean to whatever it currently isnt.
    void ChangeFollow() {
        m_follow = !m_follow;
    }

    //Method for stopping the camera from following the player.
    void StopFollow() {
        m_follow = false;
    }

    //Method for resetting the follow boolean back to true.
    void Reset() {
        m_follow = true;
    }
}
