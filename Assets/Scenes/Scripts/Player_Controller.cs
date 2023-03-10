using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    public TMP_Text moveCounterText;
    public TMP_Text gameOverText;
    public float moveCounter;
    public GameObject gameBoard;
    public GameObject Enemy;
    public Quaternion currentAngle;
    Quaternion angle_00 = Quaternion.Euler(0, 0, 0); // <-- origin angle
    Quaternion angle_90 = Quaternion.Euler(0, 0, 90);
    Quaternion angle_180 = Quaternion.Euler(0, 0, 180);
    Quaternion angle_270 = Quaternion.Euler(0, 0, 270);
    bool rotateRight; // whether or not the board is being rotated Right or Left

    void Start()
    {
        moveCounterText.text = "Moves Remaining: " + moveCounter.ToString();
        gameOverText.enabled = false;
        currentAngle = angle_00;
    }

    void Update()
    {
        #region Controls
        // Rotate Right v
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (moveCounter > 0)
            {
                UpdateMoveCounter();
                rotateRight = true;
                DoRotate();
            }
        }
        // Rotate Left v
        if ((Input.GetKeyDown(KeyCode.A))) 
        {
            if (moveCounter > 0)
            {
                rotateRight = false;
                UpdateMoveCounter();
                DoRotate();
            }
        }
        // Restart v
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
            Debug.Log("restarted");
        }
        #endregion controls
        
        RotateBoard();
    }

    void DoRotate() // input for directions
    {
        if (currentAngle == angle_00)
        {
            if (rotateRight) currentAngle = angle_270;
            else currentAngle = angle_90;
            return;
        }
        if (currentAngle == angle_90)
        {
            if (rotateRight) currentAngle = angle_00;
            else currentAngle = angle_180;
            return;
        }
        if (currentAngle == angle_180)
        {
            if (rotateRight) currentAngle = angle_90;
            else currentAngle = angle_270;
            return;
        }
        if (currentAngle == angle_270)
        {
            if (rotateRight) currentAngle = angle_180;
            else currentAngle = angle_00;
            return;
        }
    }

    public void RotateBoard() // handles actual rotation of the board
    {
        gameBoard.transform.rotation = Quaternion.Slerp(gameBoard.transform.rotation, currentAngle, 0.02f);
    }

    private void UpdateMoveCounter()
    {
        moveCounter -= 1;
        moveCounterText.text = "Moves Remaining: " + moveCounter.ToString();
        if (moveCounter <= 0)
        {
            gameOverText.enabled = true;
            gameOverText.text = "Game Over".ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Enemy"))
        {
           // enemyRigidbody.velocity = new Vector3(0, 0, 0);
            Debug.Log("Hit Enemy");
        }
        if ((collision.gameObject.tag == "Wall"))
        {
            //enemyRigidbody.velocity = new Vector3(0, 0, 0);
            Debug.Log("Hit Wall");
        }
    }
}