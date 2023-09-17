using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CalculateCameraBox : MonoBehaviour
{
    private Camera _camera;
    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private float _sizeX, _sizeY, ratio;
    void Start()
    {
        _camera = Camera.main;
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _sizeY = _camera.orthographicSize * 2;
        ratio = (float)Screen.width / (float)Screen.height;
        _sizeX = _sizeY * ratio;
        _boxCollider2D.size = new Vector2(_sizeX, _sizeY);
        _spriteRenderer.size = new Vector2(_sizeX, _sizeY);
    }
}
