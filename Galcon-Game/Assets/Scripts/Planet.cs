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

   [SerializeField] private float _shipPerSecond;
   [SerializeField] public int _iniaitalShips; // CR: change to private
   [SerializeField] public int _maxShips;
   [HideInInspector] public int _numberOfShips; // CR: since it's public, rename to 'public int numberOfShips'.

   [Header("Player Colors")]
   [SerializeField] private Color _playerColor;
   [SerializeField] private Color _enemyColor;
   [SerializeField] private Color _neutralColor;
   [HideInInspector] public Color _updateColor;

   [Header("UI Elements")]
   [SerializeField] private TextMeshPro _shipCounterText;
   [SerializeField] private TextMeshPro _maxShipCounterText;

   // CR: if it's public, rename to 'public bool isSelected' (the '_' is used only for private fields).
   public bool _isSelected;
   private SpriteRenderer _spriteRenderer;
   private LineRenderer _lineRenderer;
   private float _timer;
   private float _size = 1;

   public bool isFriendly => planetColor == PlanetColor.Friendly;
   public bool isEnemy => planetColor == PlanetColor.Enemy;
   public bool isNeutral => planetColor == PlanetColor.Neutral;
   public float size => _size;

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
      _numberOfShips = _iniaitalShips;
      _shipPerSecond /= _size; // CR: bigger planet => less ships per second? (size *= 2 => shipPerSecond /= 2)
      UpdateNumOfShipsText();
      _lineRenderer = GetComponent<LineRenderer>();
   }

   private void Update()
   {
      UpdateShipTimer();
      UpdateDefineState();
      UpdateNumOfShipsText();


      if (_numberOfShips >= _maxShips)
      {
         _numberOfShips = _maxShips;
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
      if (_timer > _shipPerSecond && _numberOfShips < _maxShips)
      {
         _timer = 0;
         if (isFriendly || isEnemy)
         {
            _numberOfShips++;
         }
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

   public void SetSettings(PlanetColor color, int numOfShips, int maxShips, float size)
   {
      planetColor = color;
      _iniaitalShips = numOfShips;
      _numberOfShips = _iniaitalShips;
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


   public void PlanetSetteings(float size)
   {
      if (size <= 0.8f)
      {
         _numberOfShips = Random.Range(8, 10);
         _maxShips = Random.Range(12, 25);
         _iniaitalShips = Random.Range(2, _numberOfShips);
      }
      else if (size > 0.8f && PlanetSize() <= 1.2f)
      {
         _numberOfShips = Random.Range(25, 35);
         _maxShips = Random.Range(37, 60);
         _iniaitalShips = Random.Range(10, _numberOfShips);
      }
      else if (size > 1.2f)
      {
         _numberOfShips = Random.Range(37, 50);
         _maxShips = Random.Range(60, 100);
         _iniaitalShips = Random.Range(35, _numberOfShips);
      }
   }

   public void Hit(Ship ship)
   {

      //GetComponent<CircleCollider2D>().isTrigger = true;
      if (_numberOfShips == 0)
      {
         _numberOfShips = 1;
         planetColor = ship._shipColor;
         PlanetManager.Instance.UpdateLists(ship._targetPlanet);
         if (planetColor != ship._shipColor)
         {
            _numberOfShips--;
         }
         return;
      }
      if (planetColor == ship._shipColor)
      {
         _numberOfShips++;
      }
      else
      {
         _numberOfShips--;
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
