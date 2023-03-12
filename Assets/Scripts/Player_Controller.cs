using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    [Header("Parameters")]
    public TMP_Text moveCounterText;
    public TMP_Text gameOverText;
    public TMP_Text winText;
    public float moveCounter;
    public GameObject gameBoard;
    [NonSerialized]
    public GameObject[] Pieces;
    private int goaledPieces;
    private int totalPieces;

    [Space]

    [NonSerialized]
    public Quaternion currentAngle;
    Quaternion angle_00 = Quaternion.Euler(0, 0, 0); // <-- origin angle
    Quaternion angle_90 = Quaternion.Euler(0, 0, 90);
    Quaternion angle_180 = Quaternion.Euler(0, 0, 180);
    Quaternion angle_270 = Quaternion.Euler(0, 0, 270);
    bool isRotateRight; // whether or not the board is being rotated Right or Left
    
    [Header("Controls")]
    public InputAction RotateRight;
    public InputAction RotateLeft;
    public InputAction Restart;

    void Start()
    {
        moveCounterText.text = "Moves Remaining: " + moveCounter.ToString();
        gameOverText.enabled = false;
        currentAngle = angle_00;
        Pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in Pieces)
            if (piece.GetComponent<EnemyScript>().goal != null) totalPieces++;
    }

    private void OnEnable()
    {
        RotateRight.Enable();
        RotateLeft.Enable();
        Restart.Enable();
    }

    private void OnDisable()
    {
        RotateRight.Disable();
        RotateLeft.Disable();
        Restart.Disable();
    }

    void Update()
    {
        #region Controls
        if (RotateRight.triggered)
        {
            if (moveCounter > 0)
            {
                UpdateMoveCounter();
                isRotateRight = true;
                DoRotate();
            }
        }
        if (RotateLeft.triggered)
        {
            if (moveCounter > 0)
            {
                isRotateRight = false;
                UpdateMoveCounter();
                DoRotate();
            }
        }
        // Restart v
        if (Restart.triggered)
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
            if (isRotateRight) currentAngle = angle_270;
            else currentAngle = angle_90;
            return;
        }
        else if (currentAngle == angle_90)
        {
            if (isRotateRight) currentAngle = angle_00;
            else currentAngle = angle_180;
            return;
        }
        else if (currentAngle == angle_180)
        {
            if (isRotateRight) currentAngle = angle_90;
            else currentAngle = angle_270;
            return;
        }
        else if (currentAngle == angle_270)
        {
            if (isRotateRight) currentAngle = angle_180;
            else currentAngle = angle_00;
            return;
        }
    }

    public void LevelCompletion()
    {
        goaledPieces++;
        print(goaledPieces + " out of " + totalPieces);
        if (goaledPieces >= totalPieces)
        {
            winText.enabled = true;
            winText.text = "You Win".ToString();
        }
    }

    public void RotateBoard() // handles actual rotation of the board
    {
        gameBoard.transform.rotation = Quaternion.Slerp(gameBoard.transform.rotation, currentAngle, 0.02f);
        //foreach (GameObject piece in Pieces)
            //piece.transform.rotation = Quaternion.Slerp(piece.transform.rotation, currentAngle, 0.02f);
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
            Debug.Log("Hit Wall");
        }
    }
}