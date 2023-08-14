using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGlow : MonoBehaviour
{
    [SerializeField] GameObject _selectedGlow;
    public bool _isClicked; // When a planet is clicked, it will glow until it is clicked again or another planet is clicked
    public bool _isSelected; // When a planet is hovered over, it will glow until the mouse exits the planet

    private void OnMouseEnter()
    {
        _isSelected = !_isSelected;
        if (!_isClicked)
        {
            if (_isSelected)
            {
                _selectedGlow.SetActive(true);
            }
            else
            {
                _selectedGlow.SetActive(false);
            }
        }
    }

    private void OnMouseExit()
    {
        if (!_isClicked)
        {
            _isSelected = false;
            _selectedGlow.SetActive(false);
        }
    }

    public void SetGlowOff()
    {
        _selectedGlow.SetActive(false);
        _isClicked = false;
        _isSelected = false;    
    }
}
