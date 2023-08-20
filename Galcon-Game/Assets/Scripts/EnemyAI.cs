using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Ship _attackingShip;
    [SerializeField] private Color _enemyColor;
    [SerializeField] private float _mixAttackRate;
    [SerializeField] private float _maxAttackRate;
    private float _attackRate = 5f;
    private float _timer;
    public bool _hasTargets = true;

    private Planet _thisPlanet;

    void Start()
    {
        _thisPlanet = GetComponent<Planet>();
    }

    void Update()
    {
        if (_thisPlanet._numberOfShips >= _thisPlanet._maxShips / 2)
        {
            TimeToNextAttack();
        }

        if (PlanetManager.Instance._friendlyPlanets.Count <= 0 && PlanetManager.Instance._neutralPlanets.Count <= 0)
        {
            _hasTargets = false;
        }
        else
        {
            _hasTargets = true;
        }
    }

    public void Attack()
    {
        InstatiateAttackingShips(ChooseTarget());
    }

    private GameObject ChooseTarget()
    {
        GameObject friendlyTarget = null;
        GameObject neutralTarget = null;
        GameObject target = null;

        int randomChoose = Random.Range(0, 2);
        switch (randomChoose)
        {
            case 0:
                if (PlanetManager.Instance._friendlyPlanets.Count <= 0)
                {
                    return ChooseTarget();
                }
                friendlyTarget = PlanetManager.Instance._friendlyPlanets[Random.Range(0, PlanetManager.Instance._friendlyPlanets.Count)];
                target = friendlyTarget;
                break;
            case 1:
                if (PlanetManager.Instance._neutralPlanets.Count <= 0)
                {
                    return ChooseTarget();
                }
                neutralTarget = PlanetManager.Instance._neutralPlanets[Random.Range(0, PlanetManager.Instance._neutralPlanets.Count)];
                target = neutralTarget;
                break;
        }

        if (PlanetManager.Instance._friendlyPlanets.Count <= 0)
        {
            friendlyTarget = neutralTarget;
            target = friendlyTarget;
        }
        else if (PlanetManager.Instance._neutralPlanets.Count <= 0)
        {
            neutralTarget = friendlyTarget;
            target = neutralTarget;
        }

        if (target.GetComponent<Planet>()._isEnemy)
        {
            if (target.GetComponent<Planet>()._numberOfShips >= target.GetComponent<Planet>()._maxShips)
            {
                return ChooseTarget();
            }
        }

        return target.gameObject;
    }

    private void InstatiateAttackingShips(GameObject _target)
    {
        _thisPlanet._numberOfShips /= 2;
        _thisPlanet.UpdateNumOfShipsText();

        for (int i = 0; i < _thisPlanet._numberOfShips; i++)
        {
            GameObject ship = Instantiate(_attackingShip.gameObject, this.transform.position, Quaternion.identity);
            ship.GetComponent<SpriteRenderer>().color = _enemyColor;
            ship.GetComponent<Ship>()._targetPlanet = _target;
            ship.GetComponent<Ship>()._imEnemyShip = true;
        }
    }

    private void TimeToNextAttack()
    {
        _timer += Time.deltaTime;
        if (_timer >= _attackRate)
        {
            _attackRate = Random.Range(_mixAttackRate, _maxAttackRate);
            if (_hasTargets)
                Attack();
            // foreach (GameObject planet in GameManager.Instance._enemyPlanets)
            // {
            //     if (planet.GetComponent<Planet>()._numberOfShips > 0)
            //     {
            //         Attack();
            //         break;
            //     }
            // }
            _timer = 0;
        }
    }
}
