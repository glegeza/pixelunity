using UnityEngine;
using System.Collections;

public class BackgroundToggle : MonoBehaviour
{
    public KeyCode ToggleKey;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(ToggleKey))
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
        }
	}
}