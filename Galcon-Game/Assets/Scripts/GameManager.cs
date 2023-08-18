using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _mapPlanets;
    [SerializeField] private int _numberOfPlanets;

    public GameObject _attackingShips;
    public List<GameObject> _planets;
    public List<GameObject> _enemies;

    private static GameManager _instance;
    public static GameManager Instance { get; set; }

    void Start()
    {
        InstatiatePlanets();
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
            planet.GetComponent<Planet>()._isSelected = false;
            planet.GetComponent<TargetGlow>().SetGlowOff();
        }
        GameManager.Instance._planets.Clear();

        foreach (GameObject enemy in GameManager.Instance._enemies)
        {
            enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
        }
    }

    public void RemoveFromList(Planet enemy)
    {
        _enemies.Remove(enemy.gameObject);
    }

    public void InstatiatePlanets()
    {       
        for (int i = 0; i < _numberOfPlanets; i++)
        {
            var position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
            Instantiate(_mapPlanets, position, Quaternion.identity);
            _mapPlanets.GetComponent<Planet>().RandomizePlanet();
        }       
    }
}
