using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    // CR: delete unsued
    private static int _numberOfRandomPlanets;
    private static int _numberOfEnemyPlanets;
    private static int _numberOfFriendlyPlanets;
    private static int _numberOfNeutralPlanets;
    private static bool _isRandomMap;
    private static bool _isCustomMap;
    // CR: (style) lowerCaseCamel: 'numberOfRandomPlanets', etc. (See 'camera.Main');
    public int NumberOfRandomPlanets { get; set; }
    public int NumberOfEnemyPlanets { get; set; }
    public int NumberOfFriendlyPlanets { get; set; }
    public int NumberOfNeutralPlanets { get; set; }
    public bool IsRandomMap { get; set; } = false;
    public bool IsCustomMap { get; set; } = false;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        UIManager.Instance.UpdateNumOfShips(NumberOfFriendlyPlanets, NumberOfEnemyPlanets, NumberOfNeutralPlanets);
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
