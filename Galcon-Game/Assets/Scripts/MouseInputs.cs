using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseInputs : MonoBehaviour
{
    [SerializeField] private GameObject _attackingShips;

    private List<GameObject> _planets;
    private List<GameObject> _enemies;

    void Start()
    {
        _planets = new List<GameObject>();
        _enemies = new List<GameObject>();
        _enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        _enemies.AddRange(GameObject.FindGameObjectsWithTag("Neutral"));
    }

    void Update()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (Input.GetMouseButtonDown(0))
        {
            if (rayHit.collider.gameObject.tag == "Player" && !rayHit.collider.gameObject.GetComponent<Planet>()._isSelected)
            {
                GameObject player = rayHit.collider.gameObject;
                player.GetComponent<SpriteRenderer>().color = player.GetComponent<Planet>()._selectedColor;
                player.GetComponent<Planet>()._isSelected = true;
                player.GetComponent<TargetGlow>()._isClicked = true;
                _planets.Add(player);

                foreach (GameObject enemy in _enemies)
                {
                    enemy.GetComponent<TargetGlow>()._glowingEnabled = true;
                }
            }
            else if (rayHit.collider.gameObject.tag == "Background")
            {
                foreach (GameObject planet in _planets)
                {
                    planet.GetComponent<SpriteRenderer>().color = planet.GetComponent<Planet>()._defaultColor;
                    planet.GetComponent<Planet>()._isSelected = false;
                    planet.GetComponent<TargetGlow>().SetGlowOff();

                    foreach (GameObject enemy in _enemies)
                    {
                        enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
                    }
                }
                _planets.Clear();
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(rayHit.collider.gameObject.tag == "Player" && !rayHit.collider.gameObject.GetComponent<Planet>()._isSelected)
            {
                GameObject freindly = rayHit.collider.gameObject;
                _attackingShips.GetComponent<Ship>()._targetPlanet = freindly;
                SpawnShips();               
            }
            else if (rayHit.collider.gameObject.tag == "Enemy" || rayHit.collider.gameObject.tag == "Neutral")
            {
                GameObject enemy = rayHit.collider.gameObject;
                _attackingShips.GetComponent<Ship>()._targetPlanet = enemy;
                Debug.Log(enemy);
                SpawnShips();
            }
        }
    }

    private void SpawnShips()
    {
        foreach (GameObject planet in _planets)
        {
            planet.GetComponent<Planet>()._numberOfShips /= 2;
            planet.GetComponent<Planet>().UpdateNumOfShipsText();
            for (int i = 0; i < planet.GetComponent<Planet>()._numberOfShips; i++)
            {
                GameObject ship = Instantiate(_attackingShips, planet.transform.position, Quaternion.identity);
            }
        }

        foreach (GameObject planet in _planets)
        {
            planet.GetComponent<SpriteRenderer>().color = planet.GetComponent<Planet>()._defaultColor;
            planet.GetComponent<Planet>()._isSelected = false;
            planet.GetComponent<TargetGlow>().SetGlowOff();
        }
        _planets.Clear();

        foreach (GameObject enemy in _enemies)
        {
            enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
        }
    }
}
