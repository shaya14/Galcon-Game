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
    public void PlayRandomGame()
    {
        SceneManager.LoadScene("Game");
        GameSettings.Instance.IsRandomMap = true;
        GameSettings.Instance.NumberOfRandomPlanets = Random.Range(12, 15);
    }

    public void StartGeneratedGame()
    {
        SceneManager.LoadScene("Game");
        GameSettings.Instance.IsCustomMap = true;
    }

    public void GenerateButton()
    {
        //UIManager._instance.mainMenuPanel.SetActive(false);
        //UIManager._instance.gameModePanel.SetActive(true);
        UIManager._instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        UIManager._instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

    }
    public void BackButton()
    {
        //UIManager._instance.mainMenuPanel.SetActive(true);
        //UIManager._instance.gameModePanel.SetActive(false);
        UIManager._instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        UIManager._instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);

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
            EnablePlanetFunctions();
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
        GameSettings.Instance.NumberOfFriendlyPlanets = (int)UIManager._instance.numOfFriendlyShipsSlider.value;
        GameSettings.Instance.NumberOfEnemyPlanets = (int)UIManager._instance.numOfEnemyShipsSlider.value;
        GameSettings.Instance.NumberOfNeutralPlanets = (int)UIManager._instance.numOfNeutralShipsSlider.value;
    }


    #region Disable/Enable Planet Functions

    private void DisablePlanetFunctions()
    {
        MouseInputs.Instance._isEnable = false;
        foreach (Planet planet in PlanetManager.Instance._enemyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = false;
        }

        foreach (Planet planet in PlanetManager.Instance._friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = false;
        }

        foreach (Planet planet in PlanetManager.Instance._neutralPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = false;
        }
    }

    private void EnablePlanetFunctions()
    {
        MouseInputs.Instance._isEnable = true;
        foreach (Planet planet in PlanetManager.Instance._enemyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = true;
        }

        foreach (Planet planet in PlanetManager.Instance._friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = true;
        }

        foreach (Planet planet in PlanetManager.Instance._neutralPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = true;
        }
    }
    #endregion
}
