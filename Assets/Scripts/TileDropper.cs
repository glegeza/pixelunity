using UnityEngine;
using System.Collections.Generic;

public class TileDropper : MonoBehaviour
{
    public KeyCode ResetKey;
    public List<GameObject> TilePrefabs;
    public int SelectedTile = 0;
    public bool RandomTile = true;

    private MouseObjectTracker _tracker;
    private PixelMouse _mouse;
    private List<GameObject> _tiles = new List<GameObject>();
    
	void Start ()
    {
        _tracker = FindObjectOfType<MouseObjectTracker>();
        _mouse = FindObjectOfType<PixelMouse>();
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && _tracker.CurrentObject == null)
        {
            DropAtLocation();
        }
        else if (Input.GetMouseButtonDown(1) && _tracker.CurrentObject != null && _tracker.CurrentObject.GetComponent<HoverColorizer>() != null)
        {
            Destroy(_tracker.CurrentObject);
        }
        else if (Input.GetKeyDown(ResetKey))
        {
            RemoveAllTiles();
        }
	}

    private void DropAtLocation()
    {
        var selectedTile = SelectedTile;
        if (RandomTile)
        {
            selectedTile = Random.Range(0, TilePrefabs.Count);
        }
        var newTile = Instantiate(TilePrefabs[selectedTile]) as GameObject;
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
