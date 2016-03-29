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
        var worldPos = _mouse.GetMouseWorldLocation();
        transform.position = new Vector3
            (worldPos.x, worldPos.y, transform.position.z);
	}
}
