using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(PixelCameraScaler))]
public class PixelMouse : MonoBehaviour
{
    private PixelCameraScaler _scaler;
    private Camera _camera;

    public Vector3 GetMouseWorldLocation()
    {
        var screenPos = GetMouseScreenLocation();
        return _camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _camera.nearClipPlane));
    }

    public Vector2 GetMouseScreenLocation()
    {
        var pixelsPerUnitScreen = _scaler.OutputCamera.orthographicSize * 2 / Screen.height;
        var quadPixelWidth = _scaler.OutputQuad.localScale.x / pixelsPerUnitScreen;
        var quadPixelHeight = _scaler.OutputQuad.localScale.y / pixelsPerUnitScreen;
        var xOffset = (Screen.width - quadPixelWidth) / 2.0f;
        var yOffset = (Screen.height - quadPixelHeight) / 2.0f;

        var mousePos = Input.mousePosition;
        mousePos.x = (int)Mathf.Clamp(((mousePos.x - xOffset) / _scaler.CurrentScale), 0.0f, quadPixelWidth);
        mousePos.y = (int)Mathf.Clamp(((mousePos.y - yOffset) / _scaler.CurrentScale), 0.0f, quadPixelHeight);
        return mousePos;
    }

	private void Start ()
    {
        _scaler = GetComponent<PixelCameraScaler>();
        _camera = GetComponent<Camera>();
	}
}
