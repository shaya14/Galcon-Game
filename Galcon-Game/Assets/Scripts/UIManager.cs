using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// CR: same as in GameManager - split this into a 'GameUIManager' and 'MainMenuUiManager'.
public class UIManager : Singleton<UIManager>
{
    public GameObject losePanel;
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject backgroundPanel;
    public GameObject mainMenuPanel;
    public GameObject gameModePanel;
    public Slider numOfFriendlyShipsSlider;
    public Slider numOfEnemyShipsSlider;
    public Slider numOfNeutralShipsSlider;
    public TextMeshProUGUI numOfFriendlyShipsText;
    public TextMeshProUGUI numOfEnemyShipsText;
    public TextMeshProUGUI numOfNeutralShipsText;

    public void UpdateNumOfShips(int numberOfFriendlyShips , int numberOfEnemyShips , int numberOfNeutralShips)
    {
        if(numOfFriendlyShipsSlider == null || numOfEnemyShipsSlider == null || numOfNeutralShipsSlider == null)
        {
            return;
        }
        numOfFriendlyShipsText.text = numberOfFriendlyShips.ToString();
        numOfEnemyShipsText.text = numberOfEnemyShips.ToString();
        numOfNeutralShipsText.text = numberOfNeutralShips.ToString();
    }   
}
