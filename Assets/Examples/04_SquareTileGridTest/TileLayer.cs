using UnityEngine;
using System.Collections.Generic;

public class TileLayer : MonoBehaviour
{
    public float XStart = 0.0f;
    public float YStart = 0.0f;
    public int GridWidth = 1;
    public int GridHeight = 1;
    public float TileWidth = 1.0f;
    public float TileHeight = 1.0f;
    public List<GameObject> Tiles = new List<GameObject>();
    
	void Start ()
    {
        LayTiles();
	}

    private void LayTiles()
    {
        for (var x = XStart; x < XStart + GridWidth; x++)
        {
            for (var y = YStart; y < YStart + GridHeight; y++)
            {
                var xPos = x * TileWidth;
                var yPos = y * TileHeight;
                var nextTile = Instantiate(GetNextTile()) as GameObject;
                nextTile.transform.position = new Vector3(
                    xPos, yPos, nextTile.transform.position.z);
            }
        }
    }

    private GameObject GetNextTile()
    {
        var idx = Random.Range(0, Tiles.Count);
        return Tiles[idx];
    }
}
