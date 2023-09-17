using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    public int numberOfRandomPlanets;
    public int numberOfEnemyPlanets;
    public int numberOfFriendlyPlanets;
    public int numberOfNeutralPlanets;
    public bool isRandomMap;
    public bool isCustomMap;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        MainMenuUIManager.instance.UpdateNumOfShips(numberOfFriendlyPlanets, numberOfEnemyPlanets, numberOfNeutralPlanets);
    }

    public bool MapMode()
    {
        if (isRandomMap)
        {
            return true;
        }
        else if (isCustomMap)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
