using UnityEngine;
using Assets.Scripts;
using System.IO;

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
    private RenderTexture _currentTexture;
    private bool _shouldSave = false;

    private void Start()
    {
        Debug.LogFormat("Initializing PixelCamera...\nScreen resolution is {0}x{1}.",
            Screen.width, Screen.height);

        _pixelCamera = GetComponent<Camera>();
        _screenPixelsY = Screen.height;
        FindOutputCameraAndSurface();
        UpdateCameras();
    }

    private void OnPostRender()
    {
        if (_shouldSave)
        {
            Texture2D texture = new Texture2D(_currentTexture.width, _currentTexture.height);
            texture.ReadPixels(new Rect(0, 0, _currentTexture.width, _currentTexture.height), 0, 0);
            texture.Apply();
            var bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
            Debug.LogFormat("Saved file");
            _shouldSave = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _shouldSave = true;
        }
        if (_screenPixelsY != Screen.height)
        {
            Debug.LogFormat("Updating PixelCamera...\nOld screen height was {0}.\nNew resolution is {1}x{2}.",
                _screenPixelsY, Screen.width, Screen.height);

            UpdateCameras();
            _screenPixelsY = Screen.height;
        }
    }

    private void FindOutputCameraAndSurface()
    {
        foreach (Transform child in transform)
        {
            var cam = child.gameObject.GetComponent<Camera>();
            if (cam)
            {
                _outputCamera = cam;
                break;
            }
        }
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
        SetOrthoSize(((float)textureHeight / PixelsPerUnit) / 2.0f);
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

        SetOrthoSize(((float)TargetHeight / PixelsPerUnit) / 2.0f);
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
        SetOrthoSize(((float)TargetHeight / PixelsPerUnit) / 2.0f);
        UpdateRenderQuad(TargetWidth * bestScale, TargetHeight * bestScale);
    }

    private void SetOrthoSize(float size)
    {
        _pixelCamera.orthographicSize = size;
    }

    private void SetTexture(int textureWidth, int textureHeight)
    {
        Debug.LogFormat("Creating new {0}x{1} render texture", textureWidth, textureHeight);
        if (_currentTexture)
        {
            Destroy(_currentTexture);
            _currentTexture = null;
        }
        var texture = new RenderTexture(textureWidth, textureHeight, 16);
        texture.filterMode = SampleMode;
        _outputQuad.material.mainTexture = texture;
        _pixelCamera.targetTexture = texture;
        _currentTexture = texture;
    }

    private void UpdateRenderQuad(float widthPixels, float heightPixels)
    {
        float y = Screen.height;
        float x = Screen.width;
        var ortho = x / (((x / y) * 2.0f) * 1.0f);
        _outputCamera.orthographicSize = ortho;
        _outputQuad.transform.localScale = new Vector3(widthPixels, heightPixels, 1.0f);
        if (Screen.height % 2 != 0)
        {
            _outputQuad.transform.localPosition = new Vector3(-0.0f, 0.5f, 1.0f);
        }
        else
        {
            _outputQuad.transform.localPosition = new Vector3(-0.0f, 0.0f, 1.0f);
        }
        Debug.LogFormat("Setting quad scale to {0}", _outputQuad.transform.localScale);
    }
}