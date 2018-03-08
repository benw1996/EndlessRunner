using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour {

    public float speed = 5.0f;
    public string nextSegmentName = "";
    public string[] nextSegements;

    private GameController gameController;
    private Rigidbody2D m_rigidBody;
    private PlayerController player;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
    void OnEnable() {
        m_rigidBody = GetComponent<Rigidbody2D>();
        //m_rigidBody.velocity = Vector2.left * speed;
    }
    void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if ( !player.IsGrounded() ) {
            m_rigidBody.velocity = (Vector2.up) * speed;
        } else {
            m_rigidBody.velocity = Vector2.left * speed;
        }
	}
}
