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
    //public int enemySpeed = 1;
   // public Rigidbody2D enemyRigidbody;

    void Start()
    {
        moveCounterText.text = "Moves Remaining: " + moveCounter.ToString();
        gameOverText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (moveCounter > 0)
            {
                moveCounter = moveCounter - 1;
                moveCounterText.text = "Moves Remaining: " + moveCounter.ToString();
                gameBoard.transform.Rotate(0.0f, 0.0f, 90.0f);
                //Enemy.transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
                //Enemy.transform.Translate(Vector3.right * Time.deltaTime * enemySpeed, Space.World);



            }
        }
        if ((Input.GetKeyDown(KeyCode.A)))
        {
            if (moveCounter > 0)
            {
                moveCounter = moveCounter - 1;
                moveCounterText.text = "Moves Remaining: " + moveCounter.ToString();
                gameBoard.transform.Rotate(0.0f, 0.0f, -90.0f);
                //Enemy.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
               // Enemy.transform.Translate(Vector3.left * Time.deltaTime * enemySpeed, Space.World);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
                Debug.Log("restarted");
            }

            if (moveCounter <= 0)
            {
                gameOverText.enabled = true;
                gameOverText.text = "Game Over".ToString();
            }
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