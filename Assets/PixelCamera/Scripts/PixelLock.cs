using UnityEngine;

[RequireComponent(typeof(PixelCameraScaler))]
[RequireComponent(typeof(Camera))]
public class PixelLock : MonoBehaviour
{
    [Tooltip("Snaps the camera's position to pixel boundaries.")]
    public bool PixelSnap = true;

    private PixelCameraScaler _cameraScaler;
    private Vector3 _savedTransform = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 _tempTransform = new Vector3(0.0f, 0.0f, 0.0f);
    
	void Start ()
    {
        _cameraScaler = GetComponent<PixelCameraScaler>();
	}

    void OnPreRender()
    {
        if (!(PixelSnap && _cameraScaler))
        {
            return;
        }
        var pixelsPerUnit = _cameraScaler.PixelsPerUnit;
        _savedTransform = transform.position;
        _tempTransform.x = Mathf.RoundToInt(transform.position.x * pixelsPerUnit) / (float)pixelsPerUnit;
        _tempTransform.y = Mathf.RoundToInt(transform.position.y * pixelsPerUnit) / (float)pixelsPerUnit;
        _tempTransform.z = transform.position.z;
        transform.position = _tempTransform;
    }

    void OnPostRender()
    {
        if (!(PixelSnap && _cameraScaler))
        {
            return;
        }
        transform.position = _savedTransform;
    }
}
