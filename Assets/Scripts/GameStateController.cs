using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{
    [Header("Pause Menu ")] [SerializeField]
    public GameObject pauseMenu;

    [Header("Game Over Menu")] [SerializeField]
    public GameObject gameOverMenu;
    
    [Header("Score Menu")] [SerializeField]
    public GameObject ScoreGameObject;

    [Header("Camera UI")][SerializeField]
    public GameObject CameraUi;

    [Header("Player")] [SerializeField] public GameObject player;
    

    private bool _isPause;

    // Start is called before the first frame update
    void Start()
    {
        #region controll music
        FindObjectOfType<AudioManager>().PauseAll();
        FindObjectOfType<AudioManager>().Play(FindObjectOfType<AudioManager>().GetCurrentThemeName(SceneManager.GetActiveScene().name));
        #endregion

        #region getScoreFromPreviousLevel
        // if (FindObjectOfType<ScoreHolder>().ReadTempScore() != 0)
        // {
        //     GameObject.FindWithTag("ScoreText").GetComponent<Text>().text =
        //         FindObjectOfType<ScoreHolder>().ReadTempScore().ToString();
        // }
        #endregion
    }

    private String GetCurrentThemeName(string scenename)
    {
        switch (scenename)
        {
            case "Boss1Phase1":
                return "BossMusic";
                break;
            case "Boss1Phase2":
                return "BossMusic";
                break;
            default:
                return "Theme";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        #region disable menu

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverMenu.activeSelf)
        {
            if (!_isPause)
            {
                ActivePauseMenu();
            }
            else
            {
                ClosePauseMenu();
            }
        }

        #endregion
    }

    /// <summary>
    /// Set time scale to default
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1f;
        ResumePlayerControl();
    }

    /// <summary>
    /// froze timescale so that game be stopped
    /// </summary>
    void Pause()
    {
        //Supposed to 0, but it will trigger Way point enter a infinite loop, in this case 0.01f is used
        Time.timeScale = 0.001f;
        DisablePlayerControl();
    }

    /// <summary>
    /// restart this level
    /// </summary>
    public void Replay()
    {
        CloseGameOverMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Show Pause menu
    /// </summary>
    public void ActivePauseMenu()
    {
        CameraUi.SetActive(false);
        Pause();
        pauseMenu.SetActive(true);
       // audioManager.PauseTheme("Theme");
        FindObjectOfType<AudioManager>().PauseTheme(FindObjectOfType<AudioManager>().GetCurrentThemeName(SceneManager.GetActiveScene().name));
        _isPause = true;
    }

    /// <summary>
    /// Close Pause Menu
    /// </summary>
    public void ClosePauseMenu()
    {
        CameraUi.SetActive(true);
        Resume();
        pauseMenu.SetActive(false);
        FindObjectOfType<AudioManager>().ResumeTheme(FindObjectOfType<AudioManager>().GetCurrentThemeName(SceneManager.GetActiveScene().name));
 
        _isPause = false;
    }

    /// <summary>
    /// Close Game Over Menu
    /// </summary>
    public void CloseGameOverMenu()
    {
        CameraUi.SetActive(true);
        ActiveScoreMenu();
        gameOverMenu.SetActive(false);
        Resume();
    }

    /// <summary>
    /// Show Up Game Over Menu, Game will be paused
    /// </summary>
    public void ActiveGameOverMenu()
    {
        CameraUi.SetActive(false);
        //Debug.Log(GameObject.FindWithTag("ScoreText").GetComponent<Text>().text.ToString() + " 6986");
        //FindObjectOfType<HighScoreTable>().AddNewScoreEntry(int.Parse(GameObject.FindWithTag("ScoreText").GetComponent<Text>().text.ToString()));
        //FindObjectOfType<ScoreHolder>().StoreScoreToDatabase();
        //FindObjectOfType<ScoreHolder>().ResetTempScore();
        CloseScoreMenu();
        Pause();
        gameOverMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        //FindObjectOfType<AudioManager>().ResumeTheme(FindObjectOfType<AudioManager>().GetCurrentThemeName(SceneManager.GetActiveScene().name));
        //FindObjectOfType<ScoreHolder>().StoreScoreToDatabase();   
        //FindObjectOfType<ScoreHolder>().ResetTempScore();
        SceneManager.LoadScene(0);
    }

    public void DisablePlayerControl()
    {
        player.GetComponent<PlayerController>().enabled = false;
    }

    public void ResumePlayerControl()
    {
        player.GetComponent<PlayerController>().enabled = true;
    }

    public void Exit()
    {
        Debug.Log("quit game!");
        Application.Quit();
    }

    public void ActiveScoreMenu()
    {
        ScoreGameObject.SetActive(true);
    }

    public void CloseScoreMenu()
    {
        ScoreGameObject.SetActive(false);
    }
}