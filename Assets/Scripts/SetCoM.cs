using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCoM : MonoBehaviour {

    public Transform com;
    private Vector2 m_com;
    private Rigidbody2D m_rigidbody;

	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_com = com.position;

        m_rigidbody.centerOfMass = m_com;
	}
}
