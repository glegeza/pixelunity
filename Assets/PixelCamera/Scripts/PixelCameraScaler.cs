using UnityEngine;
using Assets.Scripts;
using System.IO;

[RequireComponent(typeof(Camera))]
public class PixelCameraScaler : MonoBehaviour
{
    public int Scale;
    public int OffscreenWidth;
    public int OffscreenHeight;
    public int PixelsPerUnit;
    public ScalingMode Mode;
    public FilterMode SampleMode = FilterMode.Point;
    
    private int _screenPixelsY;
    private int _screenPixelsX;
    private MeshRenderer _outputQuad;
    private Camera _pixelCamera;
    private Camera _outputCamera;
    private RenderTexture _currentTexture;
    private bool _shouldSave = false;

    private void Start()
    {
        Debug.LogFormat("Initializing PixelCamera...\nScreen resolution is {0}x{1}.",
            Screen.width, Screen.height);

        _pixelCamera = GetComponent<Camera>();
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
        if (_screenPixelsY != Screen.height || _screenPixelsX != Screen.width)
        {
            Debug.LogFormat("Updating PixelCamera...\nOld screen height was {0}.\nNew resolution is {1}x{2}.",
                _screenPixelsY, Screen.width, Screen.height);

            UpdateCameras();
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
                CheckScale();
                CreateFixedScaleCamera();
                break;
            default:
                CheckScale();
                CreateFixedPlayAreaCamera();
                break;
        }
        _screenPixelsY = Screen.height;
        _screenPixelsX = Screen.width;
    }


    private void CheckScale()
    {
        if (Scale < 1)
        {
            Debug.LogErrorFormat("FixedPlayArea and FixedScale require Scale greater than 0. Scale is set to {0}.", Scale);
            Debug.LogFormat("Setting scale to 1.");
            Scale = 1;
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
        Debug.LogFormat("Creating fixed scale camera at {0}x...", Scale);

        var bestFitWidth = (int)(Scale * Mathf.Floor(Screen.width / (float)Scale));
        var bestFitHeight = (int)(Scale * Mathf.Floor(Screen.height / (float)Scale));
        var textureWidth = bestFitWidth / Scale;
        var textureHeight = bestFitHeight / Scale;
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
            OffscreenWidth, OffscreenHeight, Scale);
        Debug.LogFormat("Final output will be {0}x{1}", OffscreenWidth * Scale, OffscreenHeight * Scale);
        SetTexture(OffscreenWidth, OffscreenHeight);

        SetOrthoSize(((float)OffscreenHeight / PixelsPerUnit) / 2.0f);
        UpdateRenderQuad(OffscreenWidth * Scale, OffscreenHeight * Scale);
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
            OffscreenWidth, OffscreenHeight);

        var bestScale = 0;
        while (OffscreenWidth * (bestScale + 1) < Screen.width && 
            OffscreenHeight * (bestScale + 1) < Screen.height && 
            (Scale < 1 || bestScale <= Scale))
        {
            bestScale += 1;
        }
        var bestWidth = bestScale * OffscreenWidth;
        var bestHeight = bestScale * OffscreenHeight;
        Debug.LogFormat("Best fit scale is {0}x", bestScale);
        Debug.LogFormat("Final output resolution will be {0}x{1}", bestWidth, bestHeight);
        SetTexture(OffscreenWidth, OffscreenHeight);
        SetOrthoSize(((float)OffscreenHeight / PixelsPerUnit) / 2.0f);
        UpdateRenderQuad(OffscreenWidth * bestScale, OffscreenHeight * bestScale);
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
        // Set orthographic size of output camera
        float y = Screen.height;
        float x = Screen.width;
        var ortho = x / (((x / y) * 2.0f) * 1.0f);
        _outputCamera.orthographicSize = ortho;

        // Scale output quad
        _outputQuad.transform.localScale = new Vector3(widthPixels, heightPixels, 1.0f);
        Debug.LogFormat("Setting quad scale to {0}", _outputQuad.transform.localScale);

        // Check if the screen width/height are odd and move the output quad
        // by half a pixel to keep it on pixel boundaries
        float xQuadPos = 0.0f;
        float yQuadPos = 0.0f;
        float zQuadPos = 1.0f;
        if (Screen.height % 2 != 0)
        {
            yQuadPos = 0.5f;
        }
        if (Screen.width % 2 != 0)
        {
            xQuadPos = -0.5f;
        }
        _outputQuad.transform.localPosition = new Vector3(xQuadPos, yQuadPos, zQuadPos);
        Debug.LogFormat("Setting quad position to {0}", _outputQuad.transform.localPosition);
    }
}