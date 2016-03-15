using UnityEngine;

public class MoveDude : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;
    public float MoveTime;

    private float elapsedTime;
    private float currentSpeed;
	
    void Start()
    {
        currentSpeed = Speed;
    }
    
	void Update ()
    {
        elapsedTime += Time.deltaTime;
        var xPos = transform.position.x + currentSpeed * Time.deltaTime;
        var yPos = transform.position.y;
        var zPos = transform.position.z;
        if (elapsedTime > MoveTime)
        {
            currentSpeed *= -1;
            elapsedTime = 0.0f;
        }
        transform.position = new Vector3(xPos, yPos, zPos);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + RotationSpeed * Time.deltaTime);
    }
}
