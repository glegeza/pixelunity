using UnityEngine;
using System.Collections.Generic;

public class TileDropper : MonoBehaviour
{
    public KeyCode DropKey;
    public KeyCode ResetKey;
    public GameObject TilePrefab;

    private PixelMouse _mouse;
    private List<GameObject> _tiles = new List<GameObject>();
    
	void Start ()
    {
        _mouse = FindObjectOfType<PixelMouse>();
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(DropKey))
        {
            DropAtLocation();
        }
        else if (Input.GetKeyDown(ResetKey))
        {
            RemoveAllTiles();
        }
	}

    private void DropAtLocation()
    {
        var newTile = Instantiate(TilePrefab) as GameObject;
        var location = _mouse.GetMouseWorldLocation();
        location.z = newTile.transform.position.z;
        newTile.transform.position = location;
        _tiles.Add(newTile);
    }

    private void RemoveAllTiles()
    {
        foreach (var tile in _tiles)
        {
            Destroy(tile);
        }
    }
}
