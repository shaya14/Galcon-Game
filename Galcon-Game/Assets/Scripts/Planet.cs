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
   [SerializeField] private int _maxShips;
   [HideInInspector] public int _numberOfShips;


   [Header("Player Colors")]
   [SerializeField] private Color _playerColor;
   [SerializeField] private Color _enemyColor;
   [SerializeField] private Color _neutralColor;
   [HideInInspector] public Color _updateColor;

   [Header("UI Elements")]
   [SerializeField] private TextMeshPro _shipCounterText;

   public bool _isFreindly;
   public bool _isEnemy;
   public bool _isNeutral;
   public bool _isSelected;
   private SpriteRenderer _spriteRenderer;
   private float _timer;
   private float size = 1;

   void Awake()
   {
      _numberOfShips = _iniaitalShips;
      _shipCounterText.text = _numberOfShips.ToString();
      _neutralColor = GetComponent<SpriteRenderer>().color;
      _spriteRenderer = GetComponent<SpriteRenderer>();
      UpdateDefineState();
   }
   private void Start()
   {
      _isSelected = false;
      _spriteRenderer.color = _updateColor;
      if (size <= 0.8f)
      {
         _shipPerSecond *= size;
      }
      else if (size > 0.8f && PlanetSize() <= 1.2f)
      {
         _shipPerSecond = 1f;
      }
      else if (size > 1.2f)
      {
         _shipPerSecond = 0.5f;
      }
   }

   private void Update()
   {
      NewShipTimer();
      UpdateState();
      UpdateDefineState();
      _spriteRenderer.color = _updateColor;
   }

   public void UpdateDefineState()
   {
      switch (_planetColor)
      {
         case PlanetColor.Enemy:
            gameObject.name = "Enemy Planet";
            gameObject.tag = "Enemy";
            _isEnemy = true;
            _updateColor = _enemyColor;
            break;
         case PlanetColor.Friendly:
            gameObject.name = "Friendly Planet";
            gameObject.tag = "Friendly";
            _isFreindly = true;
            _updateColor = _playerColor;
            break;
         case PlanetColor.Neutral:
            gameObject.name = "Neutral Planet";
            gameObject.tag = "Neutral";
            _shipCounterText.text = _iniaitalShips.ToString();
            _isNeutral = true;
            _updateColor = _neutralColor;
            break;
         default:
            gameObject.name = "Neutral Planet";
            gameObject.tag = "Neutral";
            _isNeutral = true;
            _updateColor = _neutralColor;
            break;
      }
   }

   public void UpdateState()
   {
      if (_isFreindly)
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

   private float PlanetSize()
   {
      size = Random.Range(0.6f, 1.6f);
      transform.localScale = new Vector3(size, size, size);
      return size;
   }

   public void RandomizePlanet()
   {
      int randomColor = Random.Range(0, 3);
      switch (randomColor)
      {
         case 0:
            _planetColor = PlanetColor.Enemy;
            break;
         case 1:
            _planetColor = PlanetColor.Friendly;
            break;
         case 2:
            _planetColor = PlanetColor.Neutral;
            break;
         default:
            _planetColor = PlanetColor.Neutral;
            break;
      }

      PlanetSize();

      if (PlanetSize() <= 0.8f)
      {
         _numberOfShips = Random.Range(1, 5);
         _maxShips = Random.Range(5, 10);
         _iniaitalShips = Random.Range(1, _numberOfShips);
      }
      else if (PlanetSize() > 0.8f && PlanetSize() <= 1.2f)
      {
         _numberOfShips = Random.Range(10, 20);
         _maxShips = Random.Range(20, 40);
         _iniaitalShips = Random.Range(6, _numberOfShips);
      }
      else if (PlanetSize() > 1.2f)
      {
         _numberOfShips = Random.Range(20, 40);
         _maxShips = Random.Range(40, 80);
         _iniaitalShips = Random.Range(21, _numberOfShips);
      }
   }
}
