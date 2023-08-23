using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseInputs : MonoBehaviour
{
    void Update()
    {
        //DrawLines._instance.ClearLines();
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (Input.GetMouseButtonDown(0))
        {
            var planet = rayHit.collider.GetComponent<Planet>();
            if (planet != null && planet.isFriendly && !planet._isSelected)
            {
                planet._isSelected = true;
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
                    selectedPlanet.GetComponent<Planet>()._isSelected = false;
                    selectedPlanet.GetComponent<TargetGlow>().SetGlowOff();

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
            if (planet == null) {
              return;
            }
            if (planet.isFriendly)
            {
                PlanetManager.Instance._attackingShips.GetComponent<Ship>()._targetPlanet = planet;
                PlanetManager.Instance.SpawnShips();
                DrawLines._instance.ClearLines();
            }
            else if (planet.isEnemy || planet.isNeutral)
            {
                PlanetManager.Instance._attackingShips.GetComponent<Ship>()._targetPlanet = planet;
                PlanetManager.Instance.SpawnShips();
                DrawLines._instance.ClearLines();
            }
        }
    }
}
