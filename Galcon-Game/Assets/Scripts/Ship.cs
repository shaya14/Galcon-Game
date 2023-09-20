using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Planet _targetPlanet;
    [SerializeField] ParticleSystem _BlastParticlePrefab;
    private PlanetColor _shipColor;
    public PlanetColor shipColor => _shipColor;
    [SerializeField] private Color _playerColor;
    [SerializeField] private Color _enemyColor;

    public void Init(PlanetColor color, Planet target) {
        _shipColor = color;

        switch (_shipColor) {
        case PlanetColor.Enemy:
            GetComponent<SpriteRenderer>().color = _enemyColor;
            break;
        case PlanetColor.Friendly:
            GetComponent<SpriteRenderer>().color = _playerColor;
            break;
        }

        _targetPlanet = target;
    }

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
        if (collision.gameObject.GetInstanceID() == _targetPlanet.gameObject.GetInstanceID())
        {
            _targetPlanet.Hit(this);
            SoundFx.Instance.PlaySound(SoundFx.Instance.hitSound, 0.1f, Random.Range(0.95f, 1.05f));

            if (_targetPlanet.planetColor != _shipColor)
            {
                ParticleSystem blast = Instantiate(_BlastParticlePrefab, transform.position, Quaternion.identity);
                Destroy(blast.gameObject, 1f);
            }
            Destroy(gameObject);
        }
    }
}
