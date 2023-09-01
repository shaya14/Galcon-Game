using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxSelection : Singleton<BoxSelection>
{
    private LineRenderer _lineRenderer;
    private Vector2 _initalMousePosition, _currentMousePosition;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] private float _selectionTime = 0.2f;
    private float _timer = 0f;
    private List<Planet> newlySelectedPlanets = new List<Planet>(); // To store planets newly selected in this frame

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lineRenderer.positionCount = 4;
            _initalMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, new Vector2(_initalMousePosition.x, _initalMousePosition.y));
            _lineRenderer.SetPosition(1, new Vector2(_initalMousePosition.x, _initalMousePosition.y));
            _lineRenderer.SetPosition(2, new Vector2(_initalMousePosition.x, _initalMousePosition.y));
            _lineRenderer.SetPosition(3, new Vector2(_initalMousePosition.x, _initalMousePosition.y));
            _boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            _boxCollider2D.isTrigger = true;
            _boxCollider2D.offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        if (Input.GetMouseButton(0))
        {
            _timer += Time.deltaTime;
            _currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, new Vector2(_initalMousePosition.x, _initalMousePosition.y));
            _lineRenderer.SetPosition(1, new Vector2(_initalMousePosition.x, _currentMousePosition.y));
            _lineRenderer.SetPosition(2, new Vector2(_currentMousePosition.x, _currentMousePosition.y));
            _lineRenderer.SetPosition(3, new Vector2(_currentMousePosition.x, _initalMousePosition.y));

            transform.position = (_currentMousePosition + _initalMousePosition) / 2;

            _boxCollider2D.size = new Vector2(
                Mathf.Abs(_initalMousePosition.x - _currentMousePosition.x),
                Mathf.Abs(_initalMousePosition.y - _currentMousePosition.y));
            if (_timer > _selectionTime)
            {
                SelectObjects();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _timer = 0f;
            _lineRenderer.positionCount = 0;
            Destroy(_boxCollider2D);
            transform.position = Vector3.zero;
            DrawLines._instance.ClearLines();
        }
    }

    public void SelectObjects()
    {
        Collider2D[] objectsToSelect = Physics2D.OverlapBoxAll(transform.position, _boxCollider2D.size, 0);
        foreach (Collider2D selectable in objectsToSelect)
        {
            Planet planet = selectable.GetComponent<Planet>();
            if (planet == null)
            {
                continue;
            }
            if (planet.isFriendly && !planet.isSelected)
            {
                planet.GetComponent<TargetGlow>().SetGlowOn();
                if (!newlySelectedPlanets.Contains(planet))
                {
                    newlySelectedPlanets.Add(planet);
                }

                foreach (Planet enemy in PlanetManager.Instance.neutralAndEnemyPlanets)
                {
                    enemy.GetComponent<TargetGlow>()._glowingEnabled = true;
                }
            }
        }

        // Deselect planets that were selected previously but are not in the newlySelectedPlanets list
        foreach (Planet selectedPlanet in PlanetManager.Instance._selectedPlanets)
        {
            if (!newlySelectedPlanets.Contains(selectedPlanet))
            {
                selectedPlanet.GetComponent<TargetGlow>().SetGlowOff();
            }
        }

        PlanetManager.Instance._selectedPlanets = newlySelectedPlanets; // Update the selected planets list
    }
}

