using UnityEngine;

public class RandomAsteroidRotation : MonoBehaviour
{
    public float MaxRotationSpeed;
    public float MinRotationSpeed;

    private float _rotationSpeed;

	void Start ()
    {
        _rotationSpeed = Random.Range(MinRotationSpeed, MaxRotationSpeed);
	}
	
	void Update ()
    {
        transform.Rotate(0.0f, 0.0f, _rotationSpeed * Time.deltaTime);
	}
}
