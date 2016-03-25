using UnityEngine;
using System.Collections;

public class MouseObjectTracker : MonoBehaviour
{
    private PixelMouse _mouse;
    private PixelCameraScaler _scaler;

    public GameObject CurrentObject { get; private set; }

	// Use this for initialization
	void Start ()
    {
        _mouse = FindObjectOfType<PixelMouse>();
        _scaler = _mouse.GetComponent<PixelCameraScaler>();
	}
	
	// Update is called once per frame
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