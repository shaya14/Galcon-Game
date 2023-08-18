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
   [SerializeField] private TextMeshProUGUI _shipCounterText;
   [SerializeField] private float _shipPerSecond;
   [SerializeField] private GameObject _attackingShips;
   public int _numberOfShips;

   public PlanetColor _planetColor;
   public bool _isSelected;

   [Header("Player Colors")]
   [SerializeField] private Color _enemyColor;
   [SerializeField] private Color _playerColor;

   [Header("Game Colors")]
   public Color _defaultColor;
   public Color _updateColor;
   public Color _selectedColor;

   public bool _isFreindly;
   public bool _isEnemy;
   public bool _isNeutral;

   private SpriteRenderer _spriteRenderer;
   private float _timer;

   void Awake()
   {
      _shipCounterText.text = _numberOfShips.ToString();
      _defaultColor = GetComponent<SpriteRenderer>().color;
      _spriteRenderer = GetComponent<SpriteRenderer>();
      UpdateDefineState();
   }
   private void Start()
   {
      _isSelected = false;
      _spriteRenderer.color = _updateColor;
   }

   private void Update()
   {
      NewShipTimer();
      UpdateState();
      UpdateDefineState();
      _defaultColor = _updateColor;
      _spriteRenderer.color = _updateColor;
   }

   public void UpdateDefineState()
   {
      switch (_planetColor)
      {
         case PlanetColor.Enemy:
            gameObject.tag = "Enemy";
            _isEnemy = true;
            _updateColor = _enemyColor;
            break;
         case PlanetColor.Friendly:
            gameObject.tag = "Player";
            _isFreindly = true;
            _updateColor = _playerColor;
            break;
         case PlanetColor.Neutral:
            gameObject.tag = "Neutral";
            _isNeutral = true;
            _updateColor = _defaultColor;
            break;
         default:
            gameObject.tag = "Neutral";
            _isNeutral = true;
            _updateColor = _defaultColor;
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
      if (_timer > _shipPerSecond)
      {
         _timer = 0;
         _numberOfShips++;
         _shipCounterText.text = _numberOfShips.ToString();
         //_shipCounterText.text = (int.Parse(_shipCounterText.text) + 1).ToString();
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

      public void SpawnShips(Planet planet)
   {
         planet.GetComponent<Planet>()._numberOfShips /= 2;
         planet.GetComponent<Planet>().UpdateNumOfShipsText();
         for (int i = 0; i < planet._numberOfShips; i++)
         {
            GameObject ship = Instantiate(_attackingShips, planet.transform.position, Quaternion.identity);
         }
      
         planet.GetComponent<SpriteRenderer>().color = planet.GetComponent<Planet>()._defaultColor;
         planet.GetComponent<Planet>()._isSelected = false;
         planet.GetComponent<TargetGlow>().SetGlowOff();
   }
}
