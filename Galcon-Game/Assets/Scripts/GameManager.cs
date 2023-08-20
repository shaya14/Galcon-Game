using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public bool _isPaused = false;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;
            PauseScreen(_isPaused);
        }

        if (PlanetManager.Instance._friendlyPlanets.Count == 0)
        {
            LoseScreen();
        }
        else if (PlanetManager.Instance._enemyPlanets.Count == 0)
        {
            WinScreen();
        }
    }
    #region Buttons
    public void PauseButton()
    {
        _isPaused = !_isPaused;
        PauseScreen(_isPaused);
    }

    public void RestartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        PauseScreen(false);
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
        UIManager._instance._losePanel.SetActive(true);
    }

    public void WinScreen()
    {
        StopTime();
        UIManager._instance._winPanel.SetActive(true);
    }

    private void PauseScreen(bool isPaused)
    {
        StopTime();
        UIManager._instance._pausePanel.SetActive(isPaused);
        if (!isPaused)
        {
            StartTime();
        }
    }
    #endregion
    private void StopTime()
    {
        Time.timeScale = 0;
    }

    private void StartTime()
    {
        Time.timeScale = 1;
    }
}
