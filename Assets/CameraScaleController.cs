using UnityEngine;

public class CameraScaleController : MonoBehaviour
{
    public PixelCameraScaler scaler;
    public KeyCode ZoomIn;
    public KeyCode ZoomOut;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(ZoomIn))
        {
            scaler.Scale += 1;
            scaler.ForceUpdate();
        }
        else if (Input.GetKeyDown(ZoomOut) && scaler.Scale > 1)
        {
            scaler.Scale -= 1;
            scaler.ForceUpdate();
        }
	}
}
