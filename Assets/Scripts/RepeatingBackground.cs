using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    public int Width;
    public int Height;
    public GameObject RepeatingTile;
    public int PixelsPerUnit;

    private Vector2 _worldSize;

	void Start ()
    {
        var pixelSize = RepeatingTile.GetComponent<SpriteRenderer>().sprite.rect;
        _worldSize = new Vector2(pixelSize.width / PixelsPerUnit, pixelSize.height / PixelsPerUnit);
        RecreateBackground();
	}

    void RecreateBackground()
    {        
        var startX = -(Width * _worldSize.x) / 2.0f + _worldSize.x / 2.0f;
        var startY = (Height * _worldSize.y) / 2.0f - _worldSize.y / 2.0f;
        var x = startX;
        var y = startY;
        do
        {
            GameObject tile = Instantiate(RepeatingTile, new Vector3(x, y, RepeatingTile.transform.position.z), RepeatingTile.transform.rotation) as GameObject;
            tile.transform.SetParent(transform);
            x += _worldSize.x;
            if (x - _worldSize.x / 2.0f > (Width * _worldSize.x) / 2.0f)
            {
                x = startX;
                y -= _worldSize.y;
            }
            if (y + _worldSize.y / 2.0f < -(Height * _worldSize.y) / 2.0f)
            {
                break;
            }
        } while (true);
    }
}
