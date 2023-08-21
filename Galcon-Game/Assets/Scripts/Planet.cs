using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum PlanetColor
{
   Enemy,
   Friendly,
   Neutral
}

public class Planet : MonoBehaviour
{

   [Header("Planet Settings")]
   public PlanetColor _planetColor;
   [SerializeField] private float _shipPerSecond;
   [SerializeField] public int _iniaitalShips;
   [SerializeField] public int _maxShips;
   [HideInInspector] public int _numberOfShips;


   [Header("Player Colors")]
   [SerializeField] private Color _playerColor;
   [SerializeField] private Color _enemyColor;
   [SerializeField] private Color _neutralColor;
   [HideInInspector] public Color _updateColor;

   [Header("UI Elements")]
   [SerializeField] private TextMeshPro _shipCounterText;
   [SerializeField] private TextMeshPro _maxShipCounterText;

   public bool _isFriendly;
   public bool _isEnemy;
   public bool _isNeutral;
   public bool _isSelected;
   private SpriteRenderer _spriteRenderer;
   private LineRenderer _lineRenderer;
   private float _timer;
   public float _size = 1;

   void Awake()
   {
      _shipCounterText.text = _numberOfShips.ToString();
      _maxShipCounterText.text = _maxShips.ToString();
      _neutralColor = GetComponent<SpriteRenderer>().color;
      _spriteRenderer = GetComponent<SpriteRenderer>();
   }
   private void Start()
   {
      _isSelected = false;
      _spriteRenderer.color = _updateColor;
      _numberOfShips = _iniaitalShips;
      _shipPerSecond /= _size;
      UpdateNumOfShipsText();
      _lineRenderer = GetComponent<LineRenderer>();
   }

   private void Update()
   {
      NewShipTimer();
      UpdateState();
      UpdateDefineState();
      _spriteRenderer.color = _updateColor;

      if (_numberOfShips >= _maxShips)
      {
         _numberOfShips = _maxShips;
      }
   }

   public void UpdateDefineState()
   {
      switch (_planetColor)
      {
         case PlanetColor.Enemy:
            gameObject.name = "Enemy Planet";
            gameObject.tag = "Enemy";
            GetComponent<EnemyAI>().enabled = true;
            _isEnemy = true;
            _updateColor = _enemyColor;
            break;
         case PlanetColor.Friendly:
            gameObject.name = "Friendly Planet";
            gameObject.tag = "Friendly";
            GetComponent<EnemyAI>().enabled = false;
            _isFriendly = true;
            _updateColor = _playerColor;
            break;
         case PlanetColor.Neutral:
            gameObject.name = "Neutral Planet";
            gameObject.tag = "Neutral";
            _shipCounterText.text = _iniaitalShips.ToString();
            GetComponent<EnemyAI>().enabled = false;
            _isNeutral = true;
            _updateColor = _neutralColor;
            break;
         default:
            gameObject.name = "Neutral Planet";
            gameObject.tag = "Neutral";
            GetComponent<EnemyAI>().enabled = false;
            _isNeutral = true;
            _updateColor = _neutralColor;
            break;
      }
   }

   public void UpdateState()
   {
      if (_isFriendly)
      {
         _planetColor = PlanetColor.Friendly;
      }
      else if (_isEnemy)
      {
         _planetColor = PlanetColor.Enemy;
      }
      else if (_isNeutral)
      {
         _planetColor = PlanetColor.Neutral;
      }
   }

   void NewShipTimer()
   {
      if (_timer > _shipPerSecond && _numberOfShips < _maxShips)
      {
         _timer = 0;
         _numberOfShips++;
         _shipCounterText.text = _numberOfShips.ToString();
      }
      else
      {
         _timer += Time.deltaTime;
      }
   }

   public void UpdateNumOfShipsText()
   {
      _shipCounterText.text = _numberOfShips.ToString();
   }

   public void UpdateMaxNumOfShipsText()
   {
      _maxShipCounterText.text = _maxShips.ToString();
   }

   public float PlanetSize()
   {
      _size = Random.Range(0.6f, 1.6f);
      transform.localScale = new Vector3(_size, _size, _size);
      return _size;
   }

   public void RandomizePlanet()
   {
      int randomColor = Random.Range(0, 3);
      switch (randomColor)
      {
         case 0:
            _isFriendly = true;
            _isEnemy = false;
            _isNeutral = false;
            break;
         case 1:
            _isFriendly = false;
            _isEnemy = true;
            _isNeutral = false;
            break;
         case 2:
            _isFriendly = false;
            _isEnemy = false;
            _isNeutral = true;
            break;
         default:
            _isFriendly = false;
            _isEnemy = false;
            _isNeutral = true;
            break;
      }
   }

   public void PlanetSetteings()
   {
      if (PlanetSize() <= 0.8f)
      {
         _numberOfShips = Random.Range(8, 10);
         _maxShips = Random.Range(12, 25);
         _iniaitalShips = Random.Range(2, _numberOfShips);
      }
      else if (PlanetSize() > 0.8f && PlanetSize() <= 1.2f)
      {
         _numberOfShips = Random.Range(25, 35);
         _maxShips = Random.Range(37, 60);
         _iniaitalShips = Random.Range(10, _numberOfShips);
      }
      else if (PlanetSize() > 1.2f)
      {
         _numberOfShips = Random.Range(37, 50);
         _maxShips = Random.Range(60, 100);
         _iniaitalShips = Random.Range(35, _numberOfShips);
      }
   }

   // private void OnMouseExit()
   // {
   //    PlanetManager.Instance.LineRendererOff();
   // }

   // private void OnMouseEnter()
   // {
   //    if (PlanetManager.Instance._selectedPlanets.Count > 0)
   //    {
   //       DrawLineToTarget(this);
   //    }
   // }

   // public void DrawLineToTarget(Planet target)
   // {
   //    _lineRenderer.enabled = true;
   //    _lineRenderer.SetPosition(0, this.transform.position);
   //    _lineRenderer.SetPosition(1, target.transform.position);
   // }

   // public void LineRendererOff(Planet target)
   // {
   //    _lineRenderer.enabled = false;
   //    _lineRenderer.SetPosition(0, Vector3.zero);
   //    _lineRenderer.SetPosition(1, Vector3.zero);
   // }
}
