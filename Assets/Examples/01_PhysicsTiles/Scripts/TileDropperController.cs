using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TileDropper))]
public class TileDropperController : MonoBehaviour
{
    public KeyCode ResetKey;
    public KeyCode KinematicKey;
    public List<GameObject> TilePrefabs;
    public int SelectedTile = 0;
    public bool RandomTile = true;
    public bool KinematicTiles = false;

    private MouseObjectTracker _tracker;
    private PixelMouse _mouse;
    private TileDropper _dropper;

    void Start ()
    {
        _tracker = FindObjectOfType<MouseObjectTracker>();
        _mouse = FindObjectOfType<PixelMouse>();
        _dropper = GetComponent<TileDropper>();
	}
	
	void Update ()
    {
        if (Input.GetMouseButton(0) && _tracker.CurrentObject == null)
        {
            SpawnObject();
        }
        else if (Input.GetMouseButtonDown(1) && _tracker.CurrentObject)
        {
            KillObject(_tracker.CurrentObject);
        }

        if (Input.GetKeyDown(ResetKey))
        {
            _dropper.RemoveAllTiles();
        }
        else if (Input.GetKeyDown(KinematicKey))
        {
            KinematicTiles = !KinematicTiles;
        }
    }

    void SpawnObject()
    {
        var selectedTile = RandomTile ? Random.Range(0, TilePrefabs.Count) : SelectedTile;
        _dropper.DropAtLocation(_mouse.GetMouseWorldLocation(), TilePrefabs[selectedTile], KinematicTiles);
    }

    void KillObject(GameObject obj)
    {
        if (obj.GetComponent<HoverColorizer>() != null)
        {
            Destroy(obj);
        }
    }
}
