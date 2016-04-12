using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class SmoothFollowCamera : MonoBehaviour
{
    public float DampTime = 0.15f;
    public Transform Target;

    private Vector3 velocity = Vector3.zero;
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Target)
        {
            Vector3 point = _camera.WorldToViewportPoint(Target.position);
            Vector3 delta = Target.position - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);
        }

    }
}