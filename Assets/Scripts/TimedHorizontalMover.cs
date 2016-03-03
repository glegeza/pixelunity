using UnityEngine;

public class TimedHorizontalMover : MonoBehaviour
{
    public float Speed = 1.0f;
    public float MovementTime = 5.0f;

    private float timeMoving = 0.0f;
    private float currentSpeed;
    private Vector3 positionVector = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
        currentSpeed = Speed;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeMoving += Time.deltaTime;
        if (timeMoving > MovementTime)
        {
            currentSpeed *= -1;
            timeMoving = 0.0f;
        }
        transform.Translate(currentSpeed * Time.deltaTime, 0, 0);
	}
}