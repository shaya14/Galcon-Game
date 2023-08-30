using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [Header("Map Prefabs")]
    [SerializeField] private Planet _mapPlanet;
    public Ship _attackingShips;

    [Header("Map Settings")]
    [SerializeField] private int _numberOfRandomPlanets;
    [SerializeField] private int _numberOfEnemyPlanets;
    [SerializeField] private int _numberOfFriendlyPlanets;
    [SerializeField] private int _numberOfNeutralPlanets;
    [SerializeField] private float _distanceBetweenPlanets = 2f;

    [Header("Game Mode")]
    [SerializeField] private bool _randomMap;
    [SerializeField] public bool _custonMap;

    [Header("Map & Settings Planets")]
    [SerializeField] private List<Planet> _mapPlanets;
    public List<Planet> _selectedPlanets;
    public List<Planet> _enemiesToSelect;

    [Header("Game Planets")]
    public List<Planet> _friendlyPlanets;
    public List<Planet> _enemyPlanets;
    public List<Planet> _neutralPlanets;
    private static PlanetManager _instance;
    public static PlanetManager Instance { get; set; }
    public int _numOfShipsGenerated = 0;

    public void NewList(List<Ship> list)
    {
        list = new List<Ship>();
    }
    public void DeleteList(List<Ship> list)
    {
        foreach (Ship ship in list)
        {
            Destroy(ship.gameObject);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        _mapPlanets = new List<Planet>();
        if (GameSettings.Instance != null)
        {
            MapMode(GameSettings.Instance.MapMode());
        }

        if (_randomMap)
        {
            if (GameSettings.Instance != null)
            {
                UpdateNumOfRandomShips(GameSettings.Instance.NumberOfRandomPlanets);
            }

            InstatiatePlanets();
            PlanetCollision();
        }

        if (_custonMap)
        {
            if (GameSettings.Instance != null)
            {
                UpdateNumOfShips(GameSettings.Instance.NumberOfFriendlyPlanets,
                GameSettings.Instance.NumberOfEnemyPlanets,
                GameSettings.Instance.NumberOfNeutralPlanets);
            }
            InstantiateSpecPlanets();
            PlanetCollision();
        }
    }

    void Start()
    {
        _selectedPlanets = new List<Planet>();
        _enemiesToSelect = new List<Planet>();
        _friendlyPlanets = new List<Planet>();
        _enemyPlanets = new List<Planet>();

        var planets = FindObjectsOfType<Planet>();
        foreach (Planet planet in planets)
        {
            switch (planet.planetColor)
            {
                case PlanetColor.Friendly:
                    _friendlyPlanets.Add(planet);
                    break;
                case PlanetColor.Enemy:
                    _enemiesToSelect.Add(planet);
                    _enemyPlanets.Add(planet);
                    break;
                case PlanetColor.Neutral:
                    _enemiesToSelect.Add(planet);
                    _neutralPlanets.Add(planet);
                    break;
            }
        }
    }

    public void UpdateLists(Planet planet)
    {
        if (planet.planetColor == PlanetColor.Friendly)
        {
            _friendlyPlanets.Add(planet);
            if (_enemyPlanets.Contains(planet))
            {
                _enemyPlanets.Remove(planet);
            }
            else if (_neutralPlanets.Contains(planet))
            {
                _neutralPlanets.Remove(planet);
            }
        }
        else if (planet.planetColor == PlanetColor.Enemy)
        {
            _enemyPlanets.Add(planet);
            _enemiesToSelect.Add(planet);
            if (_friendlyPlanets.Contains(planet))
            {
                _friendlyPlanets.Remove(planet);
            }
            else if (_neutralPlanets.Contains(planet))
            {
                _neutralPlanets.Remove(planet);
            }
        }
    }

    public void SpawnShips()
    {
        foreach (Planet planet in _selectedPlanets)
        {
            planet.numberOfShips /= 2;
            planet.UpdateNumOfShipsText();
            for (int i = 0; i < planet.numberOfShips; i++)
            {
                Ship ship = Instantiate<Ship>(_attackingShips, planet.transform.position, Quaternion.identity);
                ship.ShipColor("blue");
                _numOfShipsGenerated++;
            }
        }

        foreach (Planet planet in _selectedPlanets)
        {
            planet.isSelected = false;
            planet.GetComponent<TargetGlow>().SetGlowOff();
        }
        _selectedPlanets.Clear();

        foreach (Planet enemy in _enemiesToSelect)
        {
            enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
        }
    }

    #region Instatiate Planets
    public void InstantiateSpecPlanets()
    {
        var position = new Vector3(Random.Range(6f, 8f), Random.Range(-4f, 4f), -0.1f);
        var size = 1.5f;
        InstatiateFor(_numberOfEnemyPlanets, position, PlanetColor.Enemy, 80, 100, size);

        position = new Vector3(Random.Range(-8f, -6f), Random.Range(-4f, 4f), -0.1f);
        InstatiateFor(_numberOfFriendlyPlanets, position, PlanetColor.Friendly, 80, 100, size);

        position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
        InstatiateFor(_numberOfNeutralPlanets, position, PlanetColor.Neutral);
    }
    public void InstatiatePlanets() // Random Map
    {
        for (int i = 0; i < _numberOfRandomPlanets; i++)
        {
            var planet = Instantiate<Planet>(_mapPlanet);
            _mapPlanets.Add(planet);
            planet.gameObject.SetActive(false);
        }

        foreach (Planet planet in _mapPlanets)
        {
            var position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
            planet.transform.position = position;
            planet.SetSettings(planet.RandomizePlanetColor(), Random.Range(0.6f, 1.5f));
            planet.UpdateMaxNumOfShipsText();
            planet.UpdateNumOfShipsText();
            planet.UpdateDefineState();
        }
    }

    private void InstatiateFor(float numOfPlanets, Vector3 position, PlanetColor planetColor, int numOfShips, int maxShips, float size) // Spec Map Enemy/Friendly
    {
        for (int i = 0; i < numOfPlanets; i++)
        {
            var planet = Instantiate<Planet>(_mapPlanet, position, Quaternion.identity);
            planet.SetSettings(planetColor, numOfShips, maxShips, size);
            planet.UpdateMaxNumOfShipsText();
            planet.transform.position = position;
            planet.gameObject.SetActive(false);
            _mapPlanets.Add(planet);
        }
    }

    private void InstatiateFor(float numOfPlanets, Vector3 position, PlanetColor planetColor) // Spec Map Neutral
    {
        for (int i = 0; i < numOfPlanets; i++)
        {
            var planet = Instantiate<Planet>(_mapPlanet, position, Quaternion.identity);
            var size = Random.Range(0.6f, 1.5f);
            planet.SetSettings(planetColor, size);
            planet.UpdateMaxNumOfShipsText();
            planet.transform.position = position;
            planet.gameObject.SetActive(false);
            _mapPlanets.Add(planet);
        }
    }


    #endregion
    #region Planet Collision
    private bool HasCollisions(int planetIndex)
    {
        for (int i = 0; i < planetIndex; i++)
        {
            if (Vector3.Distance(_mapPlanets[planetIndex].transform.position, _mapPlanets[i].transform.position) < _distanceBetweenPlanets)
            {
                return true;
            }
        }

        return false;
    }

    public void PlanetCollision()
    {
        for (int i = 0; i < _mapPlanets.Count; i++)
        {
            while (HasCollisions(i))
            {

                if (_mapPlanets[i].isFriendly)
                {
                    _mapPlanets[i].transform.position = new Vector3(Random.Range(-8f, -6f), Random.Range(-4f, 4f), -0.1f);
                }
                else if (_mapPlanets[i].isEnemy)
                {
                    _mapPlanets[i].transform.position = new Vector3(Random.Range(6f, 8f), Random.Range(-4f, 4f), -0.1f);
                }
                else if (_mapPlanets[i].isNeutral)
                {
                    _mapPlanets[i].transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
                }
                else
                {
                }
            }
            _mapPlanets[i].gameObject.SetActive(true);
        }
    }
    #endregion
    #region Update Num Of Ships & Map Mode
    public void UpdateNumOfShips(int numberOfFriendlyShips, int numberOfEnemyShips, int numberOfNeutralShips)
    {
        _numberOfFriendlyPlanets = numberOfFriendlyShips;
        _numberOfEnemyPlanets = numberOfEnemyShips;
        _numberOfNeutralPlanets = numberOfNeutralShips;
    }

    public void UpdateNumOfRandomShips(int numberOfRandomShips)
    {
        _numberOfRandomPlanets = numberOfRandomShips;
    }

    public bool MapMode(bool mapMode)
    {
        if (GameSettings.Instance.IsRandomMap)
        {
            return _randomMap = true;
        }
        else if (GameSettings.Instance.IsCustomMap)
        {
            return _custonMap = true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
