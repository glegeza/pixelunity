using UnityEngine;
using System.Collections.Generic;
using System;

public class HexTileLayer : MonoBehaviour
{
    public float XStart = 0.0f;
    public float YStart = 0.0f;
    public int GridWidth = 1;
    public int GridHeight = 1;
    public float TileWidth = 1.0f;
    public int PixelPadding = 2;
    public List<GameObject> Tiles = new List<GameObject>();

    private PixelCameraScaler _scaler;

    // Use this for initialization
    void Start ()
    {
        _scaler = FindObjectOfType<PixelCameraScaler>();
        LayTiles();
	}

    private void LayTiles()
    {
        var tileAdvance = (int)(TileWidth * _scaler.PixelsPerUnit) / (float)_scaler.PixelsPerUnit;
        var r = tileAdvance * 0.5f;
        var s = r * 1.15470053838f; // s = r * Sec(30*)
        var h = s * 0.5f; // h = s * Sin(30*)
        var incr = s + h;
        var paddingAdvance = PixelPadding * (1.0f / _scaler.PixelsPerUnit);
        Debug.LogFormat("s: {0}, h: {1}", s, h);
        for (var x = 0; x < GridWidth; x++)
        {
            var yPos = YStart;
            for (var y = 0; y < GridHeight; y++)
            {
                var xPos = x * (tileAdvance + paddingAdvance) + XStart;
                if (y % 2 != 0)
                {
                    xPos += tileAdvance * 0.5f + paddingAdvance * 0.5f;
                }
                var nextTile = Instantiate(GetNextTile()) as GameObject;
                nextTile.name = String.Format("Hex Tile {0}, {1}", x, y);
                nextTile.transform.position = new Vector3(
                    xPos, yPos, nextTile.transform.position.z);
                yPos += (y % 2 != 0) ? incr : incr;
                yPos += paddingAdvance;
            }
        }
    }

    private GameObject GetNextTile()
    {
        var idx = UnityEngine.Random.Range(0, Tiles.Count);
        return Tiles[idx];
    }
}
