using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectScale : MonoBehaviour
{
    public int PixelXOffset { get; set; }
    public int PixelYOffset { get; set; }
    public int TargetWidth = 0;
    public int TargetHeight = 0;

	private int screenPixelsY = 0;

    void Start()
    {
        UpdateSize();
    }

	void Update()
	{
		if(screenPixelsY != (float)Screen.height)
		{
            UpdateSize();
		}
	}

    void UpdateSize()
    {
        screenPixelsY = Screen.height;
        var unitsPerPixel = 1.0f / screenPixelsY;

        if (TargetWidth == 0 || TargetHeight == 0)
        {
            transform.localScale = new Vector3((float)Screen.width / Screen.height - (unitsPerPixel * PixelXOffset), 1.0f - (unitsPerPixel * PixelYOffset), 1.0f);
        }
        else
        {
            transform.localScale = new Vector3((float)TargetWidth * unitsPerPixel, (float)TargetHeight * unitsPerPixel, 1.0f);
        }
        Debug.Log("Setting scale to " + transform.localScale);
    }
}
