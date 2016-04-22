using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AsteroidKiller))]
public class ClickKiller : MonoBehaviour
{
    public List<GameObject> Shards = new List<GameObject>();
    public int MinShards = 2;
    public int MaxShards = 4;
    public float MinEjectionForce = 20.0f;
    public float MaxEjectionForce = 30.0f;

    private MouseObjectTracker _tracker;
    private AsteroidKiller _killer;
    private InfiniteSpawner _spawner;

	void Start ()
    {
        _tracker = GameObject.FindObjectOfType<MouseObjectTracker>();
        _killer = GetComponent<AsteroidKiller>();
        _spawner = GameObject.FindObjectOfType<InfiniteSpawner>();
        if (!_tracker)
        {
            enabled = false;
            return;
        }
        _tracker.ObjectClicked += OnObjectClicked;
	}

    private void OnObjectClicked(object sender, EventArgs e)
    {
        if (_tracker.CurrentObject != gameObject)
        {
            return;
        }
        _killer.MurderAsteroid();
        var shards = UnityEngine.Random.Range(MinShards, MaxShards);
        var angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
        for (var i = 0; i < shards; i++)
        {
            var nextShard = Instantiate(Shards[UnityEngine.Random.Range(0, Shards.Count)]) as GameObject;
            nextShard.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            nextShard.transform.Translate(direction * 1.0f);
            var rigidBody = nextShard.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(direction * UnityEngine.Random.Range(MinEjectionForce, MaxEjectionForce), ForceMode2D.Impulse);
            angle += Mathf.PI * 2.0f / shards;
            _spawner.AddTemporaryAsteroid(nextShard);
        }
    }
}
