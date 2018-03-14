using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Transform groundCheck;
    private Rigidbody2D m_rigidbody;

    public float m_jumpForce = 1000f;
    public float m_rotationSpeed = 100f;
    private bool m_grounded;

	// Use this for initialization
	void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		m_grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        float h = Input.GetAxis("Horizontal");

        transform.Rotate(0, 0, -h * Time.deltaTime * m_rotationSpeed);

        if ( Input.GetButtonDown("Jump") && m_grounded) {
            m_rigidbody.AddForce(new Vector2(0f, m_jumpForce));
        }
    }

    public bool IsGrounded() {
        return m_grounded;
    }
}
