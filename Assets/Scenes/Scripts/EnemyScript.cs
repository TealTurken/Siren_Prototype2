using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int enemySpeed = 10;
    public Rigidbody2D enemyRigidbody;
    public float moveCounter;
    private bool enemyClickedOn = false;
    public TMP_Text winText;
    void Start()
    {
        Cursor.visible = true;
        winText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.D)))
        {
            if (moveCounter > 0)
            {
                if (enemyClickedOn == false)
                {
                    enemyRigidbody.velocity = new Vector3(enemySpeed, 0, 0);
                    transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
                }
                moveCounter = moveCounter - 1;
                
                //transform.Translate(Vector3.right * Time.deltaTime * enemySpeed, Space.World);
                //if (Input.GetButton("Tilt Right"))


            }
        }
        if ((Input.GetKeyDown(KeyCode.A)))
        {
            if (moveCounter > 0)
            {
                if (enemyClickedOn == false)
                {
                    enemyRigidbody.velocity = new Vector3(-enemySpeed, 0, 0);
                    transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);

                }
                
                moveCounter = moveCounter - 1;
                
                //transform.Translate(Vector3.left * Time.deltaTime * enemySpeed, Space.World);
                //if ((Input.GetKeyDown(KeyCode.A)))
                //if (Input.GetButton("Tilt Left"))
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Enemy"))
        {
             enemyRigidbody.velocity = new Vector3(0, 0, 0);
           // Debug.Log("Hit Enemy");
        }
        if ((collision.gameObject.tag == "Wall"))
        {
            enemyRigidbody.velocity = new Vector3(0, 0, 0);
           // Debug.Log("Hit Wall");
        }
        if ((collision.gameObject.tag == "Goal"))
        {
            Destroy(collision.gameObject);
            winText.enabled = true;
            winText.text = "You Win".ToString();
        }

    }
    private void OnMouseDown()
    {
        enemyClickedOn = true;
        Debug.Log("Mouse is held down");
    }
    private void OnMouseUp()
    {
        enemyClickedOn = false;
    }
}
