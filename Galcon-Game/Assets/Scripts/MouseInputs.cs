using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseInputs : MonoBehaviour
{
    public bool _isEnable = true;
    private static MouseInputs _instance;
    public static MouseInputs Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }
    void Update()
    {
        if (!_isEnable)
        {
            return;
        }
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (rayHit.collider == null)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            var planet = rayHit.collider.GetComponent<Planet>();
            if (planet != null && planet.isFriendly && !planet.isSelected)
            {
                planet.isSelected = true;
                planet.GetComponent<TargetGlow>()._isClicked = true;
                PlanetManager.Instance._selectedPlanets.Add(planet);
                DrawLines._instance.ClearLines();
                foreach (Planet enemy in PlanetManager.Instance._enemiesToSelect)
                {
                    enemy.GetComponent<TargetGlow>()._glowingEnabled = true;
                }
            }
            else if (rayHit.collider.gameObject.tag == "Background")
            {
                foreach (Planet selectedPlanet in PlanetManager.Instance._selectedPlanets)
                {
                    selectedPlanet.isSelected = false;
                    selectedPlanet.GetComponent<TargetGlow>().SetGlowOff();
                    selectedPlanet._isAdded = false;
                    foreach (Planet enemy in PlanetManager.Instance._enemiesToSelect)
                    {
                        enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
                    }
                }
                PlanetManager.Instance._selectedPlanets.Clear();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var planet = rayHit.collider.GetComponent<Planet>();
            if (planet == null)
            {
                return;
            }

            if (planet.isFriendly)
            {
                PlanetManager.Instance._attackingShips.GetComponent<Ship>()._targetPlanet = planet;
                planet.GetComponent<CircleCollider2D>().isTrigger = true;
                planet._friendlyTargetArrows.SetActive(true);
                PlanetManager.Instance.SpawnShips();
                DrawLines._instance.ClearLines();
                PlanetManager.Instance._selectedPlanets.Clear();
                SoundFx.Instance.PlaySound(SoundFx.Instance._attackSound, 0.3f);
                planet._attackingNumber += PlanetManager.Instance._numOfShipsGenerated;
                PlanetManager.Instance._numOfShipsGenerated = 0;
            }
            else if (planet.isEnemy || planet.isNeutral)
            {
                PlanetManager.Instance._attackingShips.GetComponent<Ship>()._targetPlanet = planet;
                planet.GetComponent<CircleCollider2D>().isTrigger = true;
                planet._friendlyTargetArrows.SetActive(true);
                PlanetManager.Instance.SpawnShips();
                DrawLines._instance.ClearLines();
                PlanetManager.Instance._selectedPlanets.Clear();
                SoundFx.Instance.PlaySound(SoundFx.Instance._attackSound, 0.3f);
                planet._attackingNumber += PlanetManager.Instance._numOfShipsGenerated;
                PlanetManager.Instance._numOfShipsGenerated = 0;
            }
        }
    }
}
