using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private int poolSize;           // Number of ships in the pool
    private static ObjectPool _instance;
    public static ObjectPool Instance { get { return _instance; } }
    private Queue<Ship> shipPool = new Queue<Ship>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        // Initialize the ship pool
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Ship ship = Instantiate(PlanetManager.Instance._attackingShipPrefab, Vector3.zero, Quaternion.identity);
            ship.gameObject.SetActive(false);
            shipPool.Enqueue(ship);
        }
    }

    public Ship GetShipFromPool()
    {
        if (shipPool.Count > 0)
        {
            return shipPool.Dequeue();
        }
        return null;
    }
    public void ReturnShipToPool(Ship ship)
    {
        ship.gameObject.SetActive(false);
        shipPool.Enqueue(ship);
    }
}

