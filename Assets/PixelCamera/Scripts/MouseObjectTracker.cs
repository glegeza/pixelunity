using UnityEngine;
using System;

public class MouseObjectTracker : MonoBehaviour
{
    private PixelMouse _mouse;
    private PixelCameraScaler _scaler;

    public bool Announce = true;
    public GameObject CurrentObject { get; private set; }

    public event EventHandler ObjectClicked;
    
	void Start ()
    {
        _mouse = FindObjectOfType<PixelMouse>();
        _scaler = _mouse.GetComponent<PixelCameraScaler>();
	}
	
	void Update ()
    {
        var mousePos = _mouse.GetMouseWorldLocation();
        var collider = Physics2D.OverlapPoint(mousePos);

        if (collider != null)
        {
            if (Announce && collider.gameObject != CurrentObject)
            {
                Debug.LogFormat("Mouse target: {0}", collider.gameObject.name);
            }
            CurrentObject = collider.gameObject;
        }
        else
        {
            if (Announce && CurrentObject != null)
            {
                Debug.Log("Mouse target: None.");
            }
            CurrentObject = null;
        }

        if (CurrentObject && Input.GetMouseButtonDown(0))
        {
            ObjectClicked(this, EventArgs.Empty);
        }
    }
}