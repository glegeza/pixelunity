using UnityEngine;
using Assets.Scripts;
using System.IO;

[RequireComponent(typeof(Camera))]
public class PixelCameraScaler : MonoBehaviour
{
    [Tooltip("The amount of scaling applied to final output.")]
    public int Scale;
    [Tooltip("The width used by the offscreen render target. Ignored in FixedScale mode.")]
    public int OffscreenWidth;
    [Tooltip("The height used by the offscreen render target. Ignored in FixedScale mode.")]
    public int OffscreenHeight;
    [Tooltip("The Pixels Per Unit value used by sprites that will be displayed in the scene.")]
    public int PixelsPerUnit;
    [Tooltip("The type of scaling to apply. FixedScale is the only mode that will expand the play area.")]
    public ScalingMode Mode;
    [Tooltip("The filter mode to apply when scaling the offscreen render texture.")]
    public FilterMode SampleMode = FilterMode.Point;
    [Tooltip("The camera to output the scaled scene to.")]
    public Camera OutputCamera;

    public int CurrentScale { get; private set; }

    public Transform OutputQuad { get { return _outputQuad.transform; } }

    private int _screenPixelsY;
    private int _screenPixelsX;
    private MeshRenderer _outputQuad;
    private Camera _pixelCamera;
    
    private RenderTexture _currentTexture;
    private bool _shouldSave = false;

    public Vector2 RoundToPixelBoundary(Vector2 worldPos)
    {
        worldPos.x = Mathf.RoundToInt(worldPos.x * PixelsPerUnit) / (float)PixelsPerUnit;
        worldPos.y = Mathf.RoundToInt(worldPos.y * PixelsPerUnit) / (float)PixelsPerUnit;
        return worldPos;
    }

    public Vector2 WorldToPixelPoint(Vector2 pos)
    {
        return WorldToPixelPoint(pos.x, pos.y);
    }

    public Vector2 WorldToPixelPoint(float worldX, float worldY)
    {
        var heightOffset = Mathf.FloorToInt(_pixelCamera.orthographicSize * PixelsPerUnit);
        var widthOffset = _pixelCamera.pixelWidth / 2;
        var x = Mathf.FloorToInt(worldX * PixelsPerUnit) + widthOffset;
        var y = Mathf.FloorToInt(worldY * PixelsPerUnit) + heightOffset;
        return new Vector2(x, y);
    }

    public Vector2 ConvertToPixels(Vector2 pos)
    {
        return ConvertToPixels(pos.x, pos.y);
    }

    public Vector2 ConvertToPixels(float x, float y)
    {
        return new Vector2(
            Mathf.FloorToInt(x * PixelsPerUnit),
            Mathf.FloorToInt(y * PixelsPerUnit)
            );
    }

    public void ForceUpdate()
    {
        UpdateCameras();
    }

    private void Start()
    {
        Debug.LogFormat("Initializing PixelCamera...\nScreen resolution is {0}x{1}.",
            Screen.width, Screen.height);

        _pixelCamera = GetComponent<Camera>();
        FindOutputCameraAndSurface();
        UpdateCameras();
    }

    private void OnValidate()
    {
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
            Debug.LogFormat("Updating PixelCamera...\nOld resolution was {0}x{1}.\nNew resolution is {2}x{3}.",
                _screenPixelsX, _screenPixelsY, Screen.width, Screen.height);

            UpdateCameras();
        }
    }

    private void FindOutputCameraAndSurface()
    {
        if (!OutputCamera)
        {
            _outputQuad = null;
            return;
        }
        _outputQuad = OutputCamera.GetComponentInChildren<MeshRenderer>();        
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
        _pixelCamera.orthographicSize = ((float)textureHeight / PixelsPerUnit) / 2.0f;
        UpdateRenderQuad(bestFitWidth, bestFitHeight);
        CurrentScale = Scale;
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

        _pixelCamera.orthographicSize = ((float)OffscreenHeight / PixelsPerUnit) / 2.0f;
        UpdateRenderQuad(OffscreenWidth * Scale, OffscreenHeight * Scale);
        CurrentScale = Scale;
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
        _pixelCamera.orthographicSize = ((float)OffscreenHeight / PixelsPerUnit) / 2.0f;
        UpdateRenderQuad(OffscreenWidth * bestScale, OffscreenHeight * bestScale);
        CurrentScale = bestScale;
    }

    private void SetTexture(int textureWidth, int textureHeight)
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (_currentTexture)
        {
            Debug.LogFormat("Destroying old render texture.");
            Destroy(_currentTexture);
            _currentTexture = null;
        }
        Debug.LogFormat("Creating new {0}x{1} render texture", textureWidth, textureHeight);
        var texture = new RenderTexture(textureWidth, textureHeight, 16);
        texture.filterMode = SampleMode;
        _outputQuad.material.mainTexture = texture;
        _pixelCamera.targetTexture = texture;
        _currentTexture = texture;
    }

    private void UpdateRenderQuad(float widthPixels, float heightPixels)
    {
        if (!Application.isPlaying)
        {
            return;
        }

        // Set orthographic size of output camera
        float y = Screen.height;
        float x = Screen.width;
        var ortho = x / (((x / y) * 2.0f) * 1.0f);
        OutputCamera.orthographicSize = ortho;

        // Scale output quad
        _outputQuad.transform.localScale = new Vector3(widthPixels, heightPixels, 1.0f);
        Debug.LogFormat("Setting quad scale to {0}", _outputQuad.transform.localScale);

        // Check if the screen width/height are odd and move the output quad
        // by half a pixel to keep it on pixel boundaries
        float xQuadPos = 0.0f;
        float yQuadPos = 0.0f;
        float zQuadPos = 1.0f;
        if ((Screen.height % 2 != 0 && (int)heightPixels % 2 == 0) ||
            Screen.height % 2 == 0 & (int)heightPixels % 2 != 0)
        {
            yQuadPos = 0.5f;
        }
        if ((Screen.width % 2 != 0 && (int)widthPixels % 2 == 0) ||
            Screen.width % 2 == 0 & (int)widthPixels % 2 != 0)
        {
            xQuadPos = 0.5f;
        }
        _outputQuad.transform.localPosition = new Vector3(xQuadPos, yQuadPos, zQuadPos);
        Debug.LogFormat("Setting quad position to {0}", _outputQuad.transform.localPosition);
        Debug.LogFormat("Texture Resolution: {0}x{1}, Screen Resolution: {2}x{3}", widthPixels, heightPixels, Screen.width, Screen.height);
    }
}