using UnityEngine;
using System.Collections;

public class MouseObjectTracker : MonoBehaviour
{

    private PixelMouse _mouse;

    public GameObject CurrentObject { get; private set; }

	// Use this for initialization
	void Start ()
    {
        _mouse = FindObjectOfType<PixelMouse>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit2D hit = Physics2D.Raycast(_mouse.GetMouseWorldLocation(), Vector2.zero);

        if (hit.collider != null)
        {
            CurrentObject = hit.collider.gameObject;
            Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
        }
        else
        {
            CurrentObject = null;
        }
    }
}
