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
    private int selectedPiece = 0; // always start with first piece selected.
    private int goaledPieces;
    private int totalPieces;

    [Space]

    [NonSerialized]
    public Quaternion currentAngle;
    Quaternion angle_00 = Quaternion.Euler(0, 0, 0); // <-- origin angle
    Quaternion angle_90 = Quaternion.Euler(0, 0, 90);
    Quaternion angle_180 = Quaternion.Euler(0, 0, 180);
    Quaternion angle_270 = Quaternion.Euler(0, 0, 270);
    [NonSerialized]
    public bool isRotateRight; // whether or not the board is being rotated Right or Left
    
    [Header("Controls")]
    private InputAction RotateRight;
    private InputAction RotateLeft;
    private InputAction Restart;
    private InputAction CyclePieceRight;
    private InputAction CyclePieceLeft;
    private InputAction Select;
    public PlayerInputActions playerControls;

    [SerializeField]
    private AudioClip rotateClip;
    [SerializeField]
    private AudioSource playerAudioSource;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        if (moveCounterText != null) moveCounterText.text = "Moves Remaining: " + moveCounter.ToString();
        if (gameOverText != null) gameOverText.enabled = false;
        currentAngle = angle_00;
        Pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in Pieces)
            if (piece.GetComponent<EnemyScript>().goal != null) totalPieces++;
    }

    private void OnEnable()
    {
        RotateRight = playerControls.GameBoard.RotateRight;
        RotateRight.Enable();
        RotateLeft = playerControls.GameBoard.RotateLeft;
        RotateLeft.Enable();
        Restart = playerControls.GameBoard.Restart;
        Restart.Enable();
        CyclePieceLeft = playerControls.GameBoard.CyclePieceLeft;
        CyclePieceLeft.Enable();
        CyclePieceRight = playerControls.GameBoard.CyclePieceRight;
        CyclePieceRight.Enable();
        Select = playerControls.GameBoard.Select;
        Select.Enable();
    }

    private void OnDisable()
    {
        RotateRight.Disable();
        RotateLeft.Disable();
        Restart.Disable();
        CyclePieceLeft.Disable();
        CyclePieceRight.Disable();
        Select.Disable();
    }

    void Update()
    {
        #region Controls
        if (RotateRight.triggered)
        {
            if (moveCounter > 0)
            {
                isRotateRight = true;
                UpdateMoveCounter();
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
        
        if (Restart.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("restarted");
        }
        
        if (CyclePieceRight.triggered)
        {
            if (selectedPiece < Pieces.Length - 1)
            {
                Pieces[selectedPiece].GetComponent<EnemyScript>().highlighted = false;
                Pieces[selectedPiece].GetComponent<EnemyScript>().Unhover();
                selectedPiece++;
                Pieces[selectedPiece].GetComponent<EnemyScript>().highlighted = true;
                Pieces[selectedPiece].GetComponent<EnemyScript>().HoverOver();
            }
            else if (selectedPiece == Pieces.Length - 1)
            {
                Pieces[selectedPiece].GetComponent<EnemyScript>().highlighted = false;
                Pieces[selectedPiece].GetComponent<EnemyScript>().Unhover();
                selectedPiece = 0;
                Pieces[selectedPiece].GetComponent<EnemyScript>().highlighted = true;
                Pieces[selectedPiece].GetComponent<EnemyScript>().HoverOver();
            }
            print(Pieces[selectedPiece].name.ToString());
        }
        
        if (CyclePieceLeft.triggered)
        {
            if (selectedPiece > 0)
            {
                Pieces[selectedPiece].GetComponent<EnemyScript>().highlighted = false;
                Pieces[selectedPiece].GetComponent<EnemyScript>().Unhover();
                selectedPiece--;
                Pieces[selectedPiece].GetComponent<EnemyScript>().highlighted = true;
                Pieces[selectedPiece].GetComponent<EnemyScript>().HoverOver();
            }
            else if (selectedPiece == 0)
            {
                Pieces[selectedPiece].GetComponent<EnemyScript>().highlighted = false;
                Pieces[selectedPiece].GetComponent<EnemyScript>().Unhover();
                selectedPiece = Pieces.Length - 1;
                Pieces[selectedPiece].GetComponent<EnemyScript>().highlighted = true;
                Pieces[selectedPiece].GetComponent<EnemyScript>().HoverOver();
            }
            print(Pieces[selectedPiece].name.ToString());
        }

        if (Select.triggered)
        {
            print("Select Triggered");
            if (Pieces[selectedPiece].GetComponent<EnemyScript>().MouseUp == false)
            {
                Pieces[selectedPiece].GetComponent<EnemyScript>().MouseUp = true;
                Pieces[selectedPiece].GetComponent<SpriteRenderer>().color = Pieces[selectedPiece].GetComponent<EnemyScript>().defaultColor;
            }
            else
            {
                Pieces[selectedPiece].GetComponent<EnemyScript>().MouseUp = false;
                Pieces[selectedPiece].GetComponent<EnemyScript>().Unmagnetize(Pieces[selectedPiece]);
            }
        }
        #endregion controls
        RotateBoard();
    }

    public void DoRotate() // input for directions
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
        print(goaledPieces + " out of " + totalPieces + " pieces goaled");
        if (goaledPieces >= totalPieces)
        {
            winText.enabled = true;
            winText.text = "You Win".ToString();
        }
    }

    public void RotateBoard() // handles actual rotation of the board
    {
        gameBoard.transform.rotation = Quaternion.Slerp(gameBoard.transform.rotation, currentAngle, 0.02f);
    }

    private void UpdateMoveCounter()
    {
        moveCounter -= 1;
        if (moveCounterText != null) moveCounterText.text = "Moves Remaining: " + moveCounter.ToString();
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