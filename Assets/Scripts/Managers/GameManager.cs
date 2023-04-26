using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int level = 0;
    public int numberOfLevels = 5;

    [SerializeField]
    private AudioSource backgroundMusic;
    [SerializeField]
    private float pitchScale;

    [SerializeField]
    private float timeBeforeSceneChange;

    public bool restartClicked = false;

    private MenuManager menuManager;

    private void Awake()
    {
        menuManager = GetComponent<MenuManager>();

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ProceedLevel()
    {
        if (level == 0)
        {
            level += 1;
            SceneManager.LoadScene($"Level {level}");
        }
        else if (level < numberOfLevels)
        {
            StartCoroutine(WinScreen(false));
        }
        else
        {
            StartCoroutine(WinScreen(true));
        }
    }

    public void RestartLevel()
    {
        StartCoroutine(FailScreen());
    }

    IEnumerator FailScreen()
    {   
        FindObjectOfType<Player_Controller>().isPaused = true;
        menuManager.LevelEndCanvas(false);
        yield return new WaitForSeconds(timeBeforeSceneChange);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        menuManager.isMenuCanvasActive = false;
        FindObjectOfType<Player_Controller>().isPaused = false;
    }

    IEnumerator WinScreen(bool gameWon)
    {
        FindObjectOfType<Player_Controller>().isPaused = true;
        if (!gameWon)
        {
            menuManager.LevelEndCanvas(true);
            yield return new WaitForSeconds(timeBeforeSceneChange);
            level += 1;
            SceneManager.LoadScene($"Level {level}");
            menuManager.isMenuCanvasActive = false;
            backgroundMusic.pitch += pitchScale; 
            FindObjectOfType<Player_Controller>().isPaused = false;
        }
        else
        {
            menuManager.GameEndCanvas();
            yield return new WaitUntil(() => restartClicked == true);
            level = 0;
            SceneManager.LoadScene("Main Menu");
            menuManager.isMenuCanvasActive = false;
            backgroundMusic.pitch = 1;
            restartClicked = false;
            FindObjectOfType<Player_Controller>().isPaused = false;
        }
    }
}
