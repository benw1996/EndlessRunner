﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelComponent : MonoBehaviour {

    private bool hasBeenUsed = false;

    public float m_speed = 0f;
    private Vector2 m_velocity = Vector2.left;

    public Transform[] m_obstacleSpawnPoints;
    public Transform[] m_coinSpawnPoints;

    private Rigidbody2D m_rigidBody;

    public bool isObstacle = false;
    public bool isCoin = false;

    private Animator anim;

    private Helper helper = new Helper();

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
    void OnEnable() {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.velocity = m_velocity * m_speed;

        SpawnObstacles();
        SpawnCoin();
    }

    void OnDisable() {

    }

    public void ForceUpdate() {
        m_rigidBody.velocity = m_velocity * m_speed;
    }

    void OnBecameVisible() {
        //hasBeenSeen = true;
    }

    void OnBecameInvisible() {
        if (hasBeenUsed || isCoin) {
            gameObject.SetActive(false);
            hasBeenUsed = false;
            //Debug.Log("Goodbye!");
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
            //Debug.Log(PoolManager.current.GetObstacleNames().ToArray().Length);
            int limit = PoolManager.current.GetObstacleNames().ToArray().Length;
            int index = helper.RandomNumberGenerator(limit);

            string name = PoolManager.current.GetObstacleNames().ToArray()[index];

            GameObject obj = PoolManager.current.GetPooledObject(name);

            if(obj != null) {
                limit = m_obstacleSpawnPoints.Length;
                index = helper.RandomNumberGenerator(limit);

                obj.transform.position = m_obstacleSpawnPoints[index].position;
                obj.transform.rotation = m_obstacleSpawnPoints[index].rotation;
                obj.SetActive(true);
            }
        }
    }

    private void SpawnCoin() {
        if(m_coinSpawnPoints.Length != 0) {
            GameObject obj = PoolManager.current.GetPooledObject("Level/Coin");

            if (obj != null) {
                int limit = m_coinSpawnPoints.Length;
                int index = Random.Range(0, limit);

                obj.transform.position = m_coinSpawnPoints[index].position;
                obj.transform.rotation = m_coinSpawnPoints[index].rotation;
                obj.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player") {
            if (!isCoin) {
                LevelController.current.Stop(!isObstacle);
            } else {
                GameController.current.SendMessage("IncrementCoinsCollected");
                col.GetComponent<PlayerController>().SendMessage("CoinPickedUp");

                StartCoroutine("coinPickedUP");
            }
        }
    }

    public void HasBeenUsed() {
        hasBeenUsed = true;
    }

    IEnumerator coinPickedUP() {
        anim.SetBool("pickedUp", true);

        yield return new WaitForSeconds(0.5f);

        anim.SetBool("pickedUp", false);
        gameObject.SetActive(false);
    }
}
