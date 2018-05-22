using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public static LevelController current;

    public float m_speed;
    private Vector2 m_Velocity;
    private bool m_stop = false;

    private float acceleration = -3;
    private float minSpeed = 0f;
    private float maxSpeed = 6f;

    private List<GameObject> levelComponents;
    public GameObject m_start;

    private PlayerController player;

    void Awake() {
        current = this;
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        levelComponents = PoolManager.current.GetPooledObjects("level");
        levelComponents.AddRange(PoolManager.current.GetPooledObjects("obstacle"));

        GameController.PauseDelegate += Pause;
        GameController.UnPauseDelegate += UnPause;
        GameController.StartDelegate += StartGame;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_stop) {
            m_speed = Mathf.Clamp(m_speed + acceleration * Time.deltaTime, minSpeed, maxSpeed);
            UpdateVelocity();
        }
    }

    public void StartGame() {
        m_Velocity = Vector2.left;

        UpdateVelocity();

        player.FreezeControls(false);
    }

    public void UpdateVelocity() {
        for(int i = 0; i < levelComponents.Count; i++) {
            levelComponents[i].GetComponent<LevelComponent>().UpdateVelocity(m_Velocity, m_speed);
        }

        m_start.GetComponent<LevelComponent>().m_speed = m_speed;
        m_start.GetComponent<LevelComponent>().ForceUpdate();
    }

    public void Stop(bool fall) {
        if (fall) {
            acceleration = -3;
            m_stop = true;
        } else {
            acceleration = -100;
            m_stop = true;
        }
        
        Camera.main.GetComponent<CameraSmoothFollow>().m_follow = false;
        GameController.current.UpdateGameState(false);
        Debug.Log("GameOver");
    }

    public void Pause() {
        m_speed = 0f;
        UpdateVelocity();
    }

    public void UnPause() {
        m_speed = 5f;
        UpdateVelocity();
    }

    private float CalculateAngle(Transform pos1, Transform pos2) {
        float angle = 0f;

        float vectorX = pos1.position.x - pos2.position.x;
        float vectorY = pos1.position.y - pos2.position.y;

        angle = Mathf.Atan2(vectorY, vectorX);

        return angle;
    }

    private Vector2 CalculateVelocity(float angle) {
        Vector2 velocity = new Vector2 {
            x = Mathf.Cos(angle),
            y = Mathf.Sin(angle)
        };

        return velocity;
    }
}
