using UnityEngine;
using System.Collections;

public class MouseObjectTracker : MonoBehaviour
{
    private PixelMouse _mouse;
    private PixelCameraScaler _scaler;

    public GameObject CurrentObject { get; private set; }
    
	void Start ()
    {
        _mouse = FindObjectOfType<PixelMouse>();
        _scaler = _mouse.GetComponent<PixelCameraScaler>();
	}
	
	void Update ()
    {
        var mousePos = _mouse.GetMouseWorldLocation();
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            CurrentObject = hit.collider.gameObject;
        }
        else
        {
            CurrentObject = null;
        }
    }
}