using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private bool _isPaused = false;
    private bool _screenOn = false;
    void Start()
    {
        StartTime();
    }
    void Update()
    {
        if (GameUIManager.Instance.losePanel == null)
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
            if (PlanetManager.Instance.friendlyPlanets.Count <= 0)
            {
                LoseScreen();
            }
            else if (PlanetManager.Instance.enemyPlanets.Count <= 0)
            {
                WinScreen();
            }
        }
    }
    #region Buttons
    public void ResumeButton()
    {
        _isPaused = !_isPaused;
        PauseScreen(_isPaused);
        EnablePlanetFunctions();
        SoundFx.Instance.PlaySound(SoundFx.Instance.clickSound, .3f);
        SoundFx.Instance.PlaySound(SoundFx.Instance.gameResumeSound, .3f);
    }

    public void RestartButton()
    {
        StartTime();
        _screenOn = false;
        SceneManager.LoadScene("Game");
        EnablePlanetFunctions();
        SoundFx.Instance.PlaySound(SoundFx.Instance.clickSound, .3f);
        SoundFx.Instance.PlaySound(SoundFx.Instance.gameRestartSound, .3f);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
        SoundFx.Instance.PlaySound(SoundFx.Instance.clickSound, .3f);
        StartTime();
    }
    #endregion

    #region Screens
    public void LoseScreen()
    {
        StopTime();
        _screenOn = true;
        GameUIManager.Instance.losePanel.SetActive(true);
        GameUIManager.Instance.backgroundPanel.SetActive(true);
        DisablePlanetFunctions();
    }

    public void WinScreen()
    {
        StopTime();
        _screenOn = true;
        GameUIManager.Instance.winPanel.SetActive(true);
        GameUIManager.Instance.backgroundPanel.SetActive(true);
        DisablePlanetFunctions();
    }

    private void PauseScreen(bool isPaused)
    {
        StopTime();
        GameUIManager.Instance.pausePanel.SetActive(isPaused);
        GameUIManager.Instance.backgroundPanel.SetActive(isPaused);
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

    #region Disable/Enable Planet Functions

    private void DisablePlanetFunctions()
    {
        MouseInputs.Instance._isEnable = false;
    }

    private void EnablePlanetFunctions()
    {
        MouseInputs.Instance._isEnable = true;
    }
    #endregion
}
