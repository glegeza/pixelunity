using UnityEngine;

public class TestMover : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;
    public float MoveTime;

    private float _elapsedtime;
    private float _currentSpeed;
    private bool _moving = true;
	
    void Start()
    {
        _currentSpeed = Speed;
    }
    
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _moving = !_moving;
        }
        if (!_moving)
        {
            return;
        }
        _elapsedtime += Time.deltaTime;
        var xPos = transform.position.x + _currentSpeed * Time.deltaTime;
        var yPos = transform.position.y;
        var zPos = transform.position.z;
        if (_elapsedtime > MoveTime)
        {
            _currentSpeed *= -1;
            _elapsedtime = 0.0f;
        }
        transform.position = new Vector3(xPos, yPos, zPos);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + RotationSpeed * Time.deltaTime);
    }
}
