using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Ship _attackingShip;
    [SerializeField] private Color _enemyColor;
    [SerializeField] private float _minAttackRate;
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
        if (!_thisPlanet.isEnemy)
        {
            return;
        }

        if (_thisPlanet.numberOfShips >= _thisPlanet._maxShips / 2)
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
        SoundFx.Instance.PlaySound(SoundFx.Instance._attackSound, .3f);
    }

    private Planet ChooseTarget()
    {
        Planet friendlyTarget = null;
        Planet neutralTarget = null;
        Planet target = null;

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

        if (target.isEnemy)
        {
            if (target.numberOfShips >= target._maxShips)
            {
                return ChooseTarget();
            }
        }

        return target;
    }

    private void InstatiateAttackingShips(Planet _target)
    {
        _thisPlanet.numberOfShips /= 2;
        _thisPlanet.UpdateNumOfShipsText();
        for (int i = 0; i < _thisPlanet.numberOfShips; i++)
        {
            //Ship ship = Instantiate<Ship>(_attackingShip, this.transform.position, Quaternion.identity);
            Ship ship = ObjectPool.Instance.GetShipFromPool();
            ship.gameObject.SetActive(true);
            ship.transform.position = this.transform.position;
            ship.ShipColor("red");
            _target.GetComponent<CircleCollider2D>().isTrigger = true;
            _target._enemyTargetArrows.SetActive(true);
            ship._targetPlanet = _target;
            _target._attackingNumber++;
        }
    }

    private void TimeToNextAttack()
    {
        _timer += Time.deltaTime;
        if (_timer >= _attackRate)
        {
            _attackRate = Random.Range(_minAttackRate, _maxAttackRate);
            if (_hasTargets)
                Attack();
            _timer = 0;
        }
    }
}
