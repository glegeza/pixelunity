using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform Camera;
    public float Speed;
    public Rect Bounds;

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

        var x = Camera.position.x + _curSpeed.x * Time.deltaTime;
        var y = Camera.position.y + _curSpeed.y * Time.deltaTime;
        var z = Camera.position.z;

        if (x < Bounds.xMin)
        {
            x = Bounds.xMin;
            _curSpeed.x = 0.0f;
        }
        if (x > Bounds.xMax)
        {
            x = Bounds.xMax;
            _curSpeed.x = 0.0f;
        }
        if (y > Bounds.yMax)
        {
            y = Bounds.yMax;
            _curSpeed.y = 0.0f;
        }
        if (y < Bounds.yMin)
        {
            y = Bounds.yMin;
            _curSpeed.y = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            x = y = 0.0f;
        }

        Camera.position = new Vector3(x, y, z);

    }
}