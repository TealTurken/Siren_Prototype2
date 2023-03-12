using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCanvas;
    [SerializeField]
    private GameObject howToPlayCanvas;
    [SerializeField]
    private GameObject backButton;

    private bool isHowToPlay = false;

    void Start()
    {
        howToPlayCanvas.SetActive(false);
        backButton.SetActive(false);
    }

    public void PlayGame()
    {
        GameManager.Instance.ProceedLevel();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        if(!isHowToPlay)
        {
            mainCanvas.SetActive(false);
            howToPlayCanvas.SetActive(true);
            backButton.SetActive(true);
            isHowToPlay = true;
        }
        else
        {
            mainCanvas.SetActive(true);
            howToPlayCanvas.SetActive(false);
            backButton.SetActive(false);
            isHowToPlay = false;
        }
    }
}
