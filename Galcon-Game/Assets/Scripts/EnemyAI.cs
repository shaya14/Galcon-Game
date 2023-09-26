using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Ship _shipPrefab;
    [SerializeField] private float _minTimeBetweenAttacks;
    [SerializeField] private float _maxTimeBetweenAttacks;
    [SerializeField] private float _initialTimeBetweenAttacks;
    //       2. Add a [SerializeField] private float _initialTimeBetweenAttacks; ------- why?
    //       i add this line you said in line 11 but why i need it?
    private float _timeBetweenAttacks;
    private float _timer;
    private Planet _thisPlanet;
    private float _attackingMinimum;

    void Start()
    {
        _thisPlanet = GetComponent<Planet>();
        _attackingMinimum = _thisPlanet.numberOfShips * 2;
        AttackMinimumNumberBySize(_attackingMinimum);
        _timeBetweenAttacks = UnityEngine.Random.Range(_minTimeBetweenAttacks, _maxTimeBetweenAttacks);
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
        SoundFx.Instance.PlaySound(SoundFx.Instance.attackSound, .3f);
    }

    // Assumes that 'HasTargets' was already called, and is true.
    private Planet ChooseTarget()
    {
        if (PlanetManager.Instance.friendlyPlanets.Count <= 0)
        {
            return PlanetManager.Instance.neutralPlanets[UnityEngine.Random.Range(0, PlanetManager.Instance.neutralPlanets.Count)];
        }
        if (PlanetManager.Instance.neutralPlanets.Count <= 0)
        {
            return PlanetManager.Instance.friendlyPlanets[UnityEngine.Random.Range(0, PlanetManager.Instance.friendlyPlanets.Count)];
        }

        int randomChoose = UnityEngine.Random.Range(0, 2);
        switch (randomChoose)
        {
            case 0:
                return PlanetManager.Instance.friendlyPlanets[UnityEngine.Random.Range(0, PlanetManager.Instance.friendlyPlanets.Count)];
            case 1:
                return PlanetManager.Instance.neutralPlanets[UnityEngine.Random.Range(0, PlanetManager.Instance.neutralPlanets.Count)];
        }

        throw new Exception("Unexpected random value!");
    }

    private void InstatiateAttackingShips(Planet target)
    {
        int shipToAttack = _thisPlanet.numberOfShips / 2;
        _thisPlanet.numberOfShips -= shipToAttack;

        for (int i = 0; i < shipToAttack; i++)
        {
            var ship = Instantiate(_shipPrefab, this.transform.position, Quaternion.identity);
            ship.Init(PlanetColor.Enemy, target);
            target.GetComponent<CircleCollider2D>().isTrigger = true;
            target._enemyTargetArrows.SetActive(true);
            target._attackingNumber++;
        }
    }

    private void TimeToNextAttack()
    {
        _timer += Time.deltaTime;
        if (_timer >= _timeBetweenAttacks)
        {
            _timeBetweenAttacks = UnityEngine.Random.Range(_minTimeBetweenAttacks, _maxTimeBetweenAttacks);
            if (HasTargets())
            {
                Attack();
            }
            _timer = 0;
        }
    }

    private bool HasTargets()
    {
        return PlanetManager.Instance.friendlyPlanets.Count > 0 ||
               PlanetManager.Instance.neutralPlanets.Count > 0;
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
