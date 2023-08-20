using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [Header("Map Prefabs")]
    [SerializeField] private GameObject _mapPlanet;
    public GameObject _attackingShips;

    [Header("Map Settings")]
    [SerializeField] private int _numberOfPlanets;

    [Header("Map & Settings Planets")]
    [SerializeField] private List<Planet> _mapPlanets;
    public List<GameObject> _selectedPlanets;
    public List<GameObject> _enemiesToSelect;

    [Header("Game Planets")]
    public List<GameObject> _friendlyPlanets;
    public List<GameObject> _enemyPlanets;
    public List<GameObject> _neutralPlanets;

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
        InstatiatePlanets();
    }
    void Start()
    {
        _selectedPlanets = new List<GameObject>();
        _enemiesToSelect = new List<GameObject>();
        _friendlyPlanets = new List<GameObject>();
        _enemyPlanets = new List<GameObject>();
        _enemiesToSelect.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        _enemiesToSelect.AddRange(GameObject.FindGameObjectsWithTag("Neutral"));
        _friendlyPlanets.AddRange(GameObject.FindGameObjectsWithTag("Friendly"));
        _enemyPlanets.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        _neutralPlanets.AddRange(GameObject.FindGameObjectsWithTag("Neutral"));
    }

    void Update()
    {

    }

    public void SpawnShips()
    {
        foreach (GameObject planet in _selectedPlanets)
        {
            planet.GetComponent<Planet>()._numberOfShips /= 2;
            planet.GetComponent<Planet>().UpdateNumOfShipsText();
            for (int i = 0; i < planet.GetComponent<Planet>()._numberOfShips; i++)
            {
                GameObject ship = Instantiate(_attackingShips, planet.transform.position, Quaternion.identity);
            }
        }

        foreach (GameObject planet in _selectedPlanets)
        {
            planet.GetComponent<Planet>()._isSelected = false;
            planet.GetComponent<TargetGlow>().SetGlowOff();
        }
        _selectedPlanets.Clear();

        foreach (GameObject enemy in _enemiesToSelect)
        {
            enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
        }
    }

    public void InstatiatePlanets()
    {
        for (int i = 0; i < _numberOfPlanets; i++)
        {
            GameObject planet = Instantiate(_mapPlanet);
            _mapPlanets.Add(planet.GetComponent<Planet>());
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
            planet.UpdateState();
            planet.UpdateDefineState();
            //Debug.Log("Planet " + planet.name + " created " + planet._size);
        }

        // for (int i = 0; i < _numberOfPlanets; i++)
        // {
        //     var position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), -0.1f);
        //     GameObject newPlanet = Instantiate(_mapPlanet, position, Quaternion.identity);
        //     Debug.Log("Planet " + newPlanet.name + " created " + newPlanet.GetComponent<Planet>()._size);
        //     newPlanet.GetComponent<Planet>().PlanetSize();
        //     newPlanet.GetComponent<Planet>().PlanetSetteings();
        //     newPlanet.GetComponent<Planet>().RandomizePlanet();
        //     newPlanet.GetComponent<Planet>().UpdateState();
        //     newPlanet.GetComponent<Planet>().UpdateDefineState();
        // }
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
