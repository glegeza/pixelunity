using UnityEngine;

public class TestMaterialSwap : MonoBehaviour
{
    public int TargetScale;
    public int PixelsPerUnit;
    public MeshRenderer Quad;
    public PixelPerfectScale Scaler;

    private Camera attachedCamera;
    
	void Start ()
    {
        var screenWidth =  Screen.width;
        var screenHeight = Screen.height;
        if (TargetScale > 1)
        {
            if (screenWidth % 2 != 0)
            {
                screenWidth -= 1;
            }
            if (screenHeight % 2 != 0)
            {
                screenHeight -= 1;
            }
        }
        attachedCamera = GetComponent<Camera>();
        RenderTexture texture = new RenderTexture(screenWidth, screenHeight, 16);
        texture.filterMode = FilterMode.Point;
        Quad.material.mainTexture = texture;
        attachedCamera.targetTexture = texture;
        Scaler.screenVerticalPixels = screenHeight;
        attachedCamera.orthographicSize = ((float)screenHeight / PixelsPerUnit) / 2.0f;
	}
}