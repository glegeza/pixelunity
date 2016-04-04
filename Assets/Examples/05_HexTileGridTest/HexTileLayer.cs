using UnityEngine;
using System.Collections.Generic;

public class HexTileLayer : MonoBehaviour
{
    public float XStart = 0.0f;
    public float YStart = 0.0f;
    public int GridWidth = 1;
    public int GridHeight = 1;
    public float TileWidth = 1.0f;
    public List<GameObject> Tiles = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
        LayTiles();
	}

    private void LayTiles()
    {
        var r = TileWidth * 0.5f;
        var s = r * 1.15470053838f;
        var h = s * 0.5f;
        var incr = s + h;
        Debug.LogFormat("s: {0}, h: {1}", s, h);
        for (var x = 0; x < GridWidth; x++)
        {
            var yPos = YStart;
            for (var y = 0; y < GridHeight; y++)
            {
                var xPos = x * TileWidth + XStart;
                if (y % 2 != 0)
                {
                    xPos += TileWidth * 0.5f;
                }
                var nextTile = Instantiate(GetNextTile()) as GameObject;
                nextTile.transform.position = new Vector3(
                    xPos, yPos, nextTile.transform.position.z);
                yPos += (y % 2 != 0) ? incr : incr;
            }
        }
    }

    private GameObject GetNextTile()
    {
        var idx = Random.Range(0, Tiles.Count);
        return Tiles[idx];
    }
}
