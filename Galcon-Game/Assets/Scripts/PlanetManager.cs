using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] LayerMask layerMask;
    private static PlanetManager _instance;
    public static PlanetManager Instance { get; set; }

    //public List<Ship> _attackingShipsList;

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
            InstantiateSpecPlanets();
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
        //_attackingShipsList = new List<Ship>();

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
                //_attackingShipsList.Add(ship);
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
        }
    }

    private void InstatiateFor(float numOfPlanets, Vector3 position, PlanetColor planetColor, int numOfShips, int maxShips, float size)
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

    private void InstatiateFor(float numOfPlanets, Vector3 position, PlanetColor planetColor)
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

    // CR: note also, that 'distance < 1' is not the correct check (because 'distance' is the distance between the CENTERS of the planets).
    //     larger planets need to be further away from each other, so you probably need to add 'localScale' or 'size' or similar to your check

    // CR: Your bug is that when choosing a position for a planet, you are changing the position randomly for every planet you 
    //     chcking against! So, position = new Vector3(Random...) should be outside of the 'for (int i = 0;, i < _mapPlanets.Count; ...)'.
    //     See my implementation attached:

    // Checks collisions between _mapPlanets[planetIndex], and all the planets _mapPlanets[0,...,planetIndex-1].
    // So, HasCollisions(0) will not check anything, HasCollisions(1) will check for collisions between 0 and 1.
    // HasCollisions(2)  will check (0,2) and (1,2), ...
    
    private bool HasCollisions(int planetIndex) {
      for (int i = 0; i < planetIndex; i++) {
        if (Vector3.Distance(_mapPlanets[planetIndex].transform.position, _mapPlanets[i].transform.position) < _distanceBetweenPlanets) {
          return true;
        }
      }

      return false;
    }

    public void PlanetCollision() {
      for (int i = 0; i < _mapPlanets.Count; i++) {
        while (HasCollisions(i)) {

          if (_mapPlanets[i].isFriendly) {
            _mapPlanets[i].transform.position = new Vector3(Random.Range(-8f, -6f), Random.Range(-4f, 4f), -0.1f);
          } else if (_mapPlanets[i].isEnemy) {
            _mapPlanets[i].transform.position = new Vector3(Random.Range(6f, 8f), Random.Range(-4f, 4f), -0.1f);
          } else if (_mapPlanets[i].isNeutral) {
            _mapPlanets[i].transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
          } else {
          }
        }
        _mapPlanets[i].gameObject.SetActive(true);
      }
    }
    

    private bool CheckOverlap(Vector2 position, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius + 1f, layerMask);
        return colliders.Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Planet planet in _mapPlanets)
        {
            var size = planet.size;
            var radius = size * 2 / (2 * Mathf.PI);
            Gizmos.DrawWireSphere(planet.transform.position, radius);
        }
    }

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
