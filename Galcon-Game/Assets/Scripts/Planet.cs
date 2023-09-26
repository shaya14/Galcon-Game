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
   [SerializeField] private float _shipsPerSecond;
   [SerializeField] private int _iniaitalShips;
   [HideInInspector] public int numberOfShips;

   [Header("Player Colors")]
   [SerializeField] private Color _playerColor;
   [SerializeField] private Color _enemyColor;
   [SerializeField] private Color _neutralColor;
   
   [Header("UI Elements")]
   [SerializeField] private TextMeshPro _shipCounterText;
   [SerializeField] private TextMeshPro _shipPerMinuteText;
   public GameObject _friendlyTargetArrows;
   public GameObject _enemyTargetArrows;
   private SpriteRenderer _spriteRenderer;
   private float _timer;
   private float _size = 1;

   // CR: [single source of truth] delete this!
   //     _shipPerMinute is always 60 * _shipPerSecond, so it shouldn't be a separate field.
   //     When it is a separate field, it leaves the option of bugs where e.g. _shipPerSecond is 0.5 but 
   //     _shipPerMinute is 100, or 0, or 10, ... (should be 30 of course).
   //     replace it with:
   //       public int shipPerMinute => _shipPerSecond * 60;
   private float _shipPerMinute;


   public int _attackingNumber = 0;
   public bool _isAddedToSelectedPlanets = false;
   public bool isFriendly => planetColor == PlanetColor.Friendly;
   public bool isEnemy => planetColor == PlanetColor.Enemy;
   public bool isNeutral => planetColor == PlanetColor.Neutral;
   public Color playerColor => _playerColor;
   public Color enemyColor => _enemyColor;
   public float size => _size;
   public float shipPerSecond { get => _shipsPerSecond; set => _shipsPerSecond = value; }
   public float shipPerMinute { get => _shipPerMinute; set => _shipPerMinute = value; }

   void Awake()
   {
      _shipCounterText.text = numberOfShips.ToString();
      _shipPerMinuteText.text = shipPerMinute.ToString();
      _spriteRenderer = GetComponent<SpriteRenderer>();
   }
   private void Start()
   {
      numberOfShips = _iniaitalShips;
      _shipsPerSecond *= size;
      UpdateNumOfShipsText();
   }

   private void Update()
   {
      UpdateShipTimer();
      UpdateDefineState();
      UpdateNumOfShipsText();
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
      float shipDelay = 1.0f / _shipsPerSecond;
      if (_timer > shipDelay)
      {
         _timer = 0;
         if (isFriendly || isEnemy)
         {
            numberOfShips++;
         }
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
      _shipPerMinuteText.text = shipPerMinute.ToString();
   }
   #endregion
   #region Planet Settings
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


   public void SetSettings(PlanetColor color, int numOfShips, float size)
   {
      planetColor = color;
      _iniaitalShips = numOfShips;
      numberOfShips = _iniaitalShips;
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
         _iniaitalShips = Random.Range(2, numberOfShips);
      }
      else if (PlanetSize() > 0.8f && PlanetSize() <= 1.2f)
      {
         numberOfShips = Random.Range(25, 35);
         _iniaitalShips = Random.Range(10, numberOfShips);
      }
      else if (PlanetSize() > 1.2f)
      {
         numberOfShips = Random.Range(37, 50);
         _iniaitalShips = Random.Range(35, numberOfShips);
      }
   }
   public void PlanetSetteings(float size)
   {
      if (size <= 0.8f)
      {
         numberOfShips = Random.Range(8, 10);
         _iniaitalShips = Random.Range(2, numberOfShips);
      }
      else if (size > 0.8f && size <= 1.2f)
      {
         numberOfShips = Random.Range(25, 35);
         _iniaitalShips = Random.Range(10, numberOfShips);
      }
      else if (size > 1.2f)
      {
         numberOfShips = Random.Range(37, 50);
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
         SoundFx.Instance.PlaySound(SoundFx.Instance.conquerSound, 1f);
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

      if (_attackingNumber <= 0)
      {
         _attackingNumber = 0;
         _friendlyTargetArrows.SetActive(false);
         _enemyTargetArrows.SetActive(false);
         GetComponent<CircleCollider2D>().isTrigger = false;
      }
   }

   public bool isSelected
   {
      get
      {
         foreach (Planet planet in PlanetManager.Instance._selectedPlanets)
         {
            // if (this == planet) { // 
            if (this.gameObject.GetInstanceID() == planet.gameObject.GetInstanceID())
            {
               return true;
            }
         }

         return false;
      }
   }
}
