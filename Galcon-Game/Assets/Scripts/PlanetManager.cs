using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CR: try to keep object referneces as 'Planet' or 'Ship' instead of 'GameObject' ...
//     This will reduce the need for 'GetComponent' calls.

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

    [Header("Game Mode")]
    [SerializeField] private bool _randomeGenetate;
    [SerializeField] private bool _specificGenerate;

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

        if (_randomeGenetate)
        {
            InstatiatePlanets();
            _specificGenerate = false;
        }
        if (_specificGenerate)
        {
            InstatiateSpecificPlanets();
            PlanetCollision();
            _randomeGenetate = false;
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

    public void ClearListsFromDifrentPlanet()
    {
        foreach (Planet planet in _friendlyPlanets)
        {
            if (planet.planetColor != PlanetColor.Friendly)
            {
                if (planet.planetColor == PlanetColor.Enemy)
                {
                    _enemyPlanets.Add(planet);
                    _enemiesToSelect.Add(planet);
                }
                _friendlyPlanets.Remove(planet);
            }
        }

        foreach (Planet planet in _enemyPlanets)
        {
            if (planet.planetColor != PlanetColor.Enemy)
            {
                if (planet.planetColor == PlanetColor.Friendly)
                {
                    _friendlyPlanets.Add(planet);
                }
                _enemyPlanets.Remove(planet);
                _enemiesToSelect.Remove(planet);
            }
        }

        foreach (Planet planet in _neutralPlanets)
        {
            if (planet.planetColor != PlanetColor.Neutral)
            {
                if (planet.planetColor == PlanetColor.Friendly)
                {
                    _friendlyPlanets.Add(planet);
                }
                else if (planet.planetColor == PlanetColor.Enemy)
                {
                    _enemyPlanets.Add(planet);
                    _enemiesToSelect.Add(planet);
                }
                _neutralPlanets.Remove(planet);
                _enemiesToSelect.Remove(planet);
            }
        }

        foreach (Planet planet in _enemiesToSelect)
        {
            if (planet.planetColor != PlanetColor.Enemy && planet.planetColor != PlanetColor.Neutral)
            {
                _enemiesToSelect.Remove(planet);
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
            planet._numberOfShips /= 2;
            planet.UpdateNumOfShipsText();
            for (int i = 0; i < planet._numberOfShips; i++)
            {
                Ship ship = Instantiate<Ship>(_attackingShips, planet.transform.position, Quaternion.identity);
                ship.ShipColor("blue");
            }
        }

        foreach (Planet planet in _selectedPlanets)
        {
            planet._isSelected = false;
            planet.GetComponent<TargetGlow>().SetGlowOff();
        }
        _selectedPlanets.Clear();

        foreach (Planet enemy in _enemiesToSelect)
        {
            enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
        }
    }

    public void InstatiatePlanets()
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
            for (int i = 0; i < _mapPlanets.Count; i++)
            {
                if (Vector3.Distance(position, _mapPlanets[i].transform.position) < 1.5f)
                {
                    position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
                    i = 0;
                }
            }
            planet.transform.position = position;
            planet.gameObject.SetActive(true);
            planet.PlanetSize();
            planet.PlanetSetteings();
            planet.RandomizePlanet();
            planet.UpdateMaxNumOfShipsText();
            planet.UpdateNumOfShipsText();
            planet.UpdateDefineState();
            //Debug.Log("Planet " + planet.name + " created " + planet._size);
        }
    }

    public void InstatiateSpecificPlanets()
    {
        for (int i = 0; i < _numberOfEnemyPlanets; i++)
        {
            var planet = Instantiate<Planet>(_mapPlanet);
            planet.SetSettings(PlanetColor.Enemy, 80, 100, 1.5f);
            planet.UpdateMaxNumOfShipsText();
            var position = new Vector3(Random.Range(6f, 8f), Random.Range(-4f, 4f), -0.1f);
            planet.transform.position = position;
            // PlanetCollision(position);
            planet.gameObject.SetActive(true);
            _mapPlanets.Add(planet);
        }

        for (int i = 0; i < _numberOfFriendlyPlanets; i++)
        {
            var planet = Instantiate<Planet>(_mapPlanet);
            planet.SetSettings(PlanetColor.Friendly, 80, 100, 1.5f);
            planet.UpdateMaxNumOfShipsText();
            var position = new Vector3(Random.Range(-8f, -6f), Random.Range(-4f, 4f), -0.1f);
            planet.transform.position = position;
            // PlanetCollision(position);
            planet.gameObject.SetActive(true);
            _mapPlanets.Add(planet);
        }

        for (int i = 0; i < _numberOfNeutralPlanets; i++)
        {
            var planet = Instantiate<Planet>(_mapPlanet);
            planet.SetSettings(PlanetColor.Neutral, Random.Range(0.6f, 1.5f));
            planet.UpdateMaxNumOfShipsText();
            var position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
            planet.transform.position = position;
            // PlanetCollision(position);
            planet.gameObject.SetActive(true);
            _mapPlanets.Add(planet);
        }
    }

    public void PlanetCollision()
    {
        foreach (Planet planet in _mapPlanets)
        {
            for (int i = 0; i < _mapPlanets.Count; i++)
            {
                if (planet.isFriendly)
                {
                    var position = new Vector3(Random.Range(-8f, -6f), Random.Range(-4f, 4f), -0.1f);
                    if (Vector3.Distance(position, _mapPlanets[i].transform.position) < 1f)
                    {
                        position = new Vector3(Random.Range(-8f, -6f), Random.Range(-4f, 4f), -0.1f);
                        i = 0;
                    }
                    planet.transform.position = position;
                    planet.gameObject.SetActive(true);
                }

                if (planet.isEnemy)
                {
                    var position = new Vector3(Random.Range(6f, 8f), Random.Range(-4f, 4f), -0.1f);

                    if (Vector3.Distance(position, _mapPlanets[i].transform.position) < 1f)
                    {
                        position = new Vector3(Random.Range(6f, 8f), Random.Range(-4f, 4f), -0.1f);
                        i = 0;
                    }
                    planet.transform.position = position;
                    planet.gameObject.SetActive(true);
                }

                if (planet.isNeutral)
                {
                    var position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);

                    if (Vector3.Distance(position, _mapPlanets[i].transform.position) < 1f)
                    {
                        position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
                        i = 0;
                    }
                    planet.transform.position = position;
                    planet.gameObject.SetActive(true);
                }
            }
        }
    }

    public void PlanetCollision(Vector3 position)
    {
        foreach (Planet planet in _mapPlanets)
        {
            for (int i = 0; i < _mapPlanets.Count; i++)
            {
                if (Vector3.Distance(position, _mapPlanets[i].transform.position) < 1f)
                {
                    if (planet.isFriendly)
                    {
                        position = new Vector3(Random.Range(-8f, -6f), Random.Range(-4f, 4f), -0.1f);
                        planet.transform.position = position;
                        planet.gameObject.SetActive(true);
                    }

                    if (planet.isEnemy)
                    {
                        position = new Vector3(Random.Range(6f, 8f), Random.Range(-4f, 4f), -0.1f);
                        planet.transform.position = position;
                        planet.gameObject.SetActive(true);
                    }

                    if (planet.isNeutral)
                    {
                        position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
                        planet.transform.position = position;
                        planet.gameObject.SetActive(true);
                    }
                }
                else
                {
                    planet.transform.position = position;
                    planet.gameObject.SetActive(true);
                }
            }
        }
    }


    // public void InstatiateSpecificPlanets()
    // {
    //     for (int i = 0; i < _numberOfPlanets; i++)
    //     {
    //         var planet = Instantiate<Planet>(_mapPlanet);
    //         _mapPlanets.Add(planet);
    //         planet.gameObject.SetActive(false);
    //     }

    //     foreach (Planet planet in _mapPlanets)
    //     {
    //         var position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
    //         for (int i = 0; i < _mapPlanets.Count; i++)
    //         {
    //             if (Vector3.Distance(position, _mapPlanets[i].transform.position) < 1.5f)
    //             {
    //                 position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
    //                 i = 0;
    //             }
    //         }

    //         planet.transform.position = position;
    //         planet.gameObject.SetActive(true);
    //         for (int i = 0; i < 1; i++)
    //         {
    //             if (_mapPlanets.Contains(planet))
    //             {
    //                 if (!planet.isEnemy)
    //                 {
    //                     planet.SetSettings(PlanetColor.Enemy, 100, 1.5f);
    //                 }
    //             }
    //         }
    //     }
    // }

    // public void DrawLinesToTarget(Planet targetPlanet)
    // {
    //     foreach (GameObject planet in _selectedPlanets)
    //     {
    //             planet.GetComponent<Planet>().DrawLineToTarget(targetPlanet);

    //     }
    // }

    // public void LineRendererOff()
    // {
    //     foreach (GameObject planet in _selectedPlanets)
    //     {
    //         planet.GetComponent<Planet>().LineRendererOff(planet.GetComponent<Planet>());
    //     }
    // }
}
