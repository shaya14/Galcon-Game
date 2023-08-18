using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetGlow : MonoBehaviour
{
    [SerializeField] GameObject _selectedGlow;
    [SerializeField] GameObject _glowLines;
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
        if (this.gameObject.tag == "Friendly")
        {
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
        else if (this.gameObject.tag == "Enemy" || this.gameObject.tag == "Neutral")
        {
            GameObject enemy = this.gameObject;
            if (_glowingEnabled)
            {
                if (_isSelected)
                {
                    _selectedGlow.SetActive(true);
                    // Add a glow lines
                    for (int i = 0; i < GameManager.Instance._planets.Count; i++)
                    {
                        Debug.DrawLine(this.transform.position, GameManager.Instance._planets[i].transform.position, Color.red, 1f);
                        GameObject glowLines = Instantiate(_glowLines, this.transform.position, Quaternion.identity);
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
