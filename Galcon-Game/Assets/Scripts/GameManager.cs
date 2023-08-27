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
        if (UIManager._instance.losePanel == null)
        {
            return;
        }
        if (!_screenOn)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isPaused = !_isPaused;
                PauseScreen(_isPaused);
            }
        }

        if (PlanetManager.Instance != null)
        {
            if (PlanetManager.Instance._friendlyPlanets.Count <= 0)
            {
                LoseScreen();
            }
            else if (PlanetManager.Instance._enemyPlanets.Count <= 0)
            {
                WinScreen();
            }
        }
    }
    #region Buttons
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
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
        SceneManager.LoadScene("Game");
        EnablePlanetFunctions();
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion
    #region Screens
    public void LoseScreen()
    {
        StopTime();
        _screenOn = true;
        UIManager._instance.losePanel.SetActive(true);
        UIManager._instance.backgroundPanel.SetActive(true);
        DisablePlanetFunctions();
    }

    public void WinScreen()
    {
        StopTime();
        _screenOn = true;
        UIManager._instance.winPanel.SetActive(true);
        UIManager._instance.backgroundPanel.SetActive(true);
        DisablePlanetFunctions();
    }

    private void PauseScreen(bool isPaused)
    {
        StopTime();
        UIManager._instance.pausePanel.SetActive(isPaused);
        UIManager._instance.backgroundPanel.SetActive(isPaused);
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

    public void SliderValueChange()
    {
        // PlanetManager.Instance.SetNumberOfFriendlyShips((int)UIManager._instance.numOfFriendlyShipsSlider.value);
        // PlanetManager.Instance.SetNumberOfEnemyShips((int)UIManager._instance.numOfEnemyShipsSlider.value);
        // PlanetManager.Instance.SetNumberOfNeutralShips((int)UIManager._instance.numOfNeutralShipsSlider.value);

        GameSettings._instance.NumberOfFriendlyPlanets = (int)UIManager._instance.numOfFriendlyShipsSlider.value;
        GameSettings._instance.NumberOfEnemyPlanets = (int)UIManager._instance.numOfEnemyShipsSlider.value;
        GameSettings._instance.NumberOfNeutralPlanets = (int)UIManager._instance.numOfNeutralShipsSlider.value;
    }

    public void StartGeneratedGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void DisablePlanetFunctions()
    {
        foreach (Planet planet in PlanetManager.Instance._enemyPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = false;
        }

        foreach (Planet planet in PlanetManager.Instance._friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = false;
        }

        foreach (Planet planet in PlanetManager.Instance._neutralPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = false;
        }
    }

    private void EnablePlanetFunctions()
    {
        foreach (Planet planet in PlanetManager.Instance._enemyPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = true;
        }

        foreach (Planet planet in PlanetManager.Instance._friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = true;
        }

        foreach (Planet planet in PlanetManager.Instance._neutralPlanets)
        {
            planet.GetComponent<TargetGlow>().enabled = true;
        }
    }

}
