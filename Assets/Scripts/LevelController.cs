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
    public PhysicsMaterial2D playerMat;

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

    /// <summary>
    /// Public method for going through all the level components and updating their velocity.
    /// </summary>
    public void UpdateVelocity() {
        for(int i = 0; i < levelComponents.Count; i++) {
            levelComponents[i].GetComponent<LevelComponent>().UpdateVelocity(m_Velocity, m_speed);
        }

        //The starting comopnent is not contained in the pool manager so it needs to be updated 
        m_start.GetComponent<LevelComponent>().m_speed = m_speed;
        m_start.GetComponent<LevelComponent>().ForceUpdate();
    }

    /// <summary>
    /// Public method that is called when the level components need to be stopped.
    /// If they are being stopped due to the game being over then the game over delegate is called.
    /// </summary>
    /// <param name="fall">This boolean is used to tell the method if the user fell off the map or not.</param>
    public void Stop(bool fall) {
        if (!gameOver) {
            //If the user fell then the level is slowly stopped if not then it is stopped instantly.
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

    //Method that is called by the pause event, the level components are given a new speed of 0 to stop them.
    public void Pause() {
        m_speed = 0f;
        UpdateVelocity();
    }

    //Method that is called by the pause event, the level components are given a new speed of 5 to start them moving again.
    public void UnPause() {
        m_speed = 5f;
        UpdateVelocity();
    }
    
    //Unused method that was to be used to calculate the angle of the level component to determine what way it should move.
    private float CalculateAngle(Transform pos1, Transform pos2) {
        float angle = 0f;

        float vectorX = pos1.position.x - pos2.position.x;
        float vectorY = pos1.position.y - pos2.position.y;

        angle = Mathf.Atan2(vectorY, vectorX);

        return angle;
    }

    //Unused method that was to be used to calculate the new veloctiy given the calculated angle.
    private Vector2 CalculateVelocity(float angle) {
        Vector2 velocity = new Vector2 {
            x = Mathf.Cos(angle),
            y = Mathf.Sin(angle)
        };

        return velocity;
    }

    //Method for disabling all the level components.
    private void DisableAllLevelComponents() {
        for (int i = 0; i < levelComponents.Count; i++) {
            levelComponents[i].SetActive(false);
        }
    }

    /// <summary>
    /// Method that is called when the level needs to be reset, this is called on its own when the Home button is pressed.
    /// This method sets the speed to 0 then disables the level components and renables the starting component
    /// The relevant booleans are set to false and the players character controls are frozen and the reset event is called.
    /// </summary>
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

    /// <summary>
    /// This method is called when the restart button is pressed.
    /// The level is reset using the above method then the game is restarted using the coroutine.
    /// </summary>
    private void RestartGame() {
        ResetLevel();

        StartCoroutine(Restart());
    }

    /// <summary>
    /// This coroutine gives the user a countdown of 3 seconds before the level starts moving again.
    /// </summary>
    /// <returns></returns>
    IEnumerator Restart() {
        playerMat.bounciness = 0;

        resetTimer.text = "3";
        resetTimer.gameObject.SetActive(true);

        yield return new WaitForSeconds(1);
        resetTimer.text = "2";

        yield return new WaitForSeconds(1);
        resetTimer.text = "1";

        yield return new WaitForSeconds(1);
        resetTimer.gameObject.SetActive(false);

        playerMat.bounciness = 0.1f;

        GameReadyToRestart();

        m_speed = 5;
        UpdateVelocity();
        player.FreezeControls(false);
        player.anim.SetBool("restart", false);
    }
}
