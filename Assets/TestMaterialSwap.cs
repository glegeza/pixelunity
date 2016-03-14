using UnityEngine;
using System;
using Assets.Scripts;

public class TestMaterialSwap : MonoBehaviour
{
    public float TargetScale;
    public int TargetWidth;
    public int TargetHeight;
    public int PixelsPerUnit;
    public ScalingMode Mode;
    public FilterMode SampleMode = FilterMode.Point;
    public bool AllowCropping;
    public MeshRenderer Quad;
    public PixelPerfectScale Scaler;

    private int MaxScale = 16;
    private Camera attachedCamera;
    private int RenderWidth;
    private int RenderHeight;
    private int XOffset;
    private int YOffset;
    
	void Start ()
    {
        var targetSize = GetTargetScreenSize();
        var targetWidth =  targetSize[0];
        var targetHeight = targetSize[1];
        attachedCamera = GetComponent<Camera>();
        var texture = GetNewTexture(targetWidth, targetHeight);
        AttachTexture(texture);
        Scaler.screenVerticalPixels = targetHeight;
        if (Mode == ScalingMode.FixedPlayArea)
        {
            Scaler.TargetWidth = (int)(TargetWidth * TargetScale);
            Scaler.TargetHeight = (int)(TargetHeight * TargetScale);
            attachedCamera.orthographicSize = ((float)targetHeight / PixelsPerUnit) / 2.0f;
        }
        else if (Mode == ScalingMode.BestFit)
        {
            Scaler.TargetWidth = (int)(TargetWidth * TargetScale);
            Scaler.TargetHeight = (int)(TargetHeight * TargetScale);
            attachedCamera.orthographicSize = ((float)TargetHeight / PixelsPerUnit) / 2.0f;
        }
        else
        {
            attachedCamera.orthographicSize = ((float)targetHeight / PixelsPerUnit) / 2.0f;
        }
        Scaler.PixelXOffset = XOffset;
        Scaler.PixelYOffset = YOffset;
	}

    private int[] GetTargetScreenSize()
    {
        var baseWidth = Screen.width;
        var baseHeight = Screen.height;
        var bestFitWidth = Screen.width;
        var bestFitHeight = Screen.height;
        var diffWidth = 0;
        var diffHeight = 0;

        switch (Mode)
        {
            case ScalingMode.FixedScale:
                bestFitWidth = (int)(TargetScale * Mathf.Floor(baseWidth / TargetScale));
                bestFitHeight = (int)(TargetScale * Mathf.Floor(baseHeight / TargetScale));
                diffWidth = XOffset = baseWidth - bestFitWidth;
                diffHeight = YOffset = baseHeight - bestFitHeight;
                Debug.Log(String.Format("{0} -> {1} [{2}]", baseWidth, bestFitWidth, diffWidth));
                Debug.Log(String.Format("{0} -> {1} [{2}]", baseHeight, bestFitHeight, diffHeight));
                return new int[] 
                {
                    (int)(bestFitWidth / TargetScale),
                    (int)(bestFitHeight / TargetScale)
                };
            case ScalingMode.FixedPlayArea:
                return new int[] 
                {
                    TargetWidth,
                    TargetHeight
                };
            case ScalingMode.BestFit:
                bestFitWidth = TargetWidth;
                bestFitHeight = TargetHeight;
                TargetScale = 1;
                for (var scale = 1; scale < MaxScale; scale++)
                {
                    var scaleWidth = TargetWidth * scale;
                    var scaleHeight = TargetHeight * scale;
                    if (scaleWidth > Screen.width || scaleHeight > Screen.height)
                    {
                        break;
                    }
                    TargetScale = scale;
                    bestFitWidth = scaleWidth;
                    bestFitHeight = scaleHeight;
                }
                return new int[]
                {
                    bestFitWidth,
                    bestFitHeight
                };
            default:
                return new int[]
                {
                    Screen.width,
                    Screen.height
                };
        } 
    }

    private RenderTexture GetNewTexture(int targetWidth, int targetHeight)
    {
        var textureWidth = targetWidth;
        var textureHeight = targetHeight;
        switch (Mode)
        {
            case ScalingMode.FixedPlayArea:
                textureWidth = TargetWidth;
                textureHeight = TargetHeight;
                break;
            case ScalingMode.BestFit:
                textureWidth = TargetWidth;
                textureHeight = TargetWidth;
                break;
            case ScalingMode.FixedScale:
            default:
                break;
        }
        Debug.Log(String.Format("Creating new render texture {0}x{1}", textureWidth, textureHeight));
        var texture = new RenderTexture(textureWidth, textureHeight, 16);
        texture.filterMode = SampleMode;
        return texture;
    }

    private void AttachTexture(RenderTexture texture)
    {
        Quad.material.mainTexture = texture;
        attachedCamera.targetTexture = texture;
    }
}