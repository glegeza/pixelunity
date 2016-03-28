using UnityEngine;
using System.Collections.Generic;

public class TileDropper : MonoBehaviour
{
    private List<GameObject> _tiles = new List<GameObject>();

    public void DropAtLocation(Vector3 location, GameObject tilePrefab, bool isKinematic = false)
    {
        var newTile = Instantiate(tilePrefab) as GameObject;
        newTile.GetComponent<Rigidbody2D>().isKinematic = isKinematic;
        location.z = newTile.transform.position.z;
        newTile.transform.position = location;
        _tiles.Add(newTile);
    }

    public void RemoveAllTiles()
    {
        foreach (var tile in _tiles)
        {
            Destroy(tile);
        }
    }
}
