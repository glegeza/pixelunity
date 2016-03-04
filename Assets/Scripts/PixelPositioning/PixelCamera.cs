using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PixelCamera : MonoBehaviour
{
    public int PixelsPerUnit = 32;
    public int ScalingValue = 1;
    public int TargetWidth = 800;
    public int TargetHeight = 600;

    private float _aspectRatio;

    public float UnitsPerPixel
    {
        get { return 1.0f / PixelsPerUnit; }
    }
    public float RightBound
    {
        get; private set;
    }
    public float BottomBound
    {
        get; private set;
    }
    public float OrthoWidth
    {
        get; private set;
    }
    public float OrthoHeight
    {
        get; private set;
    }

    private float _orthoSize;
    private Camera _attachedCamera;
    
    void Start()
    {
        _attachedCamera = GetComponent<Camera>();
        _orthoSize = (_attachedCamera.pixelHeight / (PixelsPerUnit * ScalingValue)) / 2.0f;
        UpdateOrthoSize(_orthoSize, _attachedCamera.pixelWidth, _attachedCamera.pixelHeight);
    }

    private void AdjustCamera()
    {
        int width = TargetWidth;
        int height = TargetHeight;
        _aspectRatio = (float)width / height;
        _attachedCamera.orthographicSize = ((float)width / PixelsPerUnit) / 2.0f;
        OrthoHeight = ((float)height / PixelsPerUnit) / 2.0f;
        OrthoWidth = _attachedCamera.orthographicSize;
    }

    private void OnValidate()
    {
        _attachedCamera = GetComponent<Camera>();
        _orthoSize = _attachedCamera.orthographicSize;
        UpdateOrthoSize(_orthoSize, _attachedCamera.pixelWidth, _attachedCamera.pixelHeight);
    }

    private void Update()
    {
        _attachedCamera = GetComponent<Camera>();
        _orthoSize = _attachedCamera.orthographicSize;
        UpdateOrthoSize(_orthoSize, _attachedCamera.pixelWidth, _attachedCamera.pixelHeight);
    }

    private void UpdateOrthoSize(float orthoHeight, float width, float height)
    {
        var ratio = width / height;
        var orthoWidth = orthoHeight * ratio;
        OrthoWidth = orthoWidth;
        OrthoHeight = orthoHeight;
        RightBound = orthoWidth * 2;
        BottomBound = orthoHeight * 2;
        _attachedCamera.orthographicSize = orthoHeight;
        _attachedCamera.transform.position = new Vector3(
            orthoWidth, orthoHeight, _attachedCamera.transform.position.z);
    }
}
