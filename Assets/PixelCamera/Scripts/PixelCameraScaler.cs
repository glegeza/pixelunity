using UnityEngine;
using Assets.Scripts;

[RequireComponent(typeof(Camera))]
public class PixelCameraScaler : MonoBehaviour
{
    public int TargetScale;
    public int TargetWidth;
    public int TargetHeight;
    public int PixelsPerUnit;
    public ScalingMode Mode;
    public FilterMode SampleMode = FilterMode.Point;
    
    private MeshRenderer _outputQuad;
    private Camera _pixelCamera;
    private Camera _outputCamera;
    private int _screenPixelsY;

    private void Start()
    {
        Debug.LogFormat("Initializing PixelCamera...\nScreen resolution is {0}x{1}.",
            Screen.width, Screen.height);

        _pixelCamera = GetComponent<Camera>();
        _screenPixelsY = Screen.height;
        FindOutputCameraAndSurface();
        UpdateCameras();
    }

    private void Update()
    {
        if (Mode != ScalingMode.FixedPlayArea && _screenPixelsY != Screen.height)
        {
            Debug.LogFormat("Updating PixelCamera...\nOld screen height was {0}.\nNew resolution is {1}x{2}.",
                _screenPixelsY, Screen.width, Screen.height);

            UpdateCameras();
            _screenPixelsY = Screen.height;
        }
    }

    private void FindOutputCameraAndSurface()
    {
        _outputCamera = GetComponentInChildren<Camera>();
        _outputQuad = _outputCamera.GetComponentInChildren<MeshRenderer>();
    }

    private void UpdateCameras()
    {
        switch (Mode)
        {
            case ScalingMode.BestFit:
                CreateBestFitCamera();
                break;
            case ScalingMode.FixedScale:
                CreateFixedScaleCamera();
                break;
            default:
                CreateFixedPlayAreaCamera();
                break;
        }
    }

    /// <summary>
    /// Creates an offscreen render target for the pixel camera that will fill
    /// as much of the screen as possible at the chosen TargetScale. This 
    /// option will never result in any cropping, but the viewable area in
    /// world units will vary based on the user's resolution.
    /// </summary>
    private void CreateFixedScaleCamera()
    {
        Debug.LogFormat("Creating fixed scale camera at {0}x...", TargetScale);

        var bestFitWidth = (int)(TargetScale * Mathf.Floor(Screen.width / (float)TargetScale));
        var bestFitHeight = (int)(TargetScale * Mathf.Floor(Screen.height / (float)TargetScale));
        var textureWidth = bestFitWidth / TargetScale;
        var textureHeight = bestFitHeight / TargetScale;
        SetTexture(textureWidth, textureHeight);
        _pixelCamera.orthographicSize = ((float)textureHeight / PixelsPerUnit) / 2.0f;
        UpdateRenderQuad(bestFitWidth, bestFitHeight);
    }

    /// <summary>
    /// Sets the offscreen pixel camera to render at TargetWidth x TargetHeight
    /// and scales the result by TargetScale for display to the screen. The
    /// viewable area in world units is guaranteed to be the same as what is
    /// shown in the editor, but the final scaling is resolution independent
    /// and may be cropped.
    /// </summary>
    private void CreateFixedPlayAreaCamera()
    {
        Debug.LogFormat("Creating fixed play area camera at {0}x{1} {2}x...",
            TargetWidth, TargetHeight, TargetScale);
        Debug.LogFormat("Final output will be {0}x{1}", TargetWidth * TargetScale, TargetHeight * TargetScale);
        SetTexture(TargetWidth, TargetHeight);

        _pixelCamera.orthographicSize = ((float)TargetHeight / PixelsPerUnit) / 2.0f;
        UpdateRenderQuad(TargetWidth * TargetScale, TargetHeight * TargetScale);
    }

    /// <summary>
    /// Sets the offscreen pixel camera to render at TargetWidth x TargetHeight
    /// and scales up as much as possible for screen resolution. The viewable
    /// area in world units is guaranteed to be the same as what is shown in
    /// the editor and the final output will never be cropped. The output may
    /// be letterboxed (possibly on all sides).
    /// </summary>
    private void CreateBestFitCamera()
    {
        Debug.LogFormat("Creating best fit camera with render target resolution of {0}x{1}...",
            TargetWidth, TargetHeight);

        var bestScale = 0;
        while (TargetWidth * (bestScale + 1) < Screen.width && TargetHeight * (bestScale + 1) < Screen.height)
        {
            bestScale += 1;
        }
        var bestWidth = bestScale * TargetWidth;
        var bestHeight = bestScale * TargetHeight;
        Debug.LogFormat("Best fit scale is {0}x", bestScale);
        Debug.LogFormat("Final output resolution will be {0}x{1}", bestWidth, bestHeight);
        SetTexture(TargetWidth, TargetHeight);
        _pixelCamera.orthographicSize = ((float)TargetHeight / PixelsPerUnit) / 2.0f;
        UpdateRenderQuad(TargetWidth * bestScale, TargetHeight * bestScale);
    }

    private void SetTexture(int textureWidth, int textureHeight)
    {
        Debug.LogFormat("Creating new {0}x{1} render texture", textureWidth, textureHeight);

        var texture = new RenderTexture(textureWidth, textureHeight, 16);
        texture.filterMode = SampleMode;
        _outputQuad.material.mainTexture = texture;
        _pixelCamera.targetTexture = texture;
    }

    private void UpdateRenderQuad(float widthPixels, float heightPixels)
    {
        var unitsPerPixel = 1.0f / Screen.height;
        _outputQuad.transform.localScale = new Vector3(widthPixels * unitsPerPixel, heightPixels * unitsPerPixel, 1.0f);
        Debug.LogFormat("Setting quad scale to {0}", _outputQuad.transform.localScale);
    }
}