using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float _speed;
    [HideInInspector] public Planet _targetPlanet;
    public ParticleSystem _BlastParticlePrefab;
    private PlanetColor _shipColor;
    public PlanetColor shipColor => _shipColor;

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
            if (this.shipColor == PlanetColor.Friendly)
            {
                PlanetManager.Instance._friendlyAttackingShipsForce.Remove(this);
                if (PlanetManager.Instance._friendlyAttackingShipsForce.Count <= 0)
                {
                    _targetPlanet.GetComponent<CircleCollider2D>().isTrigger = false;
                    PlanetManager.Instance.DeleteList(PlanetManager.Instance._friendlyAttackingShipsForce);
                }
            }
            else if (this.shipColor == PlanetColor.Enemy)
            {
                PlanetManager.Instance._enemyAttackingShipsForce.Remove(this);
                if (PlanetManager.Instance._enemyAttackingShipsForce.Count <= 0)
                {
                    _targetPlanet.GetComponent<CircleCollider2D>().isTrigger = false;
                    PlanetManager.Instance.DeleteList(PlanetManager.Instance._enemyAttackingShipsForce);
                }
            }


            _targetPlanet.Hit(this);

            if (_targetPlanet.planetColor != _shipColor)
            {
                ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
                Destroy(blast.gameObject, 1f);
            }
            Destroy(gameObject);
        }
    }
}
