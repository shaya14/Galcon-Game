using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float _speed;
    [HideInInspector] public Planet _targetPlanet;
    public ParticleSystem _BlastParticlePrefab; // CR: serializefield private
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

    public void SetShipColor(PlanetColor color)
    {
        switch (color)
        {
            case PlanetColor.Enemy:
                _shipColor = PlanetColor.Enemy;
                GetComponent<SpriteRenderer>().color = _enemyColor;
                break;
            case PlanetColor.Friendly:
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
            SoundFx.instance.PlaySound(SoundFx.instance._hitSound, 0.1f, Random.Range(0.95f, 1.05f));

            if (_targetPlanet.planetColor != _shipColor)
            {
                ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
                Destroy(blast.gameObject, 1f);
            }
            Destroy(gameObject);
        }
    }
}
