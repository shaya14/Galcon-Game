using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseInputs : MonoBehaviour
{
    void Update()
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (Input.GetMouseButtonDown(0))
        {
            if (rayHit.collider.gameObject.tag == "Friendly" && !rayHit.collider.gameObject.GetComponent<Planet>()._isSelected)
            {
                GameObject friendly = rayHit.collider.gameObject;
                friendly.GetComponent<Planet>()._isSelected = true;
                friendly.GetComponent<TargetGlow>()._isClicked = true;
                PlanetManager.Instance._selectedPlanets.Add(friendly);
                DrawLines._instance.ClearLines();

                foreach (GameObject enemy in PlanetManager.Instance._enemiesToSelect)
                {
                    enemy.GetComponent<TargetGlow>()._glowingEnabled = true;
                }
            }
            else if (rayHit.collider.gameObject.tag == "Background")
            {
                foreach (GameObject planet in PlanetManager.Instance._selectedPlanets)
                {
                    planet.GetComponent<Planet>()._isSelected = false;
                    planet.GetComponent<TargetGlow>().SetGlowOff();

                    foreach (GameObject enemy in PlanetManager.Instance._enemiesToSelect)
                    {
                        enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
                    }
                }
                PlanetManager.Instance._selectedPlanets.Clear();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (rayHit.collider.gameObject.tag == "Friendly")
            {
                GameObject freindly = rayHit.collider.gameObject;
                PlanetManager.Instance._attackingShips.GetComponent<Ship>()._targetPlanet = freindly;
                PlanetManager.Instance.SpawnShips();
                DrawLines._instance.ClearLines();
            }
            else if (rayHit.collider.gameObject.tag == "Enemy" || rayHit.collider.gameObject.tag == "Neutral")
            {
                GameObject enemy = rayHit.collider.gameObject;
                PlanetManager.Instance._attackingShips.GetComponent<Ship>()._targetPlanet = enemy;
                PlanetManager.Instance.SpawnShips();
                DrawLines._instance.ClearLines();
            }
        }
    }
}
