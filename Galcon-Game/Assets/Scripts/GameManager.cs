using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// CR: reminder to use camera-space canvas for the background, to avoid the blue borders.

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
        if (GameUIManager.instance.losePanel == null)
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

        if (PlanetManager.instance != null)
        {
            if (PlanetManager.instance.friendlyPlanets.Count <= 0)
            {
                LoseScreen();
            }
            else if (PlanetManager.instance.enemyPlanets.Count <= 0)
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
        SoundFx.instance.PlaySound(SoundFx.instance.clickSound, .3f);
        SoundFx.instance.PlaySound(SoundFx.instance.gameResumeSound, .3f);
    }

    public void RestartButton()
    {
        StartTime();
        _screenOn = false;
        SceneManager.LoadScene("Game");
        EnablePlanetFunctions();
        SoundFx.instance.PlaySound(SoundFx.instance.clickSound, .3f);
        SoundFx.instance.PlaySound(SoundFx.instance.gameRestartSound, .3f);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
        SoundFx.instance.PlaySound(SoundFx.instance.clickSound, .3f);
        StartTime();
    }
    #endregion

    #region Screens
    public void LoseScreen()
    {
        StopTime();
        _screenOn = true;
        GameUIManager.instance.losePanel.SetActive(true);
        GameUIManager.instance.backgroundPanel.SetActive(true);
        DisablePlanetFunctions();
    }

    public void WinScreen()
    {
        StopTime();
        _screenOn = true;
        GameUIManager.instance.winPanel.SetActive(true);
        GameUIManager.instance.backgroundPanel.SetActive(true);
        DisablePlanetFunctions();
    }

    private void PauseScreen(bool isPaused)
    {
        StopTime();
        GameUIManager.instance.pausePanel.SetActive(isPaused);
        GameUIManager.instance.backgroundPanel.SetActive(isPaused);
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
        MouseInputs.instance._isEnable = false;
        
        // CR: instead of duplicating this code 3 times, how about:
        //       foreach (Planet planet in PlanetManager.instance.allPlanets) {
        //         planet.GetComponent<TargetGlow>()._isEnable = false;
        //       }
        //      
        //      In PlanetManager - just add:
        //        List<Planet> allPlanets => _mapPlanets;
        //
        //      Same below.
         

        foreach (Planet planet in PlanetManager.instance.enemyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = false;
        }

        foreach (Planet planet in PlanetManager.instance.friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = false;
        }

        foreach (Planet planet in PlanetManager.instance.neutralPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = false;
        }
    }

    private void EnablePlanetFunctions()
    {
        MouseInputs.instance._isEnable = true;
        foreach (Planet planet in PlanetManager.instance.enemyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = true;
        }

        foreach (Planet planet in PlanetManager.instance.friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = true;
        }

        foreach (Planet planet in PlanetManager.instance.neutralPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = true;
        }
    }
    #endregion
}
