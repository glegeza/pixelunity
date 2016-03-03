using UnityEngine;
using System.Collections.Generic;

public class GridController : MonoBehaviour
{
    public KeyCode MoveLeft;
    public KeyCode MoveRight;
    public KeyCode MoveDown;
    public KeyCode MoveUp;

    private GridPosition gridPos;

	// Use this for initialization
	void Start ()
    {
        gridPos = GetComponent<GridPosition>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(MoveLeft))
        {
            gridPos.XPos -= 1;
        }
        else if (Input.GetKeyDown(MoveRight))
        {
            gridPos.XPos += 1;
        }
        else if (Input.GetKeyDown(MoveUp))
        {
            gridPos.YPos += 1;
        }
        else if (Input.GetKeyDown(MoveDown))
        {
            gridPos.YPos -= 1;
        }
	}
}
