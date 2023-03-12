using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCanvas;
    private CanvasGroup mainCanvasGroup;
    [SerializeField]
    private GameObject howToPlayCanvas;
    private CanvasGroup howToPlayCanvasGroup;
    [SerializeField]
    private GameObject backCanvas;
    private CanvasGroup backCanvasGroup;

    private bool isHowToPlay = false;

    [SerializeField]
    private AudioSource clickSound;

    void Start()
    {
        mainCanvasGroup = mainCanvas.GetComponent<CanvasGroup>();
        howToPlayCanvasGroup = howToPlayCanvas.GetComponent<CanvasGroup>();
        backCanvasGroup = backCanvas.GetComponent<CanvasGroup>();
    }

    public void PlayGame()
    {
        GameManager.Instance.ProceedLevel();
        clickSound.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        if(!isHowToPlay)
        {
            mainCanvasGroup.alpha = 0f;
            mainCanvasGroup.interactable = false;
            mainCanvasGroup.blocksRaycasts = false;

            howToPlayCanvasGroup.alpha = 1f;
            howToPlayCanvasGroup.interactable = true;
            howToPlayCanvasGroup.blocksRaycasts = true;

            backCanvasGroup.alpha = 1f;
            backCanvasGroup.interactable = true;
            backCanvasGroup.blocksRaycasts = true;

            clickSound.Play();

            isHowToPlay = true;
        }
        else
        {
            mainCanvasGroup.alpha = 1f;
            mainCanvasGroup.interactable = true;
            mainCanvasGroup.blocksRaycasts = true;

            howToPlayCanvasGroup.alpha = 0f;
            howToPlayCanvasGroup.interactable = false;
            howToPlayCanvasGroup.blocksRaycasts = false;

            backCanvasGroup.alpha = 0f;
            backCanvasGroup.interactable = false;
            backCanvasGroup.blocksRaycasts = false;

            clickSound.Play();

            isHowToPlay = false;
        }
    }
}
