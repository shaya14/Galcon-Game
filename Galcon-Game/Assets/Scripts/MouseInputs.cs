using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseInputs : Singleton<MouseInputs>
{
    public bool _isEnable = true;

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
                planet.GetComponent<TargetGlow>()._isClicked = true;
                PlanetManager.instance._selectedPlanets.Add(planet);
                DrawLines._instance.ClearLines();
                foreach (Planet enemy in PlanetManager.instance.neutralAndEnemyPlanets)
                {
                    enemy.GetComponent<TargetGlow>()._glowingEnabled = true;
                }
            }
            else if (rayHit.collider.gameObject.tag == "Background")
            {
                foreach (Planet selectedPlanet in PlanetManager.instance._selectedPlanets)
                {
                    selectedPlanet.GetComponent<TargetGlow>().SetGlowOff();
                    selectedPlanet._isAddedToSelectedPlanets = false;
                    foreach (Planet enemy in PlanetManager.instance.neutralAndEnemyPlanets)
                    {
                        enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
                    }
                }
                PlanetManager.instance._selectedPlanets.Clear();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var planet = rayHit.collider.GetComponent<Planet>();
            if (planet == null)
            {
                return;
            }
            if(PlanetManager.instance._selectedPlanets.Count <= 0)
            {
                return;
            }
            if (planet.isFriendly)
            {
                PlanetManager.instance._attackingShips.GetComponent<Ship>()._targetPlanet = planet;
                planet.GetComponent<CircleCollider2D>().isTrigger = true;
                planet._friendlyTargetArrows.SetActive(true);
                PlanetManager.instance.SpawnShips();
                DrawLines._instance.ClearLines();
                PlanetManager.instance._selectedPlanets.Clear();
                SoundFx.instance.PlaySound(SoundFx.instance._attackSound, 0.3f);
                planet._attackingNumber += PlanetManager.instance._numOfShipsGenerated;
                PlanetManager.instance._numOfShipsGenerated = 0;
            }
            else if (planet.isEnemy || planet.isNeutral)
            {
                PlanetManager.instance._attackingShips.GetComponent<Ship>()._targetPlanet = planet;
                planet.GetComponent<CircleCollider2D>().isTrigger = true;
                planet._friendlyTargetArrows.SetActive(true);
                PlanetManager.instance.SpawnShips();
                DrawLines._instance.ClearLines();
                PlanetManager.instance._selectedPlanets.Clear();
                SoundFx.instance.PlaySound(SoundFx.instance._attackSound, 0.3f);
                planet._attackingNumber += PlanetManager.instance._numOfShipsGenerated;
                PlanetManager.instance._numOfShipsGenerated = 0;
            }
        }
    }
}
