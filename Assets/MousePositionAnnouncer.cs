using UnityEngine;
using System.Collections;

public class MousePositionAnnouncer : MonoBehaviour
{
    private PixelMouse _mouse;

	// Use this for initialization
	void Start ()
    {
        _mouse = FindObjectOfType<PixelMouse>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.LogFormat("{0} -- {1}", _mouse.GetMouseWorldLocation(), _mouse.GetMouseScreenLocation());
	}
}
