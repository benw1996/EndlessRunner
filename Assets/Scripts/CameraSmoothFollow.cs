using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {

    public Transform m_target;
    public bool m_follow = true;

	// Use this for initialization
	void Start () {
        LevelController.GameOverDelegate += ChangeFollow;
        LevelController.ResetDelegate += Reset;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_follow) {
            //transform.position = new Vector3(m_target.position.x, m_target.position.y, -10);
            Vector3 target = m_target.position;
            target.z = -10;

            transform.position = Vector3.Slerp(transform.position, target, 0.1f);
        }
	}

    void ChangeFollow() {
        m_follow = !m_follow;
    }

    void Reset() {
        m_follow = true;
    }
}
