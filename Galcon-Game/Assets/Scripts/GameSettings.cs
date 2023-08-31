using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private static int _numberOfRandomPlanets;
    private static int _numberOfEnemyPlanets;
    private static int _numberOfFriendlyPlanets;
    private static int _numberOfNeutralPlanets;
    private static bool _isRandomMap;
    private static bool _isCustomMap;
    public int NumberOfRandomPlanets { get; set; }
    public int NumberOfEnemyPlanets { get; set; }
    public int NumberOfFriendlyPlanets { get; set ; }
    public int NumberOfNeutralPlanets { get; set; }
    public bool IsRandomMap { get; set; } = false;
    public bool IsCustomMap { get; set; } = false;

    private static GameSettings _instance;

    public static GameSettings Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        UIManager._instance.UpdateNumOfShips(NumberOfFriendlyPlanets, NumberOfEnemyPlanets, NumberOfNeutralPlanets);
    }

    public bool MapMode()
    {
        if (IsRandomMap)
        {
            return true;
        }
        else if (IsCustomMap)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
