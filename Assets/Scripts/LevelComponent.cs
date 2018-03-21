using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour {

    public float m_speed = 5f;
    private Vector2 m_velocity = Vector2.left;

    public Transform[] m_obstacleSpawnPoints;

    private Rigidbody2D m_rigidBody;

	// Use this for initialization
	void Start () {

	}
	
    void OnEnable() {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.velocity = m_velocity * m_speed;

        SpawnObstacles();
    }
    void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        
	}

    public void UpdateVelocity(Vector2 newVelocity, float newSpeed) {
        m_velocity = newVelocity;
        m_speed = newSpeed;

        m_rigidBody.velocity = m_velocity * m_speed;
    }

    private void SpawnObstacles() {
        if(m_obstacleSpawnPoints.Length != 0) {
            GameObject obj = PoolManager.current.GetPooledObject("Obstacle/Medium");

            if(obj != null) {
                int limit = m_obstacleSpawnPoints.Length;
                int index = Random.Range(0, limit);

                obj.transform.position = m_obstacleSpawnPoints[index].position;
                obj.transform.rotation = m_obstacleSpawnPoints[index].rotation;
                obj.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            LevelController.current.Stop(true);
        }
    }
}
