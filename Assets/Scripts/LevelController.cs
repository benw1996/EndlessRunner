using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public static LevelController current;

    private float speed = 5.0f;
    private Vector2 m_Velocity;

    private List<GameObject> levelSegments;

    private PlayerController player;

    void Awake() {
        current = this;
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        levelSegments = PoolManager.current.GetPooledObjects("level");

        m_Velocity = Vector2.left * speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateVelocity(Transform pos1, Transform pos2) {
        m_Velocity = CalculateVelocity( CalculateAngle(pos1, pos2) );

        for(int i = 0; i < levelSegments.Count; i++) {
            levelSegments[i].GetComponent<LevelComponent>().UpdateVelocity(m_Velocity);
        }
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
