using UnityEngine;

public class AsteroidKiller : MonoBehaviour
{
    public float MinEjectionForce = 20.0f;
    public float MaxEjectionForce = 30.0f;

    private InfiniteSpawner _spawner;
    
	void Start ()
    {        
        _spawner = GameObject.FindObjectOfType<InfiniteSpawner>();
        if (!_spawner)
        {
            enabled = false;
            return;
        }
	}

    public void MurderAsteroid()
    {
        gameObject.SetActive(false);
        var data = GetComponent<AsteroidData>();
        if (!data)
        {
            return;
        }
        if (data.AsteroidSizeType.ChunkSize == null)
        {
            return;
        }
        var shards = UnityEngine.Random.Range(data.AsteroidSizeType.MinChunks, data.AsteroidSizeType.MaxChunks);
        var angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
        var force = UnityEngine.Random.Range(MinEjectionForce, MaxEjectionForce);
        for (var i = 0; i < shards; i++)
        {
            var nextShard = _spawner.AddTemporaryAsteroid(data.AsteroidSizeType.ChunkSize, data.AsteroidType, null) as GameObject;
            nextShard.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            nextShard.transform.Translate(direction * 1.0f);
            var startAngle = UnityEngine.Random.Range(0.0f, 360.0f);
            nextShard.transform.rotation = Quaternion.Euler(0.0f, 0.0f, startAngle);
            var rigidBody = nextShard.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(direction * force, ForceMode2D.Impulse);
            angle += Mathf.PI * 2.0f / shards;
            _spawner.AddTemporaryAsteroid(nextShard);
        }
    }
}
