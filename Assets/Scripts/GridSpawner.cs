using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject[] GridSprites;

    public int TileSize;

	void Start ()
    {
        int xSize = Camera.main.pixelWidth / TileSize + 1;
        int ySize = Camera.main.pixelHeight / TileSize + 1;

        for (var x = 0; x < xSize; x++)
        {
            for (var y = ySize; y > 0; y--)
            {
                int idx = Random.Range(0, GridSprites.Length);
                GameObject tile = Instantiate(GridSprites[idx], GridSprites[idx].transform.position, Quaternion.identity) as GameObject;
                PixelTransform pTransform = tile.GetComponent<PixelTransform>();
                pTransform.X = x * TileSize;
                pTransform.Y = y * TileSize;
                pTransform.ForceAlign();
            }
        }
	}
}