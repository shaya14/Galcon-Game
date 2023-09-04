using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetManager : Singleton<PlanetManager>
{
    [Header("Map Prefabs")]
    [SerializeField] private Planet _mapPlanet; // CR: rename: _planetPrefab
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

    private static PlanetManager _instance; // CR: delete since you're inheriting 'Instance'.
    public int _numOfShipsGenerated = 0;

    // CR: [discuss in class] how can we simplify the 'GameSettings' related code?
    protected override void Awake()
    {
        base.Awake();
        _mapPlanets = new List<Planet>();
        if (GameSettings.Instance != null)
        {
            MapMode(GameSettings.Instance.MapMode());
        }

        if (_randomMap)
        {
            if (GameSettings.Instance != null)
            {
                UpdateNumOfRandomPlanets(GameSettings.Instance.NumberOfRandomPlanets);
            }

            InstatiatePlanets();
            PlanetCollision();
        }

        if (_custonMap)
        {
            if (GameSettings.Instance != null)
            {
                UpdateNumOfPlanets(GameSettings.Instance.NumberOfFriendlyPlanets,
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
            planet.GetComponent<TargetGlow>().SetGlowOff();
        }
        _selectedPlanets.Clear();

        foreach (Planet enemy in neutralAndEnemyPlanets)
        {
            enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
        }
    }

    #region Instatiate Planets
    public void InstantiateSpecPlanets()
    {
        var position = new Vector3(Random.Range(6f, 8f), Random.Range(-4f, 4f), -0.1f);
        var size = 1.5f;
        InstatiateFor(_numberOfEnemyPlanets, position, PlanetColor.Enemy, 80, size);

        position = new Vector3(Random.Range(-8f, -6f), Random.Range(-4f, 4f), -0.1f);
        InstatiateFor(_numberOfFriendlyPlanets, position, PlanetColor.Friendly, 80, size);

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
            float size = Random.Range(0.6f, 1.5f);
            planet.shipPerSecond *= size;
            planet.shipPerMinute = planet.shipPerSecond * 60;
            planet.shipPerMinute = (int)planet.shipPerMinute;
            planet.SetSettings(planet.RandomizePlanetColor(), size);
            planet.UpdateMaxNumOfShipsText();
            planet.UpdateNumOfShipsText();
            planet.UpdateDefineState();
        }
    }

    private void InstatiateFor(float numOfPlanets, Vector3 position, PlanetColor planetColor, int numOfShips, float size) // Spec Map Enemy/Friendly
    {
        for (int i = 0; i < numOfPlanets; i++)
        {
            var planet = Instantiate<Planet>(_mapPlanet, position, Quaternion.identity);
            planet.SetSettings(planetColor, numOfShips, size);
            planet.transform.position = position;
            planet.shipPerSecond *= size;
            planet.shipPerMinute = planet.shipPerSecond * 60;
            planet.shipPerMinute = (int)planet.shipPerMinute;
            planet.UpdateMaxNumOfShipsText();
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
            planet.transform.position = position;
            planet.shipPerSecond *= size;
            planet.shipPerMinute = planet.shipPerSecond * 60;
            planet.shipPerMinute = (int)planet.shipPerMinute;
            planet.UpdateMaxNumOfShipsText();
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
                // CR: (nit) if a planet is not friendly or enemy, it's neutral.
                //     else {
                //       _mapPlanets[i].transform.position = ...
                //     }
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
    public void UpdateNumOfPlanets(int numberOfFriendlyShips, int numberOfEnemyShips, int numberOfNeutralShips)
    {
        _numberOfFriendlyPlanets = numberOfFriendlyShips;
        _numberOfEnemyPlanets = numberOfEnemyShips;
        _numberOfNeutralPlanets = numberOfNeutralShips;
    }

    public void UpdateNumOfRandomPlanets(int numberOfRandomPlanets)
    {
        _numberOfRandomPlanets = numberOfRandomPlanets;
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

    /// <summary>
    /// Thats The Same is the public List<Planet> friendlyPlanets
    /// </summary>
    // public List<Planet> GetFriendlyPlanets() {
    //    var planets = new List<Planet>();

    //     foreach (Planet planet in _mapPlanets) {
    //       if (planet.isFriendly) {
    //         planets.Add(planet);
    //       }
    //     }

    //     return planets;
    // } 

    public List<Planet> friendlyPlanets
    {
        get
        {
            var planets = new List<Planet>();

            foreach (Planet planet in _mapPlanets)
            {
                if (planet.isFriendly)
                {
                    planets.Add(planet);
                }
            }

            return planets;
        }
    }

    public List<Planet> enemyPlanets
    {
        get
        {
            var planets = new List<Planet>();

            foreach (Planet planet in _mapPlanets)
            {
                if (planet.isEnemy)
                {
                    planets.Add(planet);
                }
            }

            return planets;
        }
    }

    public List<Planet> neutralPlanets
    {
        get
        {
            var planets = new List<Planet>();

            foreach (Planet planet in _mapPlanets)
            {
                if (planet.isNeutral)
                {
                    planets.Add(planet);
                }
            }

            return planets;
        }
    }

    public List<Planet> neutralAndEnemyPlanets
    {
        get
        {
            var planets = new List<Planet>();

            foreach (Planet planet in _mapPlanets)
            {
                if (planet.isNeutral || planet.isEnemy)
                {
                    planets.Add(planet);
                }
            }

            return planets;
        }
    }
}
