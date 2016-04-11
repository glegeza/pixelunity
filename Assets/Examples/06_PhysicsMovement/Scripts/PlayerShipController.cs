using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerShipController : MonoBehaviour
{

    public float MaxVelocity;
    public float AcceleratonForce;
    public float Torque;
    public KeyCode RotateLeftKey;
    public KeyCode RotateRightKey;
    public KeyCode ForwardAccelerationKey;

    private Rigidbody2D _rigid2D;
    private ParticleSystem _particle;

	void Start ()
    {
        _rigid2D = GetComponent<Rigidbody2D>();
        _particle = GetComponentInChildren<ParticleSystem>();
	}
	
	void Update ()
    {
        if (Input.GetKey(ForwardAccelerationKey))
        {
            _rigid2D.AddForce(transform.up * AcceleratonForce);
            if (!_particle.isPlaying)
            {
                _particle.Play();
            }
        }
        else if (_particle.isPlaying)
        {
            _particle.Stop();
        }
        if (Input.GetKey(RotateLeftKey))
        {
            _rigid2D.AddTorque(Torque);
        }
        if (Input.GetKey(RotateRightKey))
        {
            _rigid2D.AddTorque(-Torque);
        }
	}
}
