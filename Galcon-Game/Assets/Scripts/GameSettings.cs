using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    // CR: There is no benefit to having these be properties. simplify:
    //       public in numberOfRandomPlanets;
    public int numberOfRandomPlanets { get; set; }
    public int numberOfEnemyPlanets { get; set; }
    public int numberOfFriendlyPlanets { get; set; }
    public int numberOfNeutralPlanets { get; set; }
    public bool isRandomMap { get; set; } = false;
    public bool isCustomMap { get; set; } = false;

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
