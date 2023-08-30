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
   public PlanetColor planetColor;

   [SerializeField] private float _shipCreationDelay;

   [SerializeField] private int _iniaitalShips;
   [SerializeField] public int _maxShips;
   [HideInInspector] public int numberOfShips;

   [Header("Player Colors")]
   [SerializeField] private Color _playerColor;
   [SerializeField] private Color _enemyColor;
   [SerializeField] private Color _neutralColor;

   [Header("UI Elements")]
   [SerializeField] private TextMeshPro _shipCounterText;
   [SerializeField] private TextMeshPro _maxShipCounterText;
   public GameObject _friendlyTargetArrows;
   public GameObject _enemyTargetArrows;
   private SpriteRenderer _spriteRenderer;
   private LineRenderer _lineRenderer;
   private float _timer;
   private float _size = 1;
   public int _attackingNumber = 0;
   public bool _isAdded = false;

   public bool isFriendly => planetColor == PlanetColor.Friendly;
   public bool isEnemy => planetColor == PlanetColor.Enemy;
   public bool isNeutral => planetColor == PlanetColor.Neutral;

   public Color playerColor => _playerColor;
   public Color enemyColor => _enemyColor;
   public float size => _size;

   void Awake()
   {
      _shipCounterText.text = numberOfShips.ToString();
      _maxShipCounterText.text = _maxShips.ToString();
      _neutralColor = GetComponent<SpriteRenderer>().color;
      _spriteRenderer = GetComponent<SpriteRenderer>();
   }
   private void Start()
   {
      numberOfShips = _iniaitalShips;
      
      // shipsPerSecond can also be named ShipCreationRate.
      // If you are using _shipsPerSecond, you need to MULTIPLY it by size (because bigger planets need to create more ships)
      // _shipsPerSecond *= size;
      _shipCreationDelay /= _size;
      
      UpdateNumOfShipsText();
      _lineRenderer = GetComponent<LineRenderer>();
   }

   private void Update()
   {
      UpdateShipTimer();
      UpdateDefineState();
      UpdateNumOfShipsText();


      if (numberOfShips >= _maxShips)
      {
         numberOfShips = _maxShips;
      }
   }

   public void UpdateDefineState()
   {
      switch (planetColor)
      {
         case PlanetColor.Enemy:
            gameObject.name = "Enemy Planet";
            _spriteRenderer.color = _enemyColor;
            break;
         case PlanetColor.Friendly:
            gameObject.name = "Friendly Planet";
            _spriteRenderer.color = _playerColor;
            break;
         case PlanetColor.Neutral:
            gameObject.name = "Neutral Planet";
            _spriteRenderer.color = _neutralColor;
            break;
      }
   }

   void UpdateShipTimer()
   {
      // If you are using shipsPerSecond, you need to compare _timer to (1.0 / shipsPerSecond)
      // float shipDelay = 1.0 / shipsPerSecond;
      // if (_timer > _shipDelay)

      if (_timer > _shipCreationDelay && numberOfShips < _maxShips)
      {
         _timer = 0;
         if (isFriendly || isEnemy)
         {
            numberOfShips++;
         }
         _shipCounterText.text = numberOfShips.ToString();
      }
      else
      {
         _timer += Time.deltaTime;
      }
   }

   #region Update Texts
   public void UpdateNumOfShipsText()
   {
      _shipCounterText.text = numberOfShips.ToString();
   }

   public void UpdateMaxNumOfShipsText()
   {
      _maxShipCounterText.text = _maxShips.ToString();
   }
   #endregion
   #region Planet Settings
   public void RandomizePlanet()
   {
      int randomColor = Random.Range(0, 3);
      switch (randomColor)
      {
         case 0:
            planetColor = PlanetColor.Friendly;
            break;
         case 1:
            planetColor = PlanetColor.Enemy;
            break;
         case 2:
            planetColor = PlanetColor.Neutral;
            break;
      }
   }

   public PlanetColor RandomizePlanetColor()
   {
      int randomColor = Random.Range(0, 3);
      switch (randomColor)
      {
         case 0:
            return planetColor = PlanetColor.Friendly;
         case 1:
            return planetColor = PlanetColor.Enemy;
         case 2:
            return planetColor = PlanetColor.Neutral;
      }
      return planetColor;
   }


   public void SetSettings(PlanetColor color, int numOfShips, int maxShips, float size)
   {
      planetColor = color;
      _iniaitalShips = numOfShips;
      numberOfShips = _iniaitalShips;
      _maxShips = maxShips;
      transform.localScale = new Vector3(size, size, size);
   }
   public void SetSettings(PlanetColor color, float size)
   {
      planetColor = color;
      transform.localScale = new Vector3(size, size, size);
      PlanetSetteings(size);
   }
   public void PlanetSetteings()
   {
      if (PlanetSize() <= 0.8f)
      {
         numberOfShips = Random.Range(8, 10);
         _maxShips = Random.Range(12, 25);
         _iniaitalShips = Random.Range(2, numberOfShips);
      }
      else if (PlanetSize() > 0.8f && PlanetSize() <= 1.2f)
      {
         numberOfShips = Random.Range(25, 35);
         _maxShips = Random.Range(37, 60);
         _iniaitalShips = Random.Range(10, numberOfShips);
      }
      else if (PlanetSize() > 1.2f)
      {
         numberOfShips = Random.Range(37, 50);
         _maxShips = Random.Range(60, 100);
         _iniaitalShips = Random.Range(35, numberOfShips);
      }
   }
   public void PlanetSetteings(float size)
   {
      if (size <= 0.8f)
      {
         numberOfShips = Random.Range(8, 10);
         _maxShips = Random.Range(12, 25);
         _iniaitalShips = Random.Range(2, numberOfShips);
      }
      else if (size > 0.8f && size <= 1.2f)
      {
         numberOfShips = Random.Range(25, 35);
         _maxShips = Random.Range(37, 60);
         _iniaitalShips = Random.Range(10, numberOfShips);
      }
      else if (size > 1.2f)
      {
         numberOfShips = Random.Range(37, 50);
         _maxShips = Random.Range(60, 100);
         _iniaitalShips = Random.Range(35, numberOfShips);
      }
   }
   public float PlanetSize()
   {
      _size = Random.Range(0.6f, 1.6f);
      transform.localScale = new Vector3(_size, _size, _size);
      return _size;
   }
   #endregion
   public void Hit(Ship ship)
   {
      _attackingNumber--;
      if (numberOfShips == 0)
      {
         numberOfShips = 1;
         planetColor = ship.shipColor;
         SoundFx.Instance.PlaySound(SoundFx.Instance._conquerSound, 1f);
         if (planetColor != ship.shipColor)
         {
            numberOfShips--;
         }
         return;
      }
      if (planetColor == ship.shipColor)
      {
         numberOfShips++;
      }
      else
      {
         numberOfShips--;
      }

      if(_attackingNumber <= 0)
      {
         _attackingNumber = 0;
         _friendlyTargetArrows.SetActive(false);
         _enemyTargetArrows.SetActive(false);
         GetComponent<CircleCollider2D>().isTrigger = false;
      }
   }

  public bool isSelected {
  get {
    foreach (Planet planet in PlanetManager.Instance._selectedPlanets) {
      // if (this == planet) { // 
      if (this.gameObject.GetInstanceID() == planet.gameObject.GetInstanceID()) {
        return true;
      }
    }

    return false;
  }
  }
}
