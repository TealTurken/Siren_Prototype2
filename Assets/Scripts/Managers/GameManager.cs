using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int level = 0;
    public int numberOfLevels = 3;

    [SerializeField]
    private AudioSource backgroundMusic;
    [SerializeField]
    private float pitchScale;

    private void Awake()
    {
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
            StartCoroutine(EndScreen(false));
        }
        else
        {
            StartCoroutine(EndScreen(true));
        }

    }

    IEnumerator EndScreen(bool gameWon)
    {
        yield return new WaitForSeconds(1.5f);

        if(!gameWon)
        {
            level += 1;
            SceneManager.LoadScene($"Level {level}");
            backgroundMusic.pitch += pitchScale;
        }
        else
        {
            level = 0;
            SceneManager.LoadScene("Main Menu");
            backgroundMusic.pitch = 1;
        }
    }
}
