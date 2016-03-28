using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(PixelCameraScaler))]
public class PixelMouse : MonoBehaviour
{
    private PixelCameraScaler _scaler;
    private Camera _camera;

    private Vector3 _rawMousePos = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector2 _screenMousePos = new Vector2(0.0f, 0.0f);
    private Vector3 _worldMousePos = new Vector3(0.0f, 0.0f, 0.0f);

    public Vector3 GetMouseWorldLocation()
    {
        return _worldMousePos;
    }

    public Vector2 GetMouseScreenLocation()
    {
        return _screenMousePos;
    }

	private void Start ()
    {
        _scaler = GetComponent<PixelCameraScaler>();
        _camera = GetComponent<Camera>();
	}

    private void Update()
    {
        UpdateMousePos();
    }

    private void UpdateMousePos()
    {
        var pixelsPerUnitScreen = _scaler.OutputCamera.orthographicSize * 2 / Screen.height;
        var quadPixelWidth = _scaler.OutputQuad.localScale.x / pixelsPerUnitScreen;
        var quadPixelHeight = _scaler.OutputQuad.localScale.y / pixelsPerUnitScreen;
        var xOffset = (Screen.width - quadPixelWidth) / 2.0f;
        var yOffset = (Screen.height - quadPixelHeight) / 2.0f;

        var mousePos = Input.mousePosition;
        _rawMousePos.x = Mathf.Clamp(((mousePos.x - xOffset) / _scaler.CurrentScale), 0.0f, quadPixelWidth);
        _rawMousePos.y = Mathf.Clamp(((mousePos.y - yOffset) / _scaler.CurrentScale), 0.0f, quadPixelHeight);
        _rawMousePos.z = _camera.nearClipPlane;
        _screenMousePos.x = (int)_rawMousePos.x;
        _screenMousePos.y = (int)_rawMousePos.y - 1;
        _worldMousePos = _camera.ScreenToWorldPoint(_rawMousePos);
    }
}