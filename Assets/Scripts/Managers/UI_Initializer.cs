using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [SerializeField]
    private MenuManager menuManager;

    void Start()
    {
        if(GameObject.Find("Manager") != null)
        {
            menuManager = GameObject.Find("Manager").GetComponent<MenuManager>();
            menuManager.InitializeUI();
        }
        else
        {
            Debug.Log("Manager not in the scene! (Start from  main menu to bring it along)");
        }
    }
}
