using UnityEngine;

[ExecuteInEditMode]
public class PixelPerfectScale : MonoBehaviour
{
	public int screenVerticalPixels;

	public bool preferUncropped = true;

	private float screenPixelsY = 0;
	
	private bool currentCropped = false;

    void Start()
    {
        UpdateSize();
    }

	void Update()
	{
		if(screenPixelsY != (float)Screen.height || currentCropped != preferUncropped)
		{
            UpdateSize();
		}
	}

    void UpdateSize()
    {
        screenPixelsY = (float)Screen.height;
        currentCropped = preferUncropped;

        float screenRatio = screenPixelsY / screenVerticalPixels;
        float ratio;

        if (preferUncropped)
        {
            ratio = Mathf.Floor(screenRatio) / screenRatio;
        }
        else
        {
            ratio = Mathf.Ceil(screenRatio) / screenRatio;
        }

        transform.localScale = new Vector3((float)Screen.width / Screen.height, 1.0f, 1.0f);
        Debug.Log("Setting scale to " + transform.localScale);
    }
}
