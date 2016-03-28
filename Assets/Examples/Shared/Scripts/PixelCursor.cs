using UnityEngine;

public class PixelCursor : MonoBehaviour
{
    private PixelMouse _mouse;
    private PixelCameraScaler _scaler;
    
	void Start ()
    {
        _mouse = FindObjectOfType<PixelMouse>();
        _scaler = FindObjectOfType<PixelCameraScaler>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
	}
	
	void Update ()
    {
        if (!(_mouse && _scaler))
        {
            return;
        }
        var screenPos = _mouse.GetMouseScreenLocation();
        var worldPos = _mouse.GetMouseWorldLocation();
        var roundedPos = _scaler.RoundToPixelBoundary(worldPos);
        transform.position = new Vector3
            (roundedPos.x, roundedPos.y, transform.position.z);
        Debug.LogFormat("Mouse Position {0}, {1}", screenPos.x, screenPos.y);
	}
}
