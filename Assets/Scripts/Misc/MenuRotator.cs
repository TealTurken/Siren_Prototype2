using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRotator : MonoBehaviour
{
    [SerializeField]
    private float rotateInterval = 3f;
    private float timePassed = 0.0f;

    [SerializeField]
    private Player_Controller controller;

    void Start()
    {
        controller = GetComponent<Player_Controller>();
        controller.isRotateRight = true;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if(timePassed > rotateInterval)
        {
            controller.DoRotate();
            timePassed = 0.0f;

            Debug.Log("yo");
        } 
    }
}
