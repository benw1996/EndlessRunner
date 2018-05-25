using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public static LevelController current;

    public delegate void OnGameOverDelegate();
    public static event OnGameOverDelegate GameOverDelegate;

    public delegate void OnResetDelegate();
    public static event OnResetDelegate ResetDelegate;

    public delegate void OnGameReadyToRestart();
    public static event OnGameReadyToRestart GameReadyToRestart;

    public float m_speed;
    private Vector2 m_Velocity;
    private bool m_stop = false;
    private bool gameOver = false;

    private float acceleration = -3;
    private float minSpeed = 0f;
    private float maxSpeed = 6f;

    private List<GameObject> levelComponents;
    public GameObject m_start;
    public Transform startPosition;

    public Text resetTimer;

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

        GameController.HomeDelegate += ResetLevel;
        GameController.RestartDelegate += RestartGame;
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

        m_speed = 5;
        UpdateVelocity();

        player.FreezeControls(false);

        player.anim.SetBool("isPlaying", true);
        player.anim.SetBool("restart", false);
    }

    public void UpdateVelocity() {
        for(int i = 0; i < levelComponents.Count; i++) {
            levelComponents[i].GetComponent<LevelComponent>().UpdateVelocity(m_Velocity, m_speed);
        }

        m_start.GetComponent<LevelComponent>().m_speed = m_speed;
        m_start.GetComponent<LevelComponent>().ForceUpdate();
    }

    public void Stop(bool fall) {
        if (!gameOver) {
            if (fall) {
                acceleration = -3;
                m_stop = true;
            } else {
                acceleration = -100;
                m_stop = true;
            }

            player.anim.SetBool("isPlaying", false);
            player.FreezeControls(true);
            GameOverDelegate();
        }
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

    private void DisableAllLevelComponents() {
        for (int i = 0; i < levelComponents.Count; i++) {
            levelComponents[i].SetActive(false);
        }
    }

    private void ResetLevel() {
        m_speed = 0f;
        UpdateVelocity();

        DisableAllLevelComponents();

        m_start.SetActive(false);
        m_start.transform.position = startPosition.position;
        m_start.SetActive(true);

        gameOver = false;
        m_stop = false;
        player.FreezeControls(true);
        ResetDelegate();
    }

    private void RestartGame() {
        ResetLevel();

        StartCoroutine(Restart());
    }

    IEnumerator Restart() {
        resetTimer.text = "3";
        resetTimer.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        resetTimer.text = "2";

        yield return new WaitForSeconds(1);
        resetTimer.text = "1";

        yield return new WaitForSeconds(1);
        resetTimer.gameObject.SetActive(false);

        GameReadyToRestart();

        m_speed = 5;
        UpdateVelocity();
        player.FreezeControls(false);
    }
}
