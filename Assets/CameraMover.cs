using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float Speed;
    public Rect CameraBounds;

    private Vector2 _curSpeed;
    
	void Start ()
    {

	}
	
	void Update ()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _curSpeed.y = Speed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _curSpeed.y = -Speed;
        }
        else
        {
            _curSpeed.y = 0.0f;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _curSpeed.x = -Speed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _curSpeed.x = Speed;
        }
        else
        {
            _curSpeed.x = 0.0f;
        }

        var x = transform.position.x + _curSpeed.x * Time.deltaTime;
        var y = transform.position.y + _curSpeed.y * Time.deltaTime;
        var z = transform.position.z;

        if (x < CameraBounds.xMin)
        {
            x = CameraBounds.xMin;
            _curSpeed.x = 0.0f;
        }
        if (x > CameraBounds.xMax)
        {
            x = CameraBounds.xMax;
            _curSpeed.x = 0.0f;
        }
        if (y > CameraBounds.yMax)
        {
            y = CameraBounds.yMax;
            _curSpeed.y = 0.0f;
        }
        if (y < CameraBounds.yMin)
        {
            y = CameraBounds.yMin;
            _curSpeed.y = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            x = y = 0.0f;
        }

        transform.position = new Vector3(x, y, z);

    }
}