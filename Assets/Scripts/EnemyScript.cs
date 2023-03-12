using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject goal;
    public GameObject GameBoard;
    [NonSerialized]
    public bool highlighted = false;
    [NonSerialized]
    public Color defaultColor;
    private Color highLightColor;
    private Color selectedColor;
    private Player_Controller playerController;
    private GameObject validwall;

    [Space]

    public Rigidbody2D enemyRigidbody;
    public float moveCounter;
    public TMP_Text winText;
    public Quaternion selfAngle;
    Quaternion angle_00 = Quaternion.Euler(0, 0, 0); // <-- origin angle
    Quaternion angle_90 = Quaternion.Euler(0, 0, 90);
    Quaternion angle_180 = Quaternion.Euler(0, 0, 180);
    Quaternion angle_270 = Quaternion.Euler(0, 0, 270);
    [NonSerialized]
    public bool MouseUp;

    private AudioSource enemyAudioSource;

    void Start()
    {
        Cursor.visible = true;
        winText.enabled = false;
        if (GameBoard == null) GameBoard = GameObject.FindGameObjectWithTag("Board");
        playerController = GameBoard.GetComponent<Player_Controller>();
        defaultColor = GetComponent<SpriteRenderer>().color;
        highLightColor = defaultColor.gamma * 2;
        selectedColor = Color.grey;
        enemyAudioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (MouseUp && this.transform.parent == null) this.transform.SetParent(validwall.transform);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            validwall = collision.gameObject;
            enemyAudioSource.Play();
        }
        if ((collision.gameObject.CompareTag("Wall")) & MouseUp == true)
        {
            this.transform.SetParent(collision.transform); // magnetize when MouseUp is true (not clicked on)
        }
        if (collision.gameObject == goal)
        {
            Destroy(collision.gameObject);
            playerController.LevelCompletion(); // update level completion progress
        }
    }

    private void OnMouseDown()
    {
        MouseUp = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        Debug.Log(hit.collider.name.ToString());
        if (hit.collider != null && hit.collider.CompareTag("Piece"))
        {
            hit.collider.transform.SetParent(null);
            GetComponent<SpriteRenderer>().color = selectedColor;
        }
    }
    private void OnMouseUp()
    {
        MouseUp = true;
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    private void OnMouseEnter()
    {
        highlighted = true;
        HoverOver();
    }

    private void OnMouseExit()
    {
        highlighted = false;
        Unhover();
    }

    public void HoverOver()
    {
        // SPAWN PARTICLES HERE
        GetComponent<SpriteRenderer>().color = highLightColor;
    }

    public void Unhover()
    {
        MouseUp = true;
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    public void Unmagnetize(GameObject piece)
    {
        piece.transform.SetParent(null);
        GetComponent<SpriteRenderer>().color = selectedColor;
    }
}
