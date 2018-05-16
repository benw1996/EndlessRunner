﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : MonoBehaviour {

    private bool hasBeenSeen = false;

    public float m_speed = 0f;
    private Vector2 m_velocity = Vector2.left;

    public Transform[] m_obstacleSpawnPoints;

    private Rigidbody2D m_rigidBody;

    public bool isObstacle = false;

	// Use this for initialization
	void Start () {

	}
	
    void OnEnable() {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.velocity = m_velocity * m_speed;

        SpawnObstacles();
    }

    public void ForceStart() {
        m_rigidBody.velocity = m_velocity * m_speed;
    }

    void OnBecameVisible() {
        hasBeenSeen = true;
        Debug.Log("Hello!");
    }

    void OnBecameInvisible() {
        if (hasBeenSeen) {
            gameObject.SetActive(false);
            hasBeenSeen = false;
            Debug.Log("Goodbye!");
        }
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
            LevelController.current.Stop(!isObstacle);
        }
    }
}
