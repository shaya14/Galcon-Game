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
        GameSettings.Instance.isRandomMap = true;
        GameSettings.Instance.numberOfRandomPlanets = Random.Range(12, 15);
        SoundFx.Instance.PlaySound(SoundFx.Instance.clickSound, .3f);
        SoundFx.Instance.PlaySound(SoundFx.Instance.gameStartSound, .3f);
    }

    public void StartGeneratedGame()
    {
        SceneManager.LoadScene("Game");
        GameSettings.Instance.isCustomMap = true;
        SoundFx.Instance.PlaySound(SoundFx.Instance.clickSound, .3f);
        SoundFx.Instance.PlaySound(SoundFx.Instance.gameStartSound, .3f);
    }

    public void GenerateButton()
    {
        MainMenuUIManager.Instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        MainMenuUIManager.Instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        SoundFx.Instance.PlaySound(SoundFx.Instance.clickSound, .3f);
    }
    public void BackButton()
    {
        MainMenuUIManager.Instance.mainMenuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        MainMenuUIManager.Instance.gameModePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        SoundFx.Instance.PlaySound(SoundFx.Instance.clickSound, .3f);
    }
    #endregion
    public void SliderValueChange()
    {
        GameSettings.Instance.numberOfFriendlyPlanets = (int)MainMenuUIManager.Instance.numOfFriendlyShipsSlider.value;
        GameSettings.Instance.numberOfEnemyPlanets = (int)MainMenuUIManager.Instance.numOfEnemyShipsSlider.value;
        GameSettings.Instance.numberOfNeutralPlanets = (int)MainMenuUIManager.Instance.numOfNeutralShipsSlider.value;
        PlaySoundEachOneSlide(MainMenuUIManager.Instance.numOfFriendlyShipsSlider);
        PlaySoundEachOneSlide(MainMenuUIManager.Instance.numOfEnemyShipsSlider);
        PlaySoundEachOneSlide(MainMenuUIManager.Instance.numOfNeutralShipsSlider);
    }

    private void PlaySoundEachOneSlide(Slider slider)
    {
        for (int i = 0; i < slider.maxValue; i++)
        {
            if (Mathf.Approximately(slider.value, i))
            {
                SoundFx.Instance.PlaySound(SoundFx.Instance.selectSound, .3f);
            }
        }
    }
}
