using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject goal;
    public GameObject GameBoard;
    private Player_Controller playerController;

    [Space]

    public Rigidbody2D enemyRigidbody;
    public float moveCounter;
    public TMP_Text winText;
    public Quaternion selfAngle;
    Quaternion angle_00 = Quaternion.Euler(0, 0, 0); // <-- origin angle
    Quaternion angle_90 = Quaternion.Euler(0, 0, 90);
    Quaternion angle_180 = Quaternion.Euler(0, 0, 180);
    Quaternion angle_270 = Quaternion.Euler(0, 0, 270);
    bool MouseUp;

    void Start()
    {
        Cursor.visible = true;
        winText.enabled = false;
        if (GameBoard == null) GameBoard = GameObject.FindGameObjectWithTag("Board");
        playerController = GameBoard.GetComponent<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Wall")) & MouseUp == true)
        {
            this.transform.SetParent(collision.transform);
        }
        if (collision.gameObject == goal)
        {
            Destroy(collision.gameObject);
            playerController.LevelCompletion();
        }
    }
    
    private void OnMouseDown()
    {
        MouseUp = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null && hit.collider.CompareTag("Piece"))
        {
            Debug.Log(hit.collider.name.ToString());
            hit.collider.transform.SetParent(null);
        }
    }

    private void OnMouseUp()
    {
        MouseUp = true;
    }
}
