using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(PixelCameraScaler))]
public class PixelMouse : MonoBehaviour
{
    private PixelCameraScaler _scaler;
    private Camera _camera;

    public Vector3 GetMouseWorldLocation()
    {
        var mousePos = GetFloatScreenPos();
        return _camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _camera.nearClipPlane));
    }

    public Vector2 GetMouseScreenLocation()
    {
        var floatPos = GetFloatScreenPos();
        floatPos.x = (int)floatPos.x;
        floatPos.y = (int)floatPos.y;
        return floatPos;
    }

	private void Start ()
    {
        _scaler = GetComponent<PixelCameraScaler>();
        _camera = GetComponent<Camera>();
	}

    private Vector2 GetFloatScreenPos()
    {
        var pixelsPerUnitScreen = _scaler.OutputCamera.orthographicSize * 2 / Screen.height;
        var quadPixelWidth = _scaler.OutputQuad.localScale.x / pixelsPerUnitScreen;
        var quadPixelHeight = _scaler.OutputQuad.localScale.y / pixelsPerUnitScreen;
        var xOffset = (Screen.width - quadPixelWidth) / 2.0f;
        var yOffset = (Screen.height - quadPixelHeight) / 2.0f;

        var mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp(((mousePos.x - xOffset) / _scaler.CurrentScale), 0.0f, quadPixelWidth);
        mousePos.y = Mathf.Clamp(((mousePos.y - yOffset) / _scaler.CurrentScale), 0.0f, quadPixelHeight);
        return mousePos;
    }
}
