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

    public float _attackingMinimum;

    void Start()
    {
        _thisPlanet = GetComponent<Planet>();
        _attackingMinimum = _thisPlanet.numberOfShips * 2;
        AttackMinimumNumberBySize(_attackingMinimum);
    }

    void Update()
    {
        if (!_thisPlanet.isEnemy)
        {
            return;
        }

        if (_thisPlanet.numberOfShips >= _attackingMinimum / 2)
        {
            TimeToNextAttack();
        }

        if (PlanetManager.Instance.friendlyPlanets.Count <= 0 && PlanetManager.Instance.neutralPlanets.Count <= 0)
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
                if (PlanetManager.Instance.friendlyPlanets.Count <= 0)
                {
                    return ChooseTarget();
                }
                friendlyTarget = PlanetManager.Instance.friendlyPlanets[Random.Range(0, PlanetManager.Instance.friendlyPlanets.Count)];
                target = friendlyTarget;
                break;
            case 1:
                if (PlanetManager.Instance.neutralPlanets.Count <= 0)
                {
                    return ChooseTarget();
                }
                neutralTarget = PlanetManager.Instance.neutralPlanets[Random.Range(0, PlanetManager.Instance.neutralPlanets.Count)];
                target = neutralTarget;
                break;
        }

        if (PlanetManager.Instance.friendlyPlanets.Count <= 0)
        {
            friendlyTarget = neutralTarget;
            target = friendlyTarget;
        }
        else if (PlanetManager.Instance.neutralPlanets.Count <= 0)
        {
            neutralTarget = friendlyTarget;
            target = neutralTarget;
        }

        // if (target.isEnemy)
        // {
        //    // add code to choose another target or help the other enemy
        // }

        return target;
    }

    private void InstatiateAttackingShips(Planet _target)
    {
        _thisPlanet.numberOfShips /= 2;
        _thisPlanet.UpdateNumOfShipsText();
        for (int i = 0; i < _thisPlanet.numberOfShips; i++)
        {
            Ship ship = Instantiate<Ship>(_attackingShip, this.transform.position, Quaternion.identity);
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

    public void AttackMinimumNumberBySize(float num)
    {
        if (num <= 8)
        {
            _attackingMinimum *= 6;
        }   
        else if (num > 8 && num <= 12)
        {
            _attackingMinimum *= 4;
        }
        else if(num > 12 && num <= 16)
        {
            _attackingMinimum *= 3;
        }
        else if (num > 16 && num <=25)
        {
            _attackingMinimum *= 2;
        }
    }
}
