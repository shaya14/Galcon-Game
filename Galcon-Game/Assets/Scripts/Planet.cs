using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _shipCounterText;
   [SerializeField] private float _shipPerSecond;
   [SerializeField] int _numberOfShips;

   public Color _defaultColor;
   public Color _selectedColor;

   public bool _isSelected;
   

   private float _timer;

   private void Start()
   {
      _shipCounterText.text = _numberOfShips.ToString();
      _defaultColor = GetComponent<SpriteRenderer>().color;
      _isSelected = false;
   }

   private void Update()
   {
      NewShipTimer();
   }

   void NewShipTimer()
   {
      if(_timer > _shipPerSecond)
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
}
