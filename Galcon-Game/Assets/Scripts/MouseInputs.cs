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
                GameManager.Instance._planets.Add(friendly);

                foreach (GameObject enemy in GameManager.Instance._enemies)
                {
                    enemy.GetComponent<TargetGlow>()._glowingEnabled = true;
                }
            }
            else if (rayHit.collider.gameObject.tag == "Background")
            {
                foreach (GameObject planet in GameManager.Instance._planets)
                {
                    planet.GetComponent<Planet>()._isSelected = false;
                    planet.GetComponent<TargetGlow>().SetGlowOff();

                    foreach (GameObject enemy in GameManager.Instance._enemies)
                    {
                        enemy.GetComponent<TargetGlow>()._glowingEnabled = false;
                    }
                }
                GameManager.Instance._planets.Clear();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (rayHit.collider.gameObject.tag == "Friendly")
            {
                GameObject freindly = rayHit.collider.gameObject;
                GameManager.Instance._attackingShips.GetComponent<Ship>()._targetPlanet = freindly;
                GameManager.Instance.SpawnShips();
                DrawLines._instance.ClearLines();
            }
            else if (rayHit.collider.gameObject.tag == "Enemy" || rayHit.collider.gameObject.tag == "Neutral")
            {
                GameObject enemy = rayHit.collider.gameObject;
                GameManager.Instance._attackingShips.GetComponent<Ship>()._targetPlanet = enemy;
                GameManager.Instance.SpawnShips();
                DrawLines._instance.ClearLines();
            }
        }
    }
}
