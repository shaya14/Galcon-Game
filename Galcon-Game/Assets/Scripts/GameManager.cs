using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _attackingShips;
    public List<GameObject> _planets;
    public List<GameObject> _enemies;

    private static GameManager _instance;
    public static GameManager Instance { get; set; }

    void Start()
    {
        Instance = this;
        _planets = new List<GameObject>();
        _enemies = new List<GameObject>();
        _enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        _enemies.AddRange(GameObject.FindGameObjectsWithTag("Neutral"));
    }

    public void SpawnShips()
    {
        foreach (GameObject planet in GameManager.Instance._planets)
        {
            planet.GetComponent<Planet>()._numberOfShips /= 2;
            planet.GetComponent<Planet>().UpdateNumOfShipsText();
            for (int i = 0; i < planet.GetComponent<Planet>()._numberOfShips; i++)
            {
                GameObject ship = Instantiate(_attackingShips, planet.transform.position, Quaternion.identity);
            }
        }

        foreach (GameObject planet in GameManager.Instance._planets)
        {
            planet.GetComponent<SpriteRenderer>().color = planet.GetComponent<Planet>()._defaultColor;
            planet.GetComponent<Planet>()._isSelected = false;
            planet.GetComponent<TargetGlow>().SetGlowOff();
        }
        GameManager.Instance._planets.Clear();

        foreach (GameObject enemy in GameManager.Instance._enemies)
        {
            enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
        }
    }
}
