using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// CR: You should split the 'GameManager' into a 'GameManager' for the 'Game' Scene,
//     and a 'MainMenuManager' for the 'MainMenu' Scene.
//     most of the code here is irrelevant for the 'MainMenu' scene 
//     (and some is irrelevant for the 'Game' scene)

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
        if (UIManager.instance.losePanel == null)
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
    public void PlayRandomGame()
    {
        SceneManager.LoadScene("Game");
        GameSettings.instance.IsRandomMap = true;
        GameSettings.instance.NumberOfRandomPlanets = Random.Range(12, 15);
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
        SoundFx.instance.PlaySound(SoundFx.instance._gameStartSound, .3f);
    }

    public void StartGeneratedGame()
    {
        SceneManager.LoadScene("Game");
        GameSettings.instance.IsCustomMap = true;
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
        SoundFx.instance.PlaySound(SoundFx.instance._gameStartSound, .3f);
    }

    public void GenerateButton()
    {
        //UIManager._instance.mainMenuPanel.SetActive(false);
        //UIManager._instance.gameModePanel.SetActive(true);
        UIManager.instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        UIManager.instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
    }
    public void BackButton()
    {
        //UIManager._instance.mainMenuPanel.SetActive(true);
        //UIManager._instance.gameModePanel.SetActive(false);
        UIManager.instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        UIManager.instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
    }
    public void ResumeButton()
    {
        _isPaused = !_isPaused;
        PauseScreen(_isPaused);
        EnablePlanetFunctions();
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
        SoundFx.instance.PlaySound(SoundFx.instance._gameResumeSound, .3f);
    }

    public void RestartButton()
    {
        StartTime();
        _screenOn = false;
        SceneManager.LoadScene("Game");
        EnablePlanetFunctions();
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
        SoundFx.instance.PlaySound(SoundFx.instance._gameRestartSound, .3f);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
    }
    #endregion
    #region Screens
    public void LoseScreen()
    {
        StopTime();
        _screenOn = true;
        UIManager.instance.losePanel.SetActive(true);
        UIManager.instance.backgroundPanel.SetActive(true);
        DisablePlanetFunctions();
    }

    public void WinScreen()
    {
        StopTime();
        _screenOn = true;
        UIManager.instance.winPanel.SetActive(true);
        UIManager.instance.backgroundPanel.SetActive(true);
        DisablePlanetFunctions();
    }

    private void PauseScreen(bool isPaused)
    {
        StopTime();
        UIManager.instance.pausePanel.SetActive(isPaused);
        UIManager.instance.backgroundPanel.SetActive(isPaused);
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
        GameSettings.instance.NumberOfFriendlyPlanets = (int)UIManager.instance.numOfFriendlyShipsSlider.value;
        GameSettings.instance.NumberOfEnemyPlanets = (int)UIManager.instance.numOfEnemyShipsSlider.value;
        GameSettings.instance.NumberOfNeutralPlanets = (int)UIManager.instance.numOfNeutralShipsSlider.value;
        PlaySoundEachOneSlide(UIManager.instance.numOfFriendlyShipsSlider);
        PlaySoundEachOneSlide(UIManager.instance.numOfEnemyShipsSlider);
        PlaySoundEachOneSlide(UIManager.instance.numOfNeutralShipsSlider);
    }

    private void PlaySoundEachOneSlide(Slider slider)
    {
        for (int i = 0; i < slider.maxValue; i++)
        {
            if (Mathf.Approximately(slider.value, i))
            {
                SoundFx.instance.PlaySound(SoundFx.instance._selectSound, .3f);
            }
        }
    }


    #region Disable/Enable Planet Functions

    private void DisablePlanetFunctions()
    {
        MouseInputs.instance._isEnable = false;
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
