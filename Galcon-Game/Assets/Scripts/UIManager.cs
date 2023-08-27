using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject losePanel;
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject backgroundPanel;
    public GameObject mainMenuPanel;
    public Slider numOfFriendlyShipsSlider;
    public Slider numOfEnemyShipsSlider;
    public Slider numOfNeutralShipsSlider;
    public TextMeshProUGUI numOfFriendlyShipsText;
    public TextMeshProUGUI numOfEnemyShipsText;
    public TextMeshProUGUI numOfNeutralShipsText;

    public static UIManager _instance;
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
    }

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
