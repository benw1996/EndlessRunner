﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Transform groundCheck;
    public Transform startPosition;
    public Rigidbody2D m_rigidbody;

    public float m_jumpForce = 1000f;
    public float m_rotationSpeed = 100f;
    private bool m_grounded;
    private bool m_jump = false;

    private bool freezeControls = true;

    public Animator anim;
    public AudioSource jumpSound;
    public AudioSource landSound;
    public AudioSource ridingSound;
    public AudioSource crashSound;
    public AudioSource coinPickup;

    public Slider volumeSlider;

    private enum HitDirection { None, Top, Bottom, Left, Right };

    // Use this for initialization
    void Start () {
        m_rigidbody = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        GameController.PauseDelegate += Pause;
        GameController.UnPauseDelegate += UnPause;
        GameController.HomeDelegate += Restart;
        GameController.RestartDelegate += Restart;
    }
	
	// Update is called once per frame
	void Update () {
        if (!freezeControls) {
            m_grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

            if (Input.GetButtonDown("Jump") && m_grounded) {

                m_jump = true;

                anim.SetBool("hasLanded", false);
                anim.SetBool("hasJumped", true);

                jumpSound.Play();
            }

            if (m_grounded && ridingSound.isPlaying == false) {
                ridingSound.Play();
                landSound.Stop();
            } else if(!m_grounded){
                ridingSound.Stop();
            }

            //float h = Input.GetAxis("Horizontal");

            //if (!m_grounded)
            //    transform.Rotate(0, 0, -h * Time.deltaTime * m_rotationSpeed);
        }
    }

    void FixedUpdate() {
        if (m_jump) {
            m_rigidbody.AddForce(new Vector2(0f, m_jumpForce));
            m_jump = false;
        }
    }

    public void Pause() {
        FreezeControls(true);
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePosition;
    }

    public void UnPause() {
        FreezeControls(false);
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    public bool IsGrounded() {
        return m_grounded;
    }

    void OnCollisionEnter2D(Collision2D col) {
        //if (col.gameObject.tag == "Obstacle") {
        //    if(ReturnDirection(col) == HitDirection.Left) {
        //        LevelController.current.Stop(false);
        //    }
        //}

        ContactPoint2D contact = col.contacts[0];
        Vector2 colision = contact.normal;

        col.gameObject.GetComponent<LevelComponent>().HasBeenUsed();

        anim.SetBool("hasLanded", true);
        anim.SetBool("hasJumped", false);

        if(landSound.isPlaying == false)
            landSound.Play();

        //Debug.Log(Vector2.Dot(colision, transform.up));

        if (Vector2.Dot(colision, transform.up) < 0.6) {
            LevelController.current.Stop(true);
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            anim.SetBool("hasCrashed", true);
            crashSound.Play();
            ridingSound.Stop();
        } 
    }

    private HitDirection ReturnDirection(Collision2D col) {
        HitDirection hitDirection = HitDirection.None;

        Vector3 contactPoint = col.contacts[0].point;
        Vector3 center = col.collider.bounds.center;

        float rectWidth = col.collider.bounds.size.x;
        float rectHieght = col.collider.bounds.size.y;

        if (contactPoint.y > center.y && (contactPoint.x < center.x + rectWidth / 2 && contactPoint.x > center.x - rectWidth / 2)) {

            hitDirection = HitDirection.Top;

        } else if (contactPoint.y < center.y && (contactPoint.x < center.x + rectWidth / 2 && contactPoint.x > center.x - rectWidth / 2)) {

            hitDirection = HitDirection.Bottom;

        } else if (contactPoint.x > center.x && (contactPoint.y < center.y + rectHieght / 2 && contactPoint.y > center.y - rectHieght / 2)) {

            hitDirection = HitDirection.Right;

        } else if (contactPoint.x < center.x && (contactPoint.y < center.y + rectHieght / 2 && contactPoint.y > center.y - rectHieght / 2)) {

            hitDirection = HitDirection.Left;

        }
    
        return hitDirection;
    }

    public void FreezeControls(bool freeze) {
        freezeControls = freeze;
    }

    private void Restart() {
        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;
        m_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;

        anim.SetBool("restart", true);
        anim.SetBool("hasCrashed", false);
        anim.SetBool("isPlaying", true);
    }

    public void SoundVolumeController() {
        SetVolume(volumeSlider.value);
        GameController.current.SoundsVolumeChanged(volumeSlider.value);
    }

    private void SetVolume(float newVolume) {
        crashSound.volume = newVolume;
        jumpSound.volume = newVolume;
        landSound.volume = newVolume;
        ridingSound.volume = newVolume;
        coinPickup.volume = newVolume;
    }

    public void CoinPickedUp() {
        coinPickup.Play();
    }
}
