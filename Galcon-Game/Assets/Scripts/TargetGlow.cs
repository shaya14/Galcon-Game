using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

// CR: [discuss how to simplify the code]
public class TargetGlow : MonoBehaviour
{
    [SerializeField] GameObject _selectedGlow;
    [SerializeField] GameObject _maxShipTextObject;
    public bool _isClicked; // When a planet is clicked, it will glow until it is clicked again or another planet is clicked
    public bool _isHoveredOrSelected; // When a planet is hovered over, it will glow until the mouse exits the planet
    public bool _glowingEnabled = false;

    public bool _isEnable = true;

    private void Awake()
    {
        _glowingEnabled = false;
    }

    private void OnMouseEnter()
    {
        if (_isEnable)
        {
            _isHoveredOrSelected = !_isHoveredOrSelected;
            _maxShipTextObject.SetActive(true);
            if (GetComponent<Planet>().isFriendly)
            {
                if (!_isClicked)
                {
                    if (_isHoveredOrSelected)
                    {
                        _selectedGlow.SetActive(true);
                        foreach (Planet targetPlanet in PlanetManager.Instance._selectedPlanets)
                        {
                            if (targetPlanet != this && targetPlanet.GetComponent<TargetGlow>()._isHoveredOrSelected)
                            {
                                DrawLines._instance.DrawLine(targetPlanet.transform, this.transform);
                            }
                        }
                    }
                    else
                    {
                        _selectedGlow.SetActive(false);
                    }
                }
            }
            else if (GetComponent<Planet>().isEnemy || GetComponent<Planet>().isNeutral)
            {
                if (_glowingEnabled)
                {
                    if (_isHoveredOrSelected)
                    {
                        _selectedGlow.SetActive(true);
                        foreach (Planet targetPlanet in PlanetManager.Instance._selectedPlanets)
                        {
                            if (targetPlanet != this && targetPlanet.GetComponent<TargetGlow>()._isHoveredOrSelected)
                            {
                                DrawLines._instance.DrawLine(targetPlanet.transform, this.transform);
                            }
                        }
                    }
                    else
                    {
                        _selectedGlow.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnMouseExit()
    {
        _maxShipTextObject.SetActive(false);
        if (!_isClicked)
        {
            _isHoveredOrSelected = false;
            _selectedGlow.SetActive(false);
            DrawLines._instance.ClearLines();
        }
    }

    public void SetGlowOff()
    {
        _selectedGlow.SetActive(false);
        _isClicked = false;
        _isHoveredOrSelected = false;
    }

    public void SetGlowOn()
    {
        _selectedGlow.SetActive(true);
        _isClicked = true;
        _isHoveredOrSelected = true;
    }
}
