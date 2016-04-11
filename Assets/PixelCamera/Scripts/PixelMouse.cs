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
        // Clamp the mouse position to within the output quad's bounds
        var mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp((int)mousePos.x - _scaler.OutputOffsetX, 0, _scaler.OutputWidth - 1);
        mousePos.y = Mathf.Clamp((int)mousePos.y - _scaler.OutputOffsetY, 0, _scaler.OutputHeight - 1);

        // Scale the mouse position down to get its pixel coordinate over the render texture
        _rawMousePos.x = mousePos.x / _scaler.CurrentScale;
        _rawMousePos.y = mousePos.y / _scaler.CurrentScale;
        _rawMousePos.z = _camera.nearClipPlane;
        _screenMousePos.x = (int)(_rawMousePos.x);
        _screenMousePos.y = (int)(_rawMousePos.y);

        _worldMousePos = _camera.ScreenToWorldPoint(_screenMousePos);
        Debug.LogFormat("World mouse position {0}, {1}", _worldMousePos.x, _worldMousePos.y);
    }
}