using UnityEngine;

public class ObjectPixelSnap : MonoBehaviour
{
    private Vector3 _savedTransform = new Vector3();
    private Vector3 _snappedPosition = new Vector3();
    private PixelCameraScaler _pixelCamera;

	// Use this for initialization
	private void Start ()
    {
        _pixelCamera = FindObjectOfType<PixelCameraScaler>();
        if (!_pixelCamera)
        {
            enabled = false;
        }
	}

    private void LateUpdate()
    {
        _savedTransform = transform.position;
        SnapToPixel();
    }

    private void SnapToPixel()
    {
        _snappedPosition.x = _pixelCamera.FloorToPixelBoundary(_savedTransform.x);
        _snappedPosition.y = _pixelCamera.FloorToPixelBoundary(_savedTransform.y);
        _snappedPosition.z = _savedTransform.z;
        transform.position = _snappedPosition;
    }

    private void OnRenderObject()
    {
        transform.position = _savedTransform;
    }
}