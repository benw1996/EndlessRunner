using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour {

    public float m_speed = 5.0f;
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
                obj.transform.position = m_obstacleSpawnPoints[0].position;
                obj.transform.rotation = m_obstacleSpawnPoints[0].rotation;
                obj.SetActive(true);
            }
        }
    }
}
