using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [HideInInspector] public GameObject _targetPlanet;

    bool _isUnderAttack = true;
    bool _isConquered = false;

    void Start()
    {

    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPlanet.transform.position, _speed * Time.deltaTime);
        transform.up = _targetPlanet.transform.position - transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _targetPlanet)
        {
            _targetPlanet.GetComponent<Planet>().UpdateDefineState();
            if (_targetPlanet.GetComponent<Planet>()._isFreindly)
            {
                if (_targetPlanet.GetComponent<Planet>()._numberOfShips > 0)
                {
                    _targetPlanet.GetComponent<Planet>()._numberOfShips++;
                }
                else if (_targetPlanet.GetComponent<Planet>()._numberOfShips <= 0)
                {
                    _targetPlanet.GetComponent<Planet>()._isFreindly = false;
                    _targetPlanet.GetComponent<Planet>()._isEnemy = true;
                }
            }
            else if (_targetPlanet.GetComponent<Planet>()._isEnemy)
            {
                if (_targetPlanet.GetComponent<Planet>()._numberOfShips > 0)
                {
                    _targetPlanet.GetComponent<Planet>()._numberOfShips--;
                }
                else if (_targetPlanet.GetComponent<Planet>()._numberOfShips <= 0)
                {
                    _targetPlanet.GetComponent<Planet>()._isFreindly = true;
                    _targetPlanet.GetComponent<Planet>()._isEnemy = false;
                }
            }
            else if (_targetPlanet.GetComponent<Planet>()._isNeutral)
            {
                if (_targetPlanet.GetComponent<Planet>()._numberOfShips >= 0)
                {
                    _targetPlanet.GetComponent<Planet>()._numberOfShips--;
                }
                else if (_targetPlanet.GetComponent<Planet>()._numberOfShips <= 0)
                {
                    _targetPlanet.GetComponent<Planet>()._isFreindly = true;
                    _targetPlanet.GetComponent<Planet>()._isNeutral = false;
                }
            }
            _targetPlanet.GetComponent<Planet>().UpdateNumOfShipsText();          
            Destroy(gameObject);
        }
    }
}
