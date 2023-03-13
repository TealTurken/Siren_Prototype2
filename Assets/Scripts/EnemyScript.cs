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

    [NonSerialized]
    public bool MouseUp;

    private AudioSource enemyAudioSource;
    
    [SerializeField]
    private GameObject highlightParticles;

    void Start()
    {
        Cursor.visible = true;
        if (GameBoard == null) GameBoard = GameObject.FindGameObjectWithTag("Board");
        playerController = GameBoard.GetComponent<Player_Controller>();
        defaultColor = Color.white;
        highLightColor = Color.black;
        selectedColor = Color.red;
        enemyAudioSource = GetComponent<AudioSource>();
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
        if (!MouseUp) return;
        highlighted = false;
        Unhover();
    }

    public void HoverOver()
    {
        Instantiate(highlightParticles);
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
