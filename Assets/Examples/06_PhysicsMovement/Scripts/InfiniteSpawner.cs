using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InfiniteSpawner : MonoBehaviour
{
    public float CullTime = 1.0f;
    public float PopulateMin = 2.0f;
    public float MinSpawnDistance = 35.0f;
    public float MaxSpawnDistance = 60.0f;
    public int PoolSize = 120;
    public List<GameObject> AsteroidTypes = new List<GameObject>();
    public Transform Player;

    private List<GameObject> _asteroids = new List<GameObject>();
    private List<GameObject> _tempAsteroids = new List<GameObject>(); // asteroids that shouldn't be respawned after culling
    private float _timeSinceCull = 0.0f;

    public void AddTemporaryAsteroid(GameObject newAsteroid)
    {
        _tempAsteroids.Add(newAsteroid);
    }

	private void Start ()
    {
        GeneratePool();
        PlaceAsteroids(true);
	}
	
	private void Update ()
    {
        _timeSinceCull += Time.deltaTime;
        if (_timeSinceCull > CullTime)
        {
            CullAsteroids();
            PlaceAsteroids();
            _timeSinceCull = 0.0f;
        }
	}

    private void CullAsteroids()
    {
        foreach (var roid in _asteroids.Where(a => a.activeSelf))
        {
            if (Vector2.Distance(Player.position, roid.transform.position) > MaxSpawnDistance)
            {
                Debug.Log("Clearing asteroid");
                roid.SetActive(false);
                break;
            }
        }
        _tempAsteroids.RemoveAll(a => Vector2.Distance(Player.position, a.transform.position) < MaxSpawnDistance);
    }

    private void GeneratePool()
    {
        _asteroids.Clear();
        for (var i = 0; i < PoolSize; i++)
        {
            var newRoid = Instantiate(AsteroidTypes[Random.Range(0, AsteroidTypes.Count)]);
            newRoid.SetActive(false);
            _asteroids.Add(newRoid);
        }
    }

    private void PlaceAsteroids(bool populate=false)
    {
        foreach (var roid in _asteroids.Where(a => !a.activeSelf))
        {
            PlaceAsteroid(roid, populate);
        }
    }

    private void PlaceAsteroid(GameObject asteroid, bool populate)
    {
        var foundPlacement = false;
        var min = populate ? PopulateMin : MinSpawnDistance;
        var asteroidRadius = asteroid.GetComponent<ExclusionRadius>();
        do
        {
            var playerAngle = Mathf.Atan2(Player.transform.up.y, Player.transform.up.x);
            var angle = Random.Range(-Mathf.PI, Mathf.PI) + playerAngle;
            var position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(min, MaxSpawnDistance);
            asteroid.transform.position = new Vector3(position.x + Player.position.x, position.y + Player.position.y, asteroid.transform.position.z);
            var collider = asteroid.GetComponent<Collider2D>();
            foundPlacement = true;
            foreach (var otherAsteroid in _asteroids)
            {
                if (asteroid == otherAsteroid)
                {
                    continue;
                }
                var otherAsteroidRadius = otherAsteroid.GetComponent<ExclusionRadius>();
                if (Vector2.Distance(asteroid.transform.position, otherAsteroid.transform.position) < otherAsteroidRadius.Radius + asteroidRadius.Radius)
                {
                    foundPlacement = false;
                    break;
                }
            }
        } while (!foundPlacement);
        var dirVec = Random.insideUnitCircle.normalized;
        asteroid.SetActive(true);
        asteroid.GetComponent<Rigidbody2D>().AddForce(dirVec * Random.Range(1.0f, 5.0f), ForceMode2D.Impulse);
    }
}
