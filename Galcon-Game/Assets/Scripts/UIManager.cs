using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject _losePanel;
    public GameObject _winPanel;
    public GameObject _pausePanel;
    public GameObject _backgroundPanel;

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
}
