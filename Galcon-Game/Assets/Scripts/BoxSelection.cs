using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxSelection : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Vector2 _initalMousePosition, _currentMousePosition;
    private BoxCollider2D _boxCollider2D;

    public static BoxSelection _instance;

    void Start()
    {
        _instance = this;
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
            _currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, new Vector2(_initalMousePosition.x, _initalMousePosition.y));
            _lineRenderer.SetPosition(1, new Vector2(_initalMousePosition.x, _currentMousePosition.y));
            _lineRenderer.SetPosition(2, new Vector2(_currentMousePosition.x, _currentMousePosition.y));
            _lineRenderer.SetPosition(3, new Vector2(_currentMousePosition.x, _initalMousePosition.y));

            transform.position = (_currentMousePosition + _initalMousePosition) / 2;

            _boxCollider2D.size = new Vector2(
                Mathf.Abs(_initalMousePosition.x - _currentMousePosition.x),
                Mathf.Abs(_initalMousePosition.y - _currentMousePosition.y));
            SelectObjects();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _lineRenderer.positionCount = 0;
            Destroy(_boxCollider2D);
            transform.position = Vector3.zero;
        }
    }

    public void SelectObjects()
    {
        Collider2D[] objectsToSelect = Physics2D.OverlapBoxAll(transform.position, _boxCollider2D.size, 0);
        foreach (Collider2D selectable in objectsToSelect)
        {
            Planet planet = selectable.GetComponent<Planet>();
            if (planet == null) {
              continue;
            }
            if (planet.isFriendly && !planet.isSelected)
            {
                planet.GetComponent<TargetGlow>().SetGlowOn();
                PlanetManager.Instance._selectedPlanets.Add(planet);
                
                foreach (Planet enemy in PlanetManager.Instance._enemiesToSelect)
                {
                    enemy.GetComponent<TargetGlow>()._glowingEnabled = true;
                }
            }
        }
    }
}

