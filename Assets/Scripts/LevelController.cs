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

    private PlayerController player;

    void Awake() {
        current = this;
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        levelComponents = PoolManager.current.GetPooledObjects("level");
        levelComponents.AddRange(PoolManager.current.GetPooledObjects("obstacle"));

        m_Velocity = Vector2.left;

        UpdateVelocity();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_stop) {
            acceleration = -3;

            m_speed = Mathf.Clamp(m_speed + acceleration * Time.deltaTime, minSpeed, maxSpeed);
            UpdateVelocity();
        }
    }

    public void UpdateVelocity() {
        for(int i = 0; i < levelComponents.Count; i++) {
            levelComponents[i].GetComponent<LevelComponent>().UpdateVelocity(m_Velocity, m_speed);
        }
    }

    public void Stop() {
        m_stop = true;
        Camera.main.GetComponent<CameraSmoothFollow>().m_follow = false;
        Debug.Log("GameOver");
    }

    private float CalculateAngle(Transform pos1, Transform pos2) {
        float angle = 0f;

        float vectorX = pos1.position.x - pos2.position.x;
        float vectorY = pos1.position.y - pos2.position.y;

        angle = Mathf.Atan2(vectorY, vectorX);

        return angle;
    }

    private Vector2 CalculateVelocity(float angle) {
        Vector2 velocity = new Vector2();

        velocity.x = Mathf.Cos(angle);
        velocity.y = Mathf.Sin(angle);

        return velocity;
    }
}
