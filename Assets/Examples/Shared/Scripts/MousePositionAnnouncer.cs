using UnityEngine;
using System.Collections;

public class MousePositionAnnouncer : MonoBehaviour
{
    public bool Active = true;

    private PixelMouse _mouse;

	// Use this for initialization
	void Start ()
    {
        _mouse = FindObjectOfType<PixelMouse>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!Active)
        {
            return;
        }
        Debug.LogFormat("{0} -- {1}", _mouse.GetMouseWorldLocation(), _mouse.GetMouseScreenLocation());
	}
}
