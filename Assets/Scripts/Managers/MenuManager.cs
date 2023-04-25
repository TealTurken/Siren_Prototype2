using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private CanvasGroup mainCanvasGroup;
    private CanvasGroup howToPlayCanvasGroup;
    private CanvasGroup howToPlayBackCanvasGroup;
    private CanvasGroup optionsCanvasGroup;
    private CanvasGroup optionsBackCanvasGroup;
    private CanvasGroup quitCanvasGroup;
    private CanvasGroup levelEndCanvasGroup;
    private CanvasGroup gameEndCanvasGroup;

    private Button howToPlayButton;
    private Button howToPlayBackButton;
    private Button optionsButton;
    private Button optionsBackButton;
    private Button quitButton;
    private Button quitBackButton;

    private Button quitYes;
    private Button quitNo;

    private Button gameEndMenuButton;
    private Button gameEndQuitButton;
    private TMP_Text levelEndText;

    private Slider volumeSlider;
    private Toggle fullscreenToggle;
    private Toggle cheatToggle;

    public bool isMenuCanvasActive = false;

    [SerializeField]
    private AudioSource clickSound;

    [SerializeField]
    private AudioMixer audioMixer;

    private bool isPaused = false;

    void Start()
    {
        if (!PlayerPrefs.HasKey("GameVolume"))
        {
            PlayerPrefs.SetFloat("GameVolume", 1);
        }
        if (!PlayerPrefs.HasKey("99Moves"))
        {
            PlayerPrefs.SetInt("99Moves", 0);
        }
    }

    //Called every new scene
    //Defines each canvas and button--clears then adds button listeners
    public void InitializeUI()
    {
        //Canvases
        mainCanvasGroup = GameObject.FindGameObjectsWithTag("MainCanvas")[0].GetComponent<CanvasGroup>();
        howToPlayCanvasGroup = GameObject.Find("HowToPlayCanvas").GetComponent<CanvasGroup>();
        optionsCanvasGroup = GameObject.Find("OptionsCanvas").GetComponent<CanvasGroup>();
        quitCanvasGroup = GameObject.Find("QuitCanvas").GetComponent<CanvasGroup>();
       
        //Canvas buttons
        howToPlayButton = GameObject.Find("HowToPlay Button").GetComponent<Button>();
        optionsButton = GameObject.Find("Options Button").GetComponent<Button>();
        quitButton = GameObject.Find("Quit Button").GetComponent<Button>();
        howToPlayButton.onClick.RemoveAllListeners();
        howToPlayButton.onClick.AddListener(HowToPlayCanvas);
        optionsButton.onClick.RemoveAllListeners();
        optionsButton.onClick.AddListener(OptionsCanvas);
        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(QuitCanvas);

        //Back canvases (for main menu)
        howToPlayBackCanvasGroup = GameObject.FindGameObjectsWithTag("HowToPlay Back")[0].GetComponent<CanvasGroup>();
        optionsBackCanvasGroup = GameObject.FindGameObjectsWithTag("Options Back")[0].GetComponent<CanvasGroup>();

        //Back buttons
        howToPlayBackButton = GameObject.Find("HowToPlay Back Button").GetComponent<Button>();
        optionsBackButton = GameObject.Find("Options Back Button").GetComponent<Button>();
        howToPlayBackButton.onClick.RemoveAllListeners();
        howToPlayBackButton.onClick.AddListener(HowToPlayCanvas);
        optionsBackButton.onClick.RemoveAllListeners();
        optionsBackButton.onClick.AddListener(OptionsCanvas);

        //Options controls
        volumeSlider = GameObject.Find("Volume Slider").GetComponent<Slider>();
        fullscreenToggle = GameObject.Find("Fullscreen Toggle").GetComponent<Toggle>();
        cheatToggle = GameObject.Find("Cheat Toggle").GetComponent<Toggle>();
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(SetVolume);
        fullscreenToggle.onValueChanged.RemoveAllListeners();
        fullscreenToggle.onValueChanged.AddListener(ToggleFullscreen);
        cheatToggle.onValueChanged.RemoveAllListeners();
        cheatToggle.onValueChanged.AddListener(SetMovesCheat);

        //Quit controls
        quitYes = GameObject.Find("YesButton").GetComponent<Button>();
        quitNo = GameObject.Find("NoButton").GetComponent<Button>();
        quitYes.onClick.RemoveAllListeners();
        quitYes.onClick.AddListener(Quit);
        quitNo.onClick.RemoveAllListeners();
        quitNo.onClick.AddListener(QuitCanvas);

        //Buttons only pause when not on main menu
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            howToPlayButton.onClick.AddListener(PauseGame);
            optionsButton.onClick.AddListener(PauseGame);
            quitButton.onClick.AddListener(PauseGame);

            howToPlayBackButton.onClick.AddListener(PauseGame);
            optionsBackButton.onClick.AddListener(PauseGame);
            quitNo.onClick.AddListener(PauseGame);
        }

        //Sets game end controls only when not on main menu
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            gameEndCanvasGroup = GameObject.Find("GameEndCanvas").GetComponent<CanvasGroup>();
            gameEndMenuButton = GameObject.Find("GameEndMenuButton").GetComponent<Button>();
            gameEndQuitButton = GameObject.Find("GameEndQuitButton").GetComponent<Button>();

            gameEndMenuButton.onClick.RemoveAllListeners();
            gameEndMenuButton.onClick.AddListener(RestartGame);
            gameEndQuitButton.onClick.RemoveAllListeners();
            gameEndQuitButton.onClick.AddListener(Quit);

            levelEndCanvasGroup = GameObject.Find("LevelEndCanvas").GetComponent<CanvasGroup>();
            levelEndText = GameObject.Find("LevelEndText").GetComponent<TMP_Text>();
        }

        //Initializes saved option data
        LoadValues();
    }

    //Pauses
    void PauseGame()
    {
        if(!isPaused)
        {
            Time.timeScale = 0.0f;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            isPaused = false;
        }
    }

    //Proceeds level
    public void PlayGame()
    {
        GameManager.Instance.ProceedLevel();
        clickSound.Play();
    }

    //Quits game
    public void Quit()
    {
        Application.Quit();
    }

    //Open and closes HowToPlay canvas
    public void HowToPlayCanvas()
    {
        if (!isMenuCanvasActive)
        {
            howToPlayBackCanvasGroup.alpha = 1f;
            howToPlayBackCanvasGroup.interactable = true;
            howToPlayBackCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            howToPlayBackCanvasGroup.alpha = 0f;
            howToPlayBackCanvasGroup.interactable = false;
            howToPlayBackCanvasGroup.blocksRaycasts = false;
        }
        ToggleCanvas(howToPlayCanvasGroup);
    }

    //Open and closes Options canvas
    public void OptionsCanvas()
    {
        if (!isMenuCanvasActive)
        {
            optionsBackCanvasGroup.alpha = 1f;
            optionsBackCanvasGroup.interactable = true;
            optionsBackCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            optionsBackCanvasGroup.alpha = 0f;
            optionsBackCanvasGroup.interactable = false;
            optionsBackCanvasGroup.blocksRaycasts = false;
        }
        ToggleCanvas(optionsCanvasGroup);
    }

    //Open and closes Quit canvas
    public void QuitCanvas()
    {
        ToggleCanvas(quitCanvasGroup);
    }

    //Open and closes Level End canvas
    public void LevelEndCanvas(bool levelWon)
    {
        if (levelWon) levelEndText.text = "Level Complete!";
        else levelEndText.text = "Level Failed!";
        ToggleCanvas(levelEndCanvasGroup);
    }

    //Open and closes Game End canvas
    public void GameEndCanvas()
    {
        ToggleCanvas(gameEndCanvasGroup);
    }

    public void RestartGame()
    {
        GameManager.Instance.restartClicked = true;
    }

    //Either opens or closes canvas parameter
    void ToggleCanvas(CanvasGroup canvasGroup)
    {
        if(!isMenuCanvasActive)
        {
            mainCanvasGroup.alpha = 0f;
            mainCanvasGroup.interactable = false;
            mainCanvasGroup.blocksRaycasts = false;

            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            clickSound.Play();

            isMenuCanvasActive = true;
        }
        else
        {
            mainCanvasGroup.alpha = 1f;
            mainCanvasGroup.interactable = true;
            mainCanvasGroup.blocksRaycasts = true;

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            clickSound.Play();

            isMenuCanvasActive = false;
        }
    }

    //Sets and saves volume
    public void SetVolume(float volumeValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat("GameVolume", volumeValue);
    }

    //Toggles fullscreen
    public void ToggleFullscreen(bool fullscreenBool)
    {
        Screen.fullScreen = fullscreenBool;
        PlayerPrefs.SetInt("Fullscreen", Convert.ToInt32(fullscreenBool));
    }

    //Sets and saves cheats
    public void SetMovesCheat(bool cheatBool)
    {
         PlayerPrefs.SetInt("99Moves", Convert.ToInt32(cheatBool));
    }

    //Initializes saved option data
    void LoadValues()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("GameVolume");
        fullscreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen"));
        cheatToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("99Moves"));

        SetVolume(PlayerPrefs.GetFloat("GameVolume"));
        ToggleFullscreen(Convert.ToBoolean(PlayerPrefs.GetInt("Fullscreen")));
        SetMovesCheat(Convert.ToBoolean(PlayerPrefs.GetInt("99Moves")));
    }
}
