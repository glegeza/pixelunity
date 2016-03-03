using UnityEngine;

[ExecuteInEditMode]
public class PixelTransform : MonoBehaviour
{
    public OrthoSizeAdjustment AttachedCamera;
    public bool LockToGrid = true;
    public int X;
    public int Y;

    private OrthoSizeAdjustment _attachedCamera;
    private Vector3 _savedPosition;
	
    public void ForceAlign()
    {
        transform.position = new Vector3(X * _attachedCamera.UnitsPerPixel, Y * _attachedCamera.UnitsPerPixel, transform.position.z);
        transform.hasChanged = false;
    }

    void Start()
    {
        _attachedCamera = AttachedCamera ?? FindObjectOfType<OrthoSizeAdjustment>();
    }

    void OnValidate()
    {
        // Attempt to grab an attached camera
        _attachedCamera = AttachedCamera ?? FindObjectOfType<OrthoSizeAdjustment>();

        // If still no valid camera, can't do anything
        if (!_attachedCamera)
        {
            return;
        }
        transform.position = new Vector3(X * _attachedCamera.UnitsPerPixel, Y * _attachedCamera.UnitsPerPixel, transform.position.z);
    }

    void Update()
    {
        if (Application.isPlaying || !transform.hasChanged || !_attachedCamera)
        {
            return;
        }
        X = (int)(transform.position.x / _attachedCamera.UnitsPerPixel);
        Y = (int)(transform.position.y / _attachedCamera.UnitsPerPixel);
        transform.position = new Vector3(X * _attachedCamera.UnitsPerPixel, Y * _attachedCamera.UnitsPerPixel, transform.position.z);
        transform.hasChanged = false;
    }

    void LateUpdate()
    {
        if (!Application.isPlaying || !_attachedCamera)
        {
            return;
        }

        _savedPosition = transform.position;
        X = (int)(transform.position.x / _attachedCamera.UnitsPerPixel);
        Y = (int)(transform.position.y / _attachedCamera.UnitsPerPixel);
        transform.position = new Vector3(X * _attachedCamera.UnitsPerPixel, Y * _attachedCamera.UnitsPerPixel, transform.position.z);
        transform.hasChanged = false;
    }

    void OnRenderObject()
    {
        if (!Application.isPlaying || !_attachedCamera)
        {
            return;
        }
        transform.position = _savedPosition;
    }
}