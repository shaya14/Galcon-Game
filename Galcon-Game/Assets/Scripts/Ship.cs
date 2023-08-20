using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [HideInInspector] public GameObject _targetPlanet;
    [SerializeField] private ParticleSystem _BlastParticlePrefab;

    public bool _imEnemyShip;

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
        Planet targetPlanet = _targetPlanet.GetComponent<Planet>();

        if (collision.gameObject == _targetPlanet && !_imEnemyShip)
        {
            if (targetPlanet._isFriendly)
            {
                if (targetPlanet._numberOfShips < targetPlanet._maxShips)
                {
                    targetPlanet._numberOfShips++;
                }
            }
            else if (targetPlanet._isEnemy)
            {
                if (targetPlanet._numberOfShips > 0)
                {
                    targetPlanet._numberOfShips--;
                    ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
                    Destroy(blast.gameObject, 1f);
                }
                else if (targetPlanet._numberOfShips <= 0)
                {
                    targetPlanet._isFriendly = true;
                    targetPlanet._isEnemy = false;
                    PlanetManager.Instance._enemyPlanets.Remove(targetPlanet.gameObject);
                    PlanetManager.Instance._friendlyPlanets.Add(targetPlanet.gameObject);
                    PlanetManager.Instance._enemiesToSelect.Remove(targetPlanet.gameObject);

                    // Why when its enable i get error?
                    //GameManager.Instance._enemies.Remove(_targetPlanet);
                }
            }
            else if (targetPlanet._isNeutral)
            {
                targetPlanet._iniaitalShips--;
                ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
                Destroy(blast.gameObject, 1f);

                if (targetPlanet._iniaitalShips <= 0)
                {
                    targetPlanet._numberOfShips = 0;
                    targetPlanet._isFriendly = true;
                    targetPlanet._isNeutral = false;
                    PlanetManager.Instance._friendlyPlanets.Add(targetPlanet.gameObject);
                    PlanetManager.Instance._enemiesToSelect.Remove(targetPlanet.gameObject);
                    PlanetManager.Instance._neutralPlanets.Remove(targetPlanet.gameObject);

                    // Why when its enable i get error?
                    //GameManager.Instance._enemies.Remove(_targetPlanet);
                }
            }
            targetPlanet.UpdateNumOfShipsText();
            Destroy(gameObject);
        }
        else if (collision.gameObject == _targetPlanet && _imEnemyShip)
        {
            if (targetPlanet._isFriendly)
            {
                targetPlanet._numberOfShips--;
                ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
                Destroy(blast.gameObject, 1f);

                if (targetPlanet._numberOfShips <= 0)
                {
                    targetPlanet._numberOfShips = 0;
                    targetPlanet._isEnemy = true;
                    targetPlanet._isFriendly = false;
                    PlanetManager.Instance._enemyPlanets.Add(targetPlanet.gameObject);
                    PlanetManager.Instance._friendlyPlanets.Remove(targetPlanet.gameObject);
                    PlanetManager.Instance._enemiesToSelect.Add(targetPlanet.gameObject);
                }
            }
            else if (targetPlanet._isEnemy)
            {
                if (targetPlanet._numberOfShips < targetPlanet._maxShips)
                {
                    targetPlanet._numberOfShips++;
                }
            }
            else if (targetPlanet._isNeutral)
            {
                targetPlanet._iniaitalShips--;
                ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
                Destroy(blast.gameObject, 1f);
                if (targetPlanet._iniaitalShips <= 0)
                {
                    targetPlanet._numberOfShips = 0;
                    targetPlanet._isEnemy = true;
                    targetPlanet._isNeutral = false;
                    PlanetManager.Instance._enemyPlanets.Add(targetPlanet.gameObject);
                    PlanetManager.Instance._enemiesToSelect.Add(targetPlanet.gameObject);
                    PlanetManager.Instance._neutralPlanets.Remove(targetPlanet.gameObject);
                }
            }

            targetPlanet.UpdateNumOfShipsText();
            Destroy(gameObject);
        }
    }
}
