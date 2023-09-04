using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Ship _shipPrefab;
    [SerializeField] private Color _enemyColor;
    [SerializeField] private float _minTimeBetweenAttacks;
    [SerializeField] private float _maxTimeBetweenAttacks;
    private float _timeBetweenAttacks = 5f;
    private float _timer;
    private Planet _thisPlanet;
    private float _attackingMinimum;

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
    }

    public void Attack()
    {
        InstatiateAttackingShips(ChooseTarget());
        SoundFx.instance.PlaySound(SoundFx.instance._attackSound, .3f);
    }

    // CR: [discuss in class]
    private Planet ChooseTarget()
    {
        Planet friendlyTarget = null;
        Planet neutralTarget = null;
        Planet target = null;

        int randomChoose = Random.Range(0, 2);
        switch (randomChoose)
        {
            case 0:
                if (PlanetManager.instance.friendlyPlanets.Count <= 0)
                {
                    return ChooseTarget();
                }
                friendlyTarget = PlanetManager.instance.friendlyPlanets[Random.Range(0, PlanetManager.instance.friendlyPlanets.Count)];
                target = friendlyTarget;
                break;
            case 1:
                if (PlanetManager.instance.neutralPlanets.Count <= 0)
                {
                    return ChooseTarget();
                }
                neutralTarget = PlanetManager.instance.neutralPlanets[Random.Range(0, PlanetManager.instance.neutralPlanets.Count)];
                target = neutralTarget;
                break;
        }

        if (PlanetManager.instance.friendlyPlanets.Count <= 0)
        {
            friendlyTarget = neutralTarget;
            target = friendlyTarget;
        }
        else if (PlanetManager.instance.neutralPlanets.Count <= 0)
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

    private void InstatiateAttackingShips(Planet target)
    {
        _thisPlanet.numberOfShips /= 2;
        _thisPlanet.UpdateNumOfShipsText();
        for (int i = 0; i < _thisPlanet.numberOfShips; i++)
        {
            Ship ship = Instantiate<Ship>(_shipPrefab, this.transform.position, Quaternion.identity);
            ship.SetShipColor(PlanetColor.Enemy);
            target.GetComponent<CircleCollider2D>().isTrigger = true;
            target._enemyTargetArrows.SetActive(true);
            ship._targetPlanet = target;
            target._attackingNumber++;
        }
    }

    private void TimeToNextAttack()
    {
        _timer += Time.deltaTime;
        if (_timer >= _timeBetweenAttacks)
        {
            _timeBetweenAttacks = Random.Range(_minTimeBetweenAttacks, _maxTimeBetweenAttacks);
            if (HasTargets())
            {
                Attack();
            }
            _timer = 0;
        }
    }

    private bool HasTargets()
    {
        return PlanetManager.instance.friendlyPlanets.Count > 0 ||
               PlanetManager.instance.neutralPlanets.Count > 0;
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
        else if (num > 12 && num <= 16)
        {
            _attackingMinimum *= 3;
        }
        else if (num > 16 && num <= 25)
        {
            _attackingMinimum *= 2;
        }
    }
}
