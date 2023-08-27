using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private static int _numberOfRandomPlanets;
    private static int _numberOfEnemyPlanets;
    private static int _numberOfFriendlyPlanets;
    private static int _numberOfNeutralPlanets;

    public int NumberOfRandomPlanets { get; set; }
    public int NumberOfEnemyPlanets { get; set; }
    public int NumberOfFriendlyPlanets { get; set ; }
    public int NumberOfNeutralPlanets { get; set; }


    public static GameSettings _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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
}
