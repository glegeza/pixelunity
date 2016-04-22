using UnityEngine;

public class AsteroidKiller : MonoBehaviour
{
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
    }
}
