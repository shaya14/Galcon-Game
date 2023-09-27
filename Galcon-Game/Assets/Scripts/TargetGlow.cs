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
            _maxShipTextObject.SetActive(true);
            if (GetComponent<Planet>().isFriendly)
            {
                if (!_isClicked)
                {
                    _selectedGlow.SetActive(true);
                    foreach (Planet targetPlanet in PlanetManager.Instance.selectedPlanets)
                    {
                        if (targetPlanet != this )
                        {
                            DrawLines.Instance.DrawLine(targetPlanet.transform, this.transform);
                        }
                    }
                }
            }
            else if (GetComponent<Planet>().isEnemy || GetComponent<Planet>().isNeutral)
            {
                if (_glowingEnabled)
                {
                    _selectedGlow.SetActive(true);
                    foreach (Planet targetPlanet in PlanetManager.Instance.selectedPlanets)
                    {
                        if (targetPlanet != this )
                        {
                            DrawLines.Instance.DrawLine(targetPlanet.transform, this.transform);
                        }
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
            _selectedGlow.SetActive(false);
            DrawLines.Instance.ClearLines();
        }
    }

    public void SetGlowOff()
    {
        _selectedGlow.SetActive(false);
        _isClicked = false;
    }

    public void SetGlowOn()
    {
        _selectedGlow.SetActive(true);
        _isClicked = true;
    }
}
