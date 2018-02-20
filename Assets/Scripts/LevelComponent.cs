using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour {

    private Transform nextPosition;
    private Transform destroyPosition;

    public float speed = 5.0f;

    private GameController gameController;
    private Rigidbody2D m_rigidBody;

	// Use this for initialization
	void Start () {
        nextPosition = gameObject.transform.GetChild(0);
        destroyPosition = gameObject.transform.GetChild(1);

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.velocity = Vector2.left * speed;

        //DisplayNextSegment();
	}
	
	// Update is called once per frame
	void Update () {
        if(!IsVisibleToCamera(destroyPosition)) {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            DisplayNextSegment();
        }
    }

    /// <summary>
    /// Method for creating the next level component.
    /// </summary>
    void DisplayNextSegment() {
        GameObject nextSegment = gameController.GetNextSegment();

        Instantiate(nextSegment, nextPosition.position, nextPosition.rotation);
    }

    public static bool IsVisibleToCamera(Transform transform) {
        Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }
}
