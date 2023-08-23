using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TargetGlow : MonoBehaviour
{
    [SerializeField] GameObject _selectedGlow;
    [SerializeField] GameObject _maxShipTextObject;
    public bool _isClicked; // When a planet is clicked, it will glow until it is clicked again or another planet is clicked
    public bool _isSelected; // When a planet is hovered over, it will glow until the mouse exits the planet
    public bool _glowingEnabled = false;

    private void Awake()
    {
        _glowingEnabled = false;
    }

    private void OnMouseEnter()
    {
        _isSelected = !_isSelected;
        _maxShipTextObject.SetActive(true);
        if (GetComponent<Planet>().isFriendly)
        {
            if (!_isClicked)
            {
                if (_isSelected)
                {
                    _selectedGlow.SetActive(true);
                    foreach (Planet targetPlanet in PlanetManager.Instance._selectedPlanets)
                    {
                        if (targetPlanet != this && targetPlanet.GetComponent<TargetGlow>()._isSelected)
                        {
                            DrawLines._instance.DrawFewLines(targetPlanet.transform, this.transform);
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
                if (_isSelected)
                {
                    _selectedGlow.SetActive(true);
                    foreach (Planet targetPlanet in PlanetManager.Instance._selectedPlanets)
                    {
                        if (targetPlanet != this && targetPlanet.GetComponent<TargetGlow>()._isSelected)
                        {
                            DrawLines._instance.DrawFewLines(targetPlanet.transform, this.transform);
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

    private void OnMouseExit()
    {
        _maxShipTextObject.SetActive(false);
        if (!_isClicked)
        {
            _isSelected = false;
            _selectedGlow.SetActive(false);
            DrawLines._instance.ClearLines();
        }
    }

    public void SetGlowOff()
    {
        _selectedGlow.SetActive(false);
        _isClicked = false;
        _isSelected = false;
    }

    public void SetGlowOn()
    {
        _selectedGlow.SetActive(true);
        _isClicked = true;
        _isSelected = true;
    }
}
