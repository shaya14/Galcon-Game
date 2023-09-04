using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region Buttons
    public void PlayRandomGame()
    {
        SceneManager.LoadScene("Game");
        GameSettings.instance.isRandomMap = true;
        GameSettings.instance.numberOfRandomPlanets = Random.Range(12, 15);
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
        SoundFx.instance.PlaySound(SoundFx.instance._gameStartSound, .3f);
    }

    public void StartGeneratedGame()
    {
        SceneManager.LoadScene("Game");
        GameSettings.instance.isCustomMap = true;
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
        SoundFx.instance.PlaySound(SoundFx.instance._gameStartSound, .3f);
    }

    public void GenerateButton()
    {
        MainMenuUIManager.instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        MainMenuUIManager.instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
    }
    public void BackButton()
    {
        MainMenuUIManager.instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        MainMenuUIManager.instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        SoundFx.instance.PlaySound(SoundFx.instance._clickSound, .3f);
    }
    #endregion
    public void SliderValueChange()
    {
        GameSettings.instance.numberOfFriendlyPlanets = (int)MainMenuUIManager.instance.numOfFriendlyShipsSlider.value;
        GameSettings.instance.numberOfEnemyPlanets = (int)MainMenuUIManager.instance.numOfEnemyShipsSlider.value;
        GameSettings.instance.numberOfNeutralPlanets = (int)MainMenuUIManager.instance.numOfNeutralShipsSlider.value;
        PlaySoundEachOneSlide(MainMenuUIManager.instance.numOfFriendlyShipsSlider);
        PlaySoundEachOneSlide(MainMenuUIManager.instance.numOfEnemyShipsSlider);
        PlaySoundEachOneSlide(MainMenuUIManager.instance.numOfNeutralShipsSlider);
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
}
