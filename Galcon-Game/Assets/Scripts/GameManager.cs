using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    private bool _isPaused = false;
    private bool _screenOn = false;
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        StartTime();
    }
    void Update()
    {
        if (!_screenOn)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isPaused = !_isPaused;
                PauseScreen(_isPaused);
            }
        }

        if (PlanetManager.Instance._friendlyPlanets.Count <= 0)
        {
            LoseScreen();
        }
        else if (PlanetManager.Instance._enemyPlanets.Count <= 0)
        {
            WinScreen();
        }
    }
    #region Buttons
    public void ResumeButton()
    {
        _isPaused = !_isPaused;
        PauseScreen(_isPaused);
    }

    public void RestartButton()
    {
        StartTime();
        _screenOn = false;
        SceneManager.LoadScene(0);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    #endregion
    #region Screens
    public void LoseScreen()
    {
        StopTime();
        _screenOn = true;
        UIManager._instance._losePanel.SetActive(true);
        UIManager._instance._backgroundPanel.SetActive(true);
    }

    public void WinScreen()
    {
        StopTime();
        _screenOn = true;
        UIManager._instance._winPanel.SetActive(true);
        UIManager._instance._backgroundPanel.SetActive(true);
    }

    private void PauseScreen(bool isPaused)
    {
        StopTime();
        UIManager._instance._pausePanel.SetActive(isPaused);
        UIManager._instance._backgroundPanel.SetActive(isPaused);
        if (!isPaused)
        {
            StartTime();
        }
    }
    #endregion

    #region Time
    private void StopTime()
    {
        Time.timeScale = 0;
    }

    private void StartTime()
    {
        Time.timeScale = 1;
    }
    #endregion
}
