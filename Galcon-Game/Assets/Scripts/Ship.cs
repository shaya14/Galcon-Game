using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    //     1. Don't keep default-values in the code - keep them in prefabs.
    //        When you have default values in the code, the value of a specific object depends on 3 
    //        things:
    //          * value in code.
    //          * value in prefab.
    //          * value in scene.
    //        By never having default-values in the code, you remove the complexity of this to 2 things:
    //          * value in prefab
    //          * value in scene.
    //        Note also the Unity will highlight in blue when the value in the scene is different 
    //        from the value in the prefab - but there's no indication if the value in the prefab
    //        is the same as the value in the code or not...
    [SerializeField] private float _speed = 2f;
    [HideInInspector] public Planet _targetPlanet;
    public ParticleSystem _BlastParticlePrefab;

    // CR: same idea about reducing the amount of state (and making public state private, wherever possible).
    //     Because _shipColor is never CHANGED from other classes, and is only changed from Ship,
    //     you can make it 'private', and add a *public readonly property* for when other classes need to access it.
    //     
    //     private PlanetColor _shipColor;
    //     public PlanetColor shipColor => _shipColor;

    public PlanetColor _shipColor;
    public bool _imEnemyShip; // CR: unused, delete

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
            // PlanetManager.Instance._attackingShipsList.Remove(this);

            // if(PlanetManager.Instance._attackingShipsList.Count <= 0)
            // {
            //     _targetPlanet.GetComponent<CircleCollider2D>().isTrigger = false;
            // }
            _targetPlanet.Hit(this);
            
            if (_targetPlanet.planetColor != _shipColor)
            {
                ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
                Destroy(blast.gameObject, 1f);
            }
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
    // }
}
