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
        EnablePlanetFunctions();
    }

    public void RestartButton()
    {
        StartTime();
        _screenOn = false;
        SceneManager.LoadScene(0);
        EnablePlanetFunctions();
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
        DisablePlanetFunctions();
    }

    public void WinScreen()
    {
        StopTime();
        _screenOn = true;
        UIManager._instance._winPanel.SetActive(true);
        UIManager._instance._backgroundPanel.SetActive(true);
        DisablePlanetFunctions();
    }

    private void PauseScreen(bool isPaused)
    {
        StopTime();
        UIManager._instance._pausePanel.SetActive(isPaused);
        UIManager._instance._backgroundPanel.SetActive(isPaused);
        DisablePlanetFunctions();
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

    private void DisablePlanetFunctions()
    {
        foreach (GameObject planet in PlanetManager.Instance._enemyPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = false;
        }

        foreach (GameObject planet in PlanetManager.Instance._friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = false;
        }

        foreach (GameObject planet in PlanetManager.Instance._neutralPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = false;
        }
    }

    private void EnablePlanetFunctions()
    {
        foreach (GameObject planet in PlanetManager.Instance._enemyPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = true;
        }

        foreach (GameObject planet in PlanetManager.Instance._friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = true;
        }

        foreach (GameObject planet in PlanetManager.Instance._neutralPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = true;
        }
    }

}
