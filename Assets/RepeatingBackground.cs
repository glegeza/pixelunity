using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    public Camera PixelCamera;
    public GameObject RepeatingTile;
    public int PixelsPerUnit;

    private Vector2 _worldSize;
    private int _screenPixelsY;
    private int _screenPixelsX;

	void Start ()
    {
        var pixelSize = RepeatingTile.GetComponent<SpriteRenderer>().sprite.rect;
        _worldSize = new Vector2(pixelSize.width / PixelsPerUnit, pixelSize.height / PixelsPerUnit);
        RecreateBackground();
	}
	
	void Update ()
    {
        if (_screenPixelsY != PixelCamera.pixelHeight || _screenPixelsX != PixelCamera.pixelWidth)
        {
            RecreateBackground();
        }
	}

    void RecreateBackground()
    {
        // Remove all existing tiles
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var cameraWidth = (float)PixelCamera.pixelWidth / PixelsPerUnit / 2.0f;
        var cameraHeight = (float)PixelCamera.pixelHeight / PixelsPerUnit / 2.0f;
        var startX = -cameraWidth + _worldSize.x / 2.0f;
        var startY = cameraHeight - _worldSize.y / 2.0f + (1.0f / PixelsPerUnit);
        var x = startX;
        var y = startY;
        do
        {
            GameObject tile = Instantiate(RepeatingTile, new Vector3(x, y, RepeatingTile.transform.position.z), RepeatingTile.transform.rotation) as GameObject;
            tile.transform.SetParent(transform);
            x += _worldSize.x;
            if (x - _worldSize.x / 2.0f > cameraWidth)
            {
                x = startX;
                y -= _worldSize.y;
            }
            if (y + _worldSize.y / 2.0f < -cameraHeight)
            {
                break;
            }
        } while (true);

        _screenPixelsX = PixelCamera.pixelWidth;
        _screenPixelsY = PixelCamera.pixelHeight;
    }
}
