using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public void PlayRandomGame()
    {
        SceneManager.LoadScene("Game");
        GameSettings.Instance.IsRandomMap = true;
        GameSettings.Instance.NumberOfRandomPlanets = Random.Range(12, 15);
        //SoundFx.Instance.PlaySound(SoundFx.Instance._clickSound, .3f);
        SoundFx.Instance.PlaySound(SoundFx.Instance._clickSound, .3f);
        SoundFx.Instance.PlaySound(SoundFx.Instance._gameStartSound, .3f);
    }

    public void StartGeneratedGame()
    {
        SceneManager.LoadScene("Game");
        GameSettings.Instance.IsCustomMap = true;
        SoundFx.Instance.PlaySound(SoundFx.Instance._clickSound, .3f);
        SoundFx.Instance.PlaySound(SoundFx.Instance._gameStartSound, .3f);
    }

    public void GenerateButton()
    {
        //UIManager._instance.mainMenuPanel.SetActive(false);
        //UIManager._instance.gameModePanel.SetActive(true);
        UIManager._instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        UIManager._instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        SoundFx.Instance.PlaySound(SoundFx.Instance._clickSound, .3f);
    }
    public void BackButton()
    {
        //UIManager._instance.mainMenuPanel.SetActive(true);
        //UIManager._instance.gameModePanel.SetActive(false);
        UIManager._instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        UIManager._instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        SoundFx.Instance.PlaySound(SoundFx.Instance._clickSound, .3f);
    }
    public void ResumeButton()
    {
        _isPaused = !_isPaused;
        PauseScreen(_isPaused);
        EnablePlanetFunctions();
        SoundFx.Instance.PlaySound(SoundFx.Instance._clickSound, .3f);
        SoundFx.Instance.PlaySound(SoundFx.Instance._gameResumeSound, .3f);
    }

    public void RestartButton()
    {
        StartTime();
        _screenOn = false;
        SceneManager.LoadScene("Game");
        EnablePlanetFunctions();
        SoundFx.Instance.PlaySound(SoundFx.Instance._clickSound, .3f);
        SoundFx.Instance.PlaySound(SoundFx.Instance._gameRestartSound, .3f);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
        SoundFx.Instance.PlaySound(SoundFx.Instance._clickSound, .3f);
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
        PlaySoundEachOneSlide(UIManager._instance.numOfFriendlyShipsSlider);
        PlaySoundEachOneSlide(UIManager._instance.numOfEnemyShipsSlider);
        PlaySoundEachOneSlide(UIManager._instance.numOfNeutralShipsSlider);
    }

    private void PlaySoundEachOneSlide(Slider slider)
    {
        for (int i = 0; i < slider.maxValue; i++)
        {
            if (Mathf.Approximately(slider.value, i))
            {
                SoundFx.Instance.PlaySound(SoundFx.Instance._selectSound, .3f);
            }
        }
    }


    #region Disable/Enable Planet Functions

    private void DisablePlanetFunctions()
    {
        MouseInputs.Instance._isEnable = false;
        foreach (Planet planet in PlanetManager.Instance.enemyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = false;
        }

        foreach (Planet planet in PlanetManager.Instance.friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = false;
        }

        foreach (Planet planet in PlanetManager.Instance.neutralPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = false;
        }
    }

    private void EnablePlanetFunctions()
    {
        MouseInputs.Instance._isEnable = true;
        foreach (Planet planet in PlanetManager.Instance.enemyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = true;
        }

        foreach (Planet planet in PlanetManager.Instance.friendlyPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = true;
        }

        foreach (Planet planet in PlanetManager.Instance.neutralPlanets)
        {
            planet.GetComponent<TargetGlow>()._isEnable = true;
        }
    }
    #endregion
}
