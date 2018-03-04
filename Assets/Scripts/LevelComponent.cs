using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour {

    private Transform nextPosition;
    private Transform destroyPosition;

    public GameObject segment;

    public float speed = 5.0f;
    public string nextSegmentName = "";
    public string[] nextSegements;

    private GameController gameController;
    private Rigidbody2D m_rigidBody;
    private PlayerController player;

	// Use this for initialization
	void Start () {
        nextPosition = gameObject.transform.GetChild(0);
        destroyPosition = gameObject.transform.GetChild(1);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
    void OnEnable() {
        m_rigidBody = GetComponent<Rigidbody2D>();
        //m_rigidBody.velocity = Vector2.left * speed;
    }

	// Update is called once per frame
	void Update () {
        //if(!IsVisibleToCamera(destroyPosition)) {
        //gameObject.SetActive(false);
        //}

        if ( !player.IsGrounded() ) {
            m_rigidBody.velocity = (Vector2.up) * speed;
        } else {
            m_rigidBody.velocity = Vector2.left * speed;
        }
	}

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            //nextSegmentName = ChooseNextSegment();
            //DisplayNextSegment();
            

            Instantiate(segment, nextPosition);
            
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

    string ChooseNextSegment() {
        int limit = nextSegements.Length;
        int index = Random.Range(0, limit);

        string name = nextSegements[index];
        Debug.Log(name);

        return name;
    }

    bool IsVisibleToCamera(Transform transform) {
        Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }
}
