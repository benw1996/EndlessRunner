using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    private float speed = 5.0f;
    private Vector2 currentVelocity;

    private PlayerController player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        currentVelocity = Vector2.left * speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private float CalculateAngle(Transform pos1, Transform pos2) {
        float angle = 0f;

        float vectorX = pos1.position.x - pos2.position.x;
        float vectorY = pos1.position.y - pos2.position.y;

        angle = Mathf.Atan2(vectorY, vectorX);

        return angle;
    }
}
