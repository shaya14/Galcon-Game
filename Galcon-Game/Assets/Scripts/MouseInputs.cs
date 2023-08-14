using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseInputs : MonoBehaviour
{
    private List<GameObject> _planets;
    void Start()
    {
        _planets = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (rayHit.collider.gameObject.tag == "Planet" && !rayHit.collider.gameObject.GetComponent<Planet>()._isSelected)
            {
                GameObject planet = rayHit.collider.gameObject;               
                planet.GetComponent<SpriteRenderer>().color = planet.GetComponent<Planet>()._selectedColor;
                planet.GetComponent<Planet>()._isSelected = true;
                planet.GetComponent<TargetGlow>()._isClicked = true;  
                _planets.Add(planet);                
            }
            else if (rayHit.collider.gameObject.tag == "Background")
            {
                foreach (GameObject planet in _planets)
                {
                    planet.GetComponent<SpriteRenderer>().color = planet.GetComponent<Planet>()._defaultColor;
                    planet.GetComponent<Planet>()._isSelected = false;
                    planet.GetComponent<TargetGlow>().SetGlowOff();
                }
                _planets.Clear();
            }
        }
    }
}
