using UnityEngine;
using System.Collections;

public class GridPosition : MonoBehaviour
{
    public int XPos = 0;
    public int YPos = 0;

	void Update ()
    {
        transform.position = new Vector3(XPos, YPos, transform.position.z);
	}
}
