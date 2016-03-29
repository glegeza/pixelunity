using UnityEngine;
using System.Collections;

public class HoverExpando : MonoBehaviour
{
    public float ExpandoFactor = 2.0f;

    private MouseObjectTracker _tracker;

    void Start()
    {
        _tracker = FindObjectOfType<MouseObjectTracker>();
    }

    void Update()
    {
        if (!_tracker)
        {
            return;
        }
        if (_tracker.CurrentObject == gameObject)
        {
            transform.localScale = new Vector3(ExpandoFactor, ExpandoFactor, 1.0f);
            transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);
        }
    }
}
