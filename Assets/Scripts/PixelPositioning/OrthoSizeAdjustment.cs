using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class OrthoSizeAdjustment : MonoBehaviour
{

    public float PixelsPerUnit = 32.0f;
    public int ScalingValue = 1;

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

    private float orthoSize;
    private Camera attachedCamera;
    
    void Start()
    {
        attachedCamera = GetComponent<Camera>();
        orthoSize = (attachedCamera.pixelHeight / (PixelsPerUnit * ScalingValue)) / 2.0f;
        UpdateOrthoSize(orthoSize, attachedCamera.pixelWidth, attachedCamera.pixelHeight);
    }

    private void OnValidate()
    {
        attachedCamera = GetComponent<Camera>();
        orthoSize = attachedCamera.orthographicSize;
        UpdateOrthoSize(orthoSize, attachedCamera.pixelWidth, attachedCamera.pixelHeight);
    }

    private void Update()
    {
        attachedCamera = GetComponent<Camera>();
        orthoSize = attachedCamera.orthographicSize;
        UpdateOrthoSize(orthoSize, attachedCamera.pixelWidth, attachedCamera.pixelHeight);
    }

    private void UpdateOrthoSize(float orthoHeight, float width, float height)
    {
        var ratio = width / height;
        var orthoWidth = orthoHeight * ratio;
        OrthoWidth = orthoWidth;
        OrthoHeight = orthoHeight;
        RightBound = orthoWidth * 2;
        BottomBound = orthoHeight * 2;
        attachedCamera.orthographicSize = orthoHeight;
        attachedCamera.transform.position = new Vector3(
            orthoWidth, orthoHeight, attachedCamera.transform.position.z);
    }
}
