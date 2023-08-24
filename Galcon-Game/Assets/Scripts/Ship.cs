using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [HideInInspector] public Planet _targetPlanet;
    [SerializeField] private ParticleSystem _BlastParticlePrefab;

    // CR: instead if '_imEnemyShip', I suggest 
    //       private PlanetColor _myColor; 
    //     This will allow you to reduce code duplication in OnTriggerEnter.
    public PlanetColor _shipColor;
    public bool _imEnemyShip;

    [SerializeField] private Color _playerColor;
    [SerializeField] private Color _enemyColor;

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPlanet.transform.position, _speed * Time.deltaTime);
        transform.up = _targetPlanet.transform.position - transform.position;
    }

    public void ShipColor(string color)
    {
        switch (color)
        {
            case "red":
                _shipColor = PlanetColor.Enemy;
                GetComponent<SpriteRenderer>().color = _enemyColor;
                break;
            case "blue":
                _shipColor = PlanetColor.Friendly;
                GetComponent<SpriteRenderer>().color = _playerColor;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetInstanceID() == _targetPlanet.gameObject.GetInstanceID())
        {
            _targetPlanet.Hit(this);
            Destroy(gameObject);
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.GetInstanceID() == _targetPlanet.gameObject.GetInstanceID() 
    //         && !_imEnemyShip)
    //     {
    //         if (_targetPlanet.isFriendly)
    //         {
    //             if (_targetPlanet._numberOfShips < _targetPlanet._maxShips)
    //             {
    //                 _targetPlanet._numberOfShips++;
    //             }
    //         }
    //         else if (_targetPlanet.isEnemy)
    //         {
    //             if (_targetPlanet._numberOfShips > 0)
    //             {
    //                 _targetPlanet._numberOfShips--;
    //                 ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
    //                 Destroy(blast.gameObject, 1f);
    //             }
    //             else if (_targetPlanet._numberOfShips <= 0)
    //             {
    //                 _targetPlanet.planetColor = PlanetColor.Friendly;
    //                 PlanetManager.Instance._enemyPlanets.Remove(_targetPlanet);
    //                 PlanetManager.Instance._friendlyPlanets.Add(_targetPlanet);
    //                 PlanetManager.Instance._enemiesToSelect.Remove(_targetPlanet);

    //                 // Why when its enable i get error?
    //                 //GameManager.Instance._enemies.Remove(_targetPlanet);
    //             }
    //         }
    //         else if (_targetPlanet.isNeutral)
    //         {
    //             _targetPlanet._iniaitalShips--;
    //             ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
    //             Destroy(blast.gameObject, 1f);

    //             if (_targetPlanet._iniaitalShips <= 0)
    //             {
    //                 _targetPlanet._numberOfShips = 0;
    //                 _targetPlanet.planetColor = PlanetColor.Friendly;
    //                 PlanetManager.Instance._friendlyPlanets.Add(_targetPlanet);
    //                 PlanetManager.Instance._enemiesToSelect.Remove(_targetPlanet);
    //                 PlanetManager.Instance._neutralPlanets.Remove(_targetPlanet);

    //                 // Why when its enable i get error?
    //                 //GameManager.Instance._enemies.Remove(_targetPlanet);
    //             }
    //         }
    //         _targetPlanet.UpdateNumOfShipsText();
    //         Destroy(gameObject);
    //     }
    //     else if (collision.gameObject.GetInstanceID() == _targetPlanet.gameObject.GetInstanceID() 
    //              && _imEnemyShip)
    //     {
    //         if (_targetPlanet.isFriendly)
    //         {
    //             _targetPlanet._numberOfShips--;
    //             ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
    //             Destroy(blast.gameObject, 1f);

    //             if (_targetPlanet._numberOfShips <= 0)
    //             {
    //                 _targetPlanet._numberOfShips = 0;
    //                 _targetPlanet.planetColor = PlanetColor.Enemy;
    //                 PlanetManager.Instance._enemyPlanets.Add(_targetPlanet);
    //                 PlanetManager.Instance._friendlyPlanets.Remove(_targetPlanet);
    //                 PlanetManager.Instance._enemiesToSelect.Add(_targetPlanet);
    //             }
    //         }
    //         else if (_targetPlanet.isEnemy)
    //         {
    //             if (_targetPlanet._numberOfShips < _targetPlanet._maxShips)
    //             {
    //                 _targetPlanet._numberOfShips++;
    //             }
    //         }
    //         else if (_targetPlanet.isNeutral)
    //         {
    //             _targetPlanet._iniaitalShips--;
    //             ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
    //             Destroy(blast.gameObject, 1f);
    //             if (_targetPlanet._iniaitalShips <= 0)
    //             {
    //                 _targetPlanet._numberOfShips = 0;
    //                 _targetPlanet.planetColor = PlanetColor.Enemy;
    //                 PlanetManager.Instance._enemyPlanets.Add(_targetPlanet);
    //                 PlanetManager.Instance._enemiesToSelect.Add(_targetPlanet);
    //                 PlanetManager.Instance._neutralPlanets.Remove(_targetPlanet);
    //             }
    //         }

    //         _targetPlanet.UpdateNumOfShipsText();
    //         Destroy(gameObject);
    //     }
    //}
}
