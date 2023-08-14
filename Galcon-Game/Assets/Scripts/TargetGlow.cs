using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGlow : MonoBehaviour
{
    [SerializeField] GameObject _selectedGlow;
    public bool _isSelected;

    private void OnMouseEnter()
    {
        _isSelected = !_isSelected;

        if (_isSelected)
        {
            _selectedGlow.SetActive(true);
        }
        else
        {
            _selectedGlow.SetActive(false);
        }
    }

    private void OnMouseExit()
    {
        _isSelected = false;
        _selectedGlow.SetActive(false);
    }
}
