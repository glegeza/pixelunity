using UnityEngine;
using System.Collections.Generic;

public class InfiniteSpawner : MonoBehaviour
{

    public float CullTime = 1.0f;
    public float CullDistance = 25.0f;
    public float SpawnCone = 30.0f;
    public int PoolSize = 30;
    public List<GameObject> AsteroidTypes = new List<GameObject>();
    public Transform Player;

    private List<GameObject> _asteroidList = new List<GameObject>();
    private int _activeAsteroids = 0;
    private int _inactiveAsteroids = 0;
    private float _timeSinceCull = 0.0f;

	private void Start ()
    {
        GeneratePool();
        PlaceAsteroids();
	}
	
	private void Update ()
    {
        _timeSinceCull += Time.deltaTime;
        if (_timeSinceCull > CullTime)
        {
            CullAsteroids();
            PlaceAsteroids();
        }
	}

    private void CullAsteroids()
    {
        var culled = 0;
        for (var i = 0; i < _activeAsteroids; i++)
        {
            var asteroid = _asteroidList[i];
            if (Vector2.Distance(asteroid.transform.position, Player.transform.position) > CullDistance)
            {
                culled += 1;
                _asteroidList[PoolSize + _inactiveAsteroids] = asteroid;
                asteroid.SetActive(false);
                _inactiveAsteroids += 1;
            }
        }
        _activeAsteroids -= culled;
    }

    private void GeneratePool()
    {
        _asteroidList = new List<GameObject>(PoolSize * 2);
        _activeAsteroids = 0;
        for (var i = 0; i < PoolSize; i++)
        {
            var asteroid = Instantiate(AsteroidTypes[Random.Range(0, AsteroidTypes.Count)]);
            asteroid.SetActive(false);
            _asteroidList.Add(asteroid);
        }
        _inactiveAsteroids = PoolSize;
    }

    private void PlaceAsteroids()
    {
        while (_activeAsteroids != PoolSize)
        {
            PlaceNextAsteroid();
        }
    }

    private void PlaceNextAsteroid()
    {
        var nextAsteroid = _asteroidList[_activeAsteroids];
        nextAsteroid.SetActive(true);
        _activeAsteroids += 1;
    }
}
