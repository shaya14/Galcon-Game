using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TargetGlow : MonoBehaviour
{
    [SerializeField] private GameObject _selectedGlow;
    [SerializeField] private GameObject _maxShipTextObject;

    private bool _isHovered;

    private Planet _thisPlanet;

    private void Awake()
    {
        _thisPlanet = GetComponent<Planet>();
        _selectedGlow.SetActive(false);
    }

    private void Update()
    {
        if (!MouseInputs.Instance._isEnable)
        {
            return;
        }
        if (_isHovered)
        {
            _maxShipTextObject.SetActive(true);
        }
        else
        {
            _maxShipTextObject.SetActive(false);
        }

        if (_thisPlanet.isSelected)
        {
            _selectedGlow.SetActive(true);
        }
        else if (_isHovered && PlanetManager.Instance.selectedPlanets.Count > 0)
        {
            _selectedGlow.SetActive(true);

            foreach (Planet targetPlanet in PlanetManager.Instance.selectedPlanets)
            {
                if (targetPlanet != this)
                {
                    DrawLines.Instance.DrawLine(targetPlanet.transform, this.transform);
                }
            }
        }
        else if (_isHovered && _thisPlanet.isFriendly)
        {
            _selectedGlow.SetActive(true);
        }
        else
        {
            _selectedGlow.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        _isHovered = true;
    }

    private void OnMouseExit()
    {
        _isHovered = false;
        DrawLines.Instance.ClearLines();
    }
}
