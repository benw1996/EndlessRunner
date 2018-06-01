using System.Collections;
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
	
    //On enable the components rigidboy is grabbed and given the velocity, and coins and obstacles are spawned.
    void OnEnable() {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.velocity = m_velocity * m_speed;

        SpawnObstacles();
        SpawnCoin();
    }

    void OnDisable() {

    }

    //Public method for forcing an update of the velocity.
    public void ForceUpdate() {
        m_rigidBody.velocity = m_velocity * m_speed;
    }

    void OnBecameVisible() {
        //hasBeenSeen = true;
    }

    //When the component becoms invisible and it has been used it is set to inactive to be used again.
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

    //Public method for updating the velocity given a new velocity and speed.
    public void UpdateVelocity(Vector2 newVelocity, float newSpeed) {
        m_velocity = newVelocity;
        m_speed = newSpeed;

        m_rigidBody.velocity = m_velocity * m_speed;
    }

    //Method for spawning obstacles, if the list of obstacle spawn points is not empty then a spawn point is chosen and an obstacle is placed at it.
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

    //Method for spawning a coin, if the list of coin spawn points is not empty then a spawn point is chosen and a coin is placed at it.
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

    //If another object enters the trigger attached to this component then this method is called.
    private void OnTriggerEnter2D(Collider2D col) {
        //First it is checked that it is the player entering the trigger
        if(col.tag == "Player") {
            //If this component is not a coin then the gameover method is called.
            if (!isCoin) {
                LevelController.current.Stop(!isObstacle);
            } else {
                //If it is a coin then the coin counter is incremented and the sound is played along with the co routine for the animation is called.
                GameController.current.SendMessage("IncrementCoinsCollected");
                col.GetComponent<PlayerController>().SendMessage("CoinPickedUp");

                StartCoroutine("coinPickedUP");
            }
        }
    }

    //Public method for telling the component it has been used.
    public void HasBeenUsed() {
        hasBeenUsed = true;
    }

    IEnumerator coinPickedUP() {
        //The animation is started.
        anim.SetBool("pickedUp", true);
        //It then waits for the animation to play and then disables the coin ready for it to be used again.
        yield return new WaitForSeconds(0.5f);

        anim.SetBool("pickedUp", false);
        gameObject.SetActive(false);
    }
}
