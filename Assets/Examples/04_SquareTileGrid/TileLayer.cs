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
    public PixelCameraScaler _camera;
    
	void Start ()
    {
        _camera = FindObjectOfType<PixelCameraScaler>();
        LayTiles();
	}

    private void LayTiles()
    {
        var xStart = XStart;
        var yStart = YStart;
        var xAdvance = TileWidth;
        var yAdvance = TileHeight;
        //var xStart = _camera ? _camera.FloorToPixelBoundary(XStart) : XStart;
        //var yStart = _camera ? _camera.FloorToPixelBoundary(YStart) : YStart;
        //var xAdvance = _camera ? _camera.FloorToPixelBoundary(TileWidth) : TileWidth;
        //var yAdvance = _camera ? _camera.FloorToPixelBoundary(TileHeight) : TileHeight;
        for (var x = 0; x < GridWidth; x++)
        {
            for (var y = 0; y < GridHeight; y++)
            {
                var xPos = x * xAdvance + xStart;
                var yPos = y * yAdvance + yStart;
                var nextTile = Instantiate(GetNextTile()) as GameObject;
                nextTile.transform.position = new Vector3(
                    (int)xPos, (int)yPos, nextTile.transform.position.z);
            }
        }
    }

    private GameObject GetNextTile()
    {
        var idx = Random.Range(0, Tiles.Count);
        return Tiles[idx];
    }
}
