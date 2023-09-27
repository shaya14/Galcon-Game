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
                PlanetManager.Instance.selectedPlanets.Add(planet);
                DrawLines.Instance.ClearLines();
            }
            else if (rayHit.collider.gameObject.tag == "Background")
            {
                PlanetManager.Instance.selectedPlanets.Clear();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var planet = rayHit.collider.GetComponent<Planet>();
            if (planet == null)
            {
                return;
            }
            if(PlanetManager.Instance.selectedPlanets.Count <= 0)
            {
                return;
            }
            if (planet.isFriendly)
            {
                planet.GetComponent<CircleCollider2D>().isTrigger = true;
                planet._friendlyTargetArrows.SetActive(true);
                PlanetManager.Instance.SpawnShips(planet);
                DrawLines.Instance.ClearLines();
                PlanetManager.Instance.selectedPlanets.Clear();
                SoundFx.Instance.PlaySound(SoundFx.Instance.attackSound, 0.3f);
                planet._attackingNumber += PlanetManager.Instance._numOfShipsGenerated;
                PlanetManager.Instance._numOfShipsGenerated = 0;
            }
            else if (planet.isEnemy || planet.isNeutral)
            {
                planet.GetComponent<CircleCollider2D>().isTrigger = true;
                planet._friendlyTargetArrows.SetActive(true);
                PlanetManager.Instance.SpawnShips(planet);
                DrawLines.Instance.ClearLines();
                PlanetManager.Instance.selectedPlanets.Clear();
                SoundFx.Instance.PlaySound(SoundFx.Instance.attackSound, 0.3f);
                planet._attackingNumber += PlanetManager.Instance._numOfShipsGenerated;
                PlanetManager.Instance._numOfShipsGenerated = 0;
            }
        }
    }
}
