using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class InfiniteSpawner : MonoBehaviour
{
    public class AsteroidSizeDescription
    {
        public AsteroidSizeDescription(string str)
        {
            SizeString = str;
        }

        public string SizeString;
        public float Mass;
        public float LinearDrag;
        public float AngularDrag;
    }

    public class AsteroidShapeDescription
    {
        public AsteroidShapeDescription(string str)
        {
            ShapeString = str;
        }

        public string ShapeString;
    }

    public class AsteroidTypeDescription
    {
        public AsteroidTypeDescription(string str)
        {
            TypeString = str;
        }

        public string TypeString;

        public string GetSpriteName(AsteroidSizeDescription size, AsteroidShapeDescription shape)
        {
            return String.Format("asteroid_{0}_{1}_{2}", size.SizeString, shape.ShapeString, TypeString);
        }

        public string GetPrefabName(AsteroidSizeDescription size, AsteroidShapeDescription shape)
        {
            return String.Format("asteroid_{0}_{1}", size.SizeString, shape.ShapeString);
        }
    }

    public string AsteroidPrefix = "asteroid";
    public float CullTime = 1.0f;
    public float PopulateMin = 2.0f;
    public float MinSpawnDistance = 35.0f;
    public float MaxSpawnDistance = 60.0f;
    public int PoolSize = 120;
    public Transform Player;

    private List<AsteroidSizeDescription> _sizes = new List<AsteroidSizeDescription>();
    private List<AsteroidShapeDescription> _shapes = new List<AsteroidShapeDescription>();
    private List<AsteroidTypeDescription> _types = new List<AsteroidTypeDescription>();

    private List<GameObject> _asteroids = new List<GameObject>();
    private List<GameObject> _tempAsteroids = new List<GameObject>(); // asteroids that shouldn't be respawned after culling
    private List<Sprite> _asteroidSprites = new List<Sprite>();
    private List<GameObject> _asteroidPrefabs = new List<GameObject>();
    private float _timeSinceCull = 0.0f;
    private float _distanceSqr;

	private void Start ()
    {
        var sprites = Resources.LoadAll<Sprite>("sprites");
        foreach (Sprite sprite in sprites)
        {
            print(sprite.name);
        }
        _asteroidSprites.AddRange(sprites.
            Where(s => s.name.Contains(AsteroidPrefix)));
        print(_asteroidSprites.Count);
        var prefabs = Resources.LoadAll<GameObject>("prefabs");
        _asteroidPrefabs.AddRange(prefabs.
            Where(p => p.name.Contains(AsteroidPrefix)));

        _sizes.Add(new AsteroidSizeDescription("tiny"));
        _sizes.Add(new AsteroidSizeDescription("small"));
        _sizes.Add(new AsteroidSizeDescription("large"));
        _sizes.Add(new AsteroidSizeDescription("huge"));

        _shapes.Add(new AsteroidShapeDescription("round"));
        _shapes.Add(new AsteroidShapeDescription("oblong"));
        //_shapes.Add(new AsteroidShapeDescription("jagged"));

        _types.Add(new AsteroidTypeDescription("blueore"));
        _types.Add(new AsteroidTypeDescription("blueveins"));
        _types.Add(new AsteroidTypeDescription("brown"));
        _types.Add(new AsteroidTypeDescription("brownore"));
        _types.Add(new AsteroidTypeDescription("darkblue"));
        _types.Add(new AsteroidTypeDescription("darkgray"));
        _types.Add(new AsteroidTypeDescription("gray"));
        _types.Add(new AsteroidTypeDescription("grayveins"));
        _types.Add(new AsteroidTypeDescription("lavaveins"));
        _types.Add(new AsteroidTypeDescription("lightblue"));
        _types.Add(new AsteroidTypeDescription("lightblueore"));
        _types.Add(new AsteroidTypeDescription("lightgray"));

        _distanceSqr = Mathf.Pow(MaxSpawnDistance, 2.0f);

        GeneratePool();
        PlaceAsteroids(true);
	}

    public void AddTemporaryAsteroid(GameObject newAsteroid)
    {
        _tempAsteroids.Add(newAsteroid);
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
            if (Vector2.SqrMagnitude(roid.transform.position - Player.position) > _distanceSqr)
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
            var size = _sizes[UnityEngine.Random.Range(0, _sizes.Count)];
            var type = _types[UnityEngine.Random.Range(0, _types.Count)];
            var shape = _shapes[UnityEngine.Random.Range(0, _shapes.Count)];
            var prefab_name = type.GetPrefabName(size, shape);
            var sprite_name = type.GetSpriteName(size, shape);
            var prefab = _asteroidPrefabs.Find(p => p.name == prefab_name);
            var sprite = _asteroidSprites.Find(s => s.name == sprite_name);
            var newRoid = Instantiate(prefab) as GameObject;
            var spriteRenderer = newRoid.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
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
            var angle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI) + playerAngle;
            var position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * UnityEngine.Random.Range(min, MaxSpawnDistance);
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
        var dirVec = UnityEngine.Random.insideUnitCircle.normalized;
        var startAngle = UnityEngine.Random.Range(0.0f, 360.0f);
        asteroid.SetActive(true);
        asteroid.GetComponent<Rigidbody2D>().AddForce(dirVec * UnityEngine.Random.Range(1.0f, 5.0f), ForceMode2D.Impulse);
        asteroid.transform.rotation = Quaternion.Euler(0.0f, 0.0f, startAngle);
    }
}
