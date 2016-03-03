using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PixelGrid : MonoBehaviour
{
    public bool EnableGrid = true;
    public float GridZ = 0.0f;
    public float MinDrawDistance = 15.0f;
    public Color GridColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);
    
    private float _verticalLines = 0.0f;
    private float _verticalEnd = 0.0f;
    private float _horizontalLines = 0.0f;
    private float _horizontalEnd = 0.0f;
    private Vector2 _bottomLeft = new Vector2(0, 0);
    
    void OnDrawGizmos()
    {
        if (!EnableGrid)
        {
            // If we don't have a camera then nothing has been calculated yet
            return;
        }
        OrthoSizeAdjustment grid = GetComponent<OrthoSizeAdjustment>();
        Camera camera = GetComponent<Camera>();
        _bottomLeft = new Vector2(
            camera.transform.position.x - grid.OrthoWidth,
            camera.transform.position.y - grid.OrthoHeight);
        _verticalLines = grid.OrthoWidth * 2 / grid.UnitsPerPixel;
        _horizontalLines = grid.OrthoHeight * 2 / grid.UnitsPerPixel;
        _horizontalEnd = camera.transform.position.x + grid.OrthoWidth;
        _verticalEnd = camera.transform.position.y + grid.OrthoHeight;

        float xStart = _bottomLeft.x;
        float yStart = _bottomLeft.y;
        Gizmos.color = GridColor;
        for (var x = 0; x < _verticalLines; x++)
        {
            var curX = xStart + x * grid.UnitsPerPixel;
            var curY = yStart;
            Gizmos.DrawLine(new Vector3(curX, curY, GridZ), new Vector3(curX, curY + _verticalEnd, GridZ));
        }
        for (var y = 0; y < _horizontalLines; y++)
        {
            var curX = xStart;
            var curY = yStart + y * grid.UnitsPerPixel;
            Gizmos.DrawLine(new Vector3(curX, curY, GridZ), new Vector3(curX + _horizontalEnd, curY, GridZ));
        }
    }
}
