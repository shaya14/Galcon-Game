using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuUIManager : Singleton<MainMenuUIManager>
{
    public GameObject mainMenuPanel;
    public GameObject gameModePanel;
    public Slider numOfFriendlyShipsSlider;
    public Slider numOfEnemyShipsSlider;
    public Slider numOfNeutralShipsSlider;
    public TextMeshProUGUI numOfFriendlyShipsText;
    public TextMeshProUGUI numOfEnemyShipsText;
    public TextMeshProUGUI numOfNeutralShipsText;

    public void UpdateNumOfShips(int numberOfFriendlyShips, int numberOfEnemyShips, int numberOfNeutralShips)
    {
        if (numOfFriendlyShipsSlider == null || numOfEnemyShipsSlider == null || numOfNeutralShipsSlider == null)
        {
            return;
        }
        numOfFriendlyShipsText.text = numberOfFriendlyShips.ToString();
        numOfEnemyShipsText.text = numberOfEnemyShips.ToString();
        numOfNeutralShipsText.text = numberOfNeutralShips.ToString();
    }
}
