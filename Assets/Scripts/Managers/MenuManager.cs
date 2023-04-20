using System;
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

    private Button howToPlayButton;
    private Button howToPlayBackButton;
    private Button optionsButton;
    private Button optionsBackButton;

    private Slider volumeSlider;
    private Dropdown graphicsDropdown;
    private Toggle cheatToggle;

    private bool isHowToPlay = false;
    private bool isOptions = false;

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
        if (!PlayerPrefs.HasKey("Graphics"))
        {
            PlayerPrefs.SetInt("Graphics", 3);
        }
        if (!PlayerPrefs.HasKey("99Moves"))
        {
            PlayerPrefs.SetInt("99Moves", 0);
        }
    }

    public void InitializeUI()
    {
        mainCanvasGroup = GameObject.FindGameObjectsWithTag("MainCanvas")[0].GetComponent<CanvasGroup>();
        howToPlayCanvasGroup = GameObject.FindGameObjectsWithTag("HowToPlay")[0].GetComponent<CanvasGroup>();
        optionsCanvasGroup = GameObject.FindGameObjectsWithTag("Options")[0].GetComponent<CanvasGroup>();
       
        howToPlayButton = GameObject.Find("HowToPlay Button").GetComponent<Button>();
        optionsButton = GameObject.Find("Options Button").GetComponent<Button>();

        howToPlayButton.onClick.RemoveAllListeners();
        howToPlayButton.onClick.AddListener(HowToPlay);
        optionsButton.onClick.RemoveAllListeners();
        optionsButton.onClick.AddListener(Options);

        volumeSlider = GameObject.Find("Volume Slider").GetComponent<Slider>();
        graphicsDropdown = GameObject.Find("Graphics Dropdown").GetComponent<Dropdown>();
        cheatToggle = GameObject.Find("Cheat Toggle").GetComponent<Toggle>();

        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(SetVolume);
        graphicsDropdown.onValueChanged.RemoveAllListeners();
        graphicsDropdown.onValueChanged.AddListener(SetGraphics);
        cheatToggle.onValueChanged.RemoveAllListeners();
        cheatToggle.onValueChanged.AddListener(SetMovesCheat);

        howToPlayBackCanvasGroup = GameObject.FindGameObjectsWithTag("HowToPlay Back")[0].GetComponent<CanvasGroup>();
        optionsBackCanvasGroup = GameObject.FindGameObjectsWithTag("Options Back")[0].GetComponent<CanvasGroup>();

        howToPlayBackButton = GameObject.Find("HowToPlay Back Button").GetComponent<Button>();
        optionsBackButton = GameObject.Find("Options Back Button").GetComponent<Button>();

        howToPlayBackButton.onClick.RemoveAllListeners();
        howToPlayBackButton.onClick.AddListener(HowToPlay);
        optionsBackButton.onClick.RemoveAllListeners();
        optionsBackButton.onClick.AddListener(Options);

        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            howToPlayButton.onClick.AddListener(PauseGame);
            optionsButton.onClick.AddListener(PauseGame);

            howToPlayBackButton.onClick.AddListener(PauseGame);
            optionsBackButton.onClick.AddListener(PauseGame);
        }

        LoadValues();
    }

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

            howToPlayBackCanvasGroup.alpha = 1f;
            howToPlayBackCanvasGroup.interactable = true;
            howToPlayBackCanvasGroup.blocksRaycasts = true;

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

            howToPlayBackCanvasGroup.alpha = 0f;
            howToPlayBackCanvasGroup.interactable = false;
            howToPlayBackCanvasGroup.blocksRaycasts = false;

            clickSound.Play();

            isHowToPlay = false;
        }
    }

    public void Options()
    {
        if(!isOptions)
        {
            mainCanvasGroup.alpha = 0f;
            mainCanvasGroup.interactable = false;
            mainCanvasGroup.blocksRaycasts = false;

            optionsCanvasGroup.alpha = 1f;
            optionsCanvasGroup.interactable = true;
            optionsCanvasGroup.blocksRaycasts = true;

            optionsBackCanvasGroup.alpha = 1f;
            optionsBackCanvasGroup.interactable = true;
            optionsBackCanvasGroup.blocksRaycasts = true;

            clickSound.Play();

            isOptions = true;
        }
        else
        {
            mainCanvasGroup.alpha = 1f;
            mainCanvasGroup.interactable = true;
            mainCanvasGroup.blocksRaycasts = true;

            optionsCanvasGroup.alpha = 0f;
            optionsCanvasGroup.interactable = false;
            optionsCanvasGroup.blocksRaycasts = false;

            optionsBackCanvasGroup.alpha = 0f;
            optionsBackCanvasGroup.interactable = false;
            optionsBackCanvasGroup.blocksRaycasts = false;

            clickSound.Play();

            isOptions = false;
        }
    }

    public void SetVolume(float volumeValue)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volumeValue) * 20);
        PlayerPrefs.SetFloat("GameVolume", volumeValue);
    }

    public void SetGraphics(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Graphics", qualityIndex);
    }

    public void SetMovesCheat(bool cheatBool)
    {
         PlayerPrefs.SetInt("99Moves", Convert.ToInt32(cheatBool));
    }

    void LoadValues()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("GameVolume");
        graphicsDropdown.value = PlayerPrefs.GetInt("Graphics");
        cheatToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("99Moves"));

        SetVolume(PlayerPrefs.GetFloat("GameVolume"));
        SetGraphics(PlayerPrefs.GetInt("Graphics"));
        SetMovesCheat(Convert.ToBoolean(PlayerPrefs.GetInt("99Moves")));
    }
}
