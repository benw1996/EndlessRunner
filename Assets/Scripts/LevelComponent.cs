using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour {

    private Transform nextPosition;
    private Transform destroyPosition;

    public float speed = 5.0f;
    public string nextSegmentName = "";
    private int currentSegment = 0;

    private GameController gameController;
    private Rigidbody2D m_rigidBody;

	// Use this for initialization
	void Start () {
        nextPosition = gameObject.transform.GetChild(0);
        destroyPosition = gameObject.transform.GetChild(1);

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
    void OnEnable() {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.velocity = Vector2.left * speed;
    }

	// Update is called once per frame
	void Update () {
        if(!IsVisibleToCamera(destroyPosition)) {
            gameObject.SetActive(false);
        }
	}

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            nextSegmentName = PoolManager.current.GetNames()[Random.Range(0, 7)];
            DisplayNextSegment();
        }
    }

    /// <summary>
    /// Method for displaying the next level component.
    /// </summary>
    void DisplayNextSegment() {
        if(nextSegmentName != "") {
            GameObject obj = PoolManager.current.GetPooledObject(nextSegmentName);
            
            if(obj != null) {
                obj.transform.position = nextPosition.position;
                obj.transform.rotation = nextPosition.rotation;
                obj.SetActive(true);
            }
        }
    }

    bool IsVisibleToCamera(Transform transform) {
        Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }
}
