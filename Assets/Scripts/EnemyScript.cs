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
    private float magnetizeTime = 0f;
    private Color highLightColor;
    private Color selectedColor;
    private Player_Controller playerController;
    private GameObject validwall;
    private ConstantForce2D constForce2D;

    [Space]

    [NonSerialized]
    public bool MouseUp;

    private AudioSource enemyAudioSource;
    
    private GameObject highlightParticles;
    private GameObject highlightParticlesInstance;
    private GameObject collisionParticles;
    private GameObject victoryParticles;

    void Start()
    {
        Cursor.visible = true;
        if (GameBoard == null) GameBoard = GameObject.FindGameObjectWithTag("Board");
        playerController = GameBoard.GetComponent<Player_Controller>();
        defaultColor = Color.white;
        highLightColor = Color.black;
        selectedColor = Color.red;
        enemyAudioSource = GetComponent<AudioSource>();
        constForce2D = GetComponent<ConstantForce2D>();

        highlightParticles = Resources.Load<GameObject>("SelectHighlight");
        collisionParticles = Resources.Load<GameObject>("CollisionParticles");
        victoryParticles = Resources.Load<GameObject>("VictoryParticles");
    }

    private void Update()
    {
        if (MouseUp && this.transform.parent == null) this.transform.SetParent(validwall.transform);

        if (highlightParticlesInstance != null) highlightParticlesInstance.transform.position = this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.gameObject.CompareTag("Wall")) // only concerned with this if our piece has no parent object
            {
                validwall = collision.gameObject;
                enemyAudioSource.Play(); // impact with wall noise
                Vector2 midpoint = Vector2.Lerp(collision.gameObject.transform.position, gameObject.transform.position, 0.5f);
                Instantiate(collisionParticles, midpoint, collision.gameObject.transform.rotation);
            }
            if (collision.gameObject == goal)
            {
                Instantiate(victoryParticles, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                Destroy(collision.gameObject);
                playerController.LevelCompletion(); // update level completion progress
            }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) magnetizeTime += Time.deltaTime;
        if (magnetizeTime >= 1.0f && !this.transform.parent.CompareTag("Wall")) // prevents sticking to walls from an initial contact
        {
            this.transform.SetParent(validwall.transform);
            this.GetComponent<Rigidbody2D>().freezeRotation = false;
            constForce2D.force = new Vector2(0f, 0f);
            if (validwall.transform.parent.name == "BottomTilemap") constForce2D.relativeForce = new Vector2(0f, -9.8f);
            else constForce2D.relativeForce = new Vector2(0f, 9.8f);
            switch (validwall.transform.parent.name.ToString()) // determining which wall the piece is attaching to by name
            {
                case "TopTilemap":
                    this.transform.rotation = new Quaternion(0, 0, 180, 0);
                    break;
                case "LeftTilemap":
                    this.transform.rotation = new Quaternion(0, 0, -90, 0);
                    break;
                case "RightTilemap":
                    this.transform.rotation = new Quaternion(0, 0, 90, 0);
                    break;
                case "BottomTilemap":
                    this.transform.rotation = new Quaternion(0, 0, 0, 0);
                    break;
                default:
                    break;
            }
            this.GetComponent<Rigidbody2D>().freezeRotation = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        magnetizeTime = 0f;
    }

    private void OnMouseDown()
    {
        MouseUp = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        Debug.Log(hit.collider.name.ToString());
        if (hit.collider != null && hit.collider.CompareTag("Piece"))
        {
            hit.collider.transform.SetParent(this.transform.parent.parent);
            constForce2D.force = new Vector2(0f, -9.8f);
            constForce2D.relativeForce = new Vector2(0f, 0f);
            GetComponent<SpriteRenderer>().color = selectedColor;
            magnetizeTime = 0f;
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
        GameObject[] particles = GameObject.FindGameObjectsWithTag("SelectParticles");
        if (particles.Length == 0) 
        {
            highlightParticlesInstance = Instantiate(highlightParticles, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (Vector2.Distance(particles[i].transform.position, gameObject.transform.position) > 0.1f)
                {
                    Destroy(particles[i]);
                    highlightParticlesInstance = Instantiate(highlightParticles, gameObject.transform.position, Quaternion.identity);
                }     
            }     
        }
        
        GetComponent<SpriteRenderer>().color = highLightColor;
    }

    public void Unhover()
    {
        MouseUp = true;
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    public void Unmagnetize(GameObject piece)
    {
        piece.transform.SetParent(this.transform.parent.parent);
        constForce2D.force = new Vector2 (0f, -9.8f);
        constForce2D.relativeForce = new Vector2(0f, 0f);
        GetComponent<SpriteRenderer>().color = selectedColor;
        magnetizeTime = 0f;
    }
}
