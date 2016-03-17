using UnityEngine;

public class TestMover : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;
    public int PixelsPerUnit;
    public Camera PixelCamera;
    
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
        var xPos = transform.position.x + _currentSpeed * Time.deltaTime;
        var yPos = transform.position.y;
        var zPos = transform.position.z;
        var rightBound = (PixelCamera.pixelWidth / PixelsPerUnit) / 2.0f;
        var leftBound = -rightBound;
        var topBound = (PixelCamera.pixelHeight / PixelsPerUnit) / 2.0f;
        var bottomBound = -topBound;
        if (transform.position.x < leftBound)
        {
            _currentSpeed = Speed;
        }
        else if (transform.position.x > rightBound)
        {
            _currentSpeed = -Speed;
        }
        transform.position = new Vector3(xPos, yPos, zPos);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + RotationSpeed * Time.deltaTime);
    }
}
