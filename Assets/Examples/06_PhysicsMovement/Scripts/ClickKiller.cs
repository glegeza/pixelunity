using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AsteroidKiller))]
public class ClickKiller : MonoBehaviour
{
    private MouseObjectTracker _tracker;
    private AsteroidKiller _killer;

	void Start ()
    {
        _tracker = GameObject.FindObjectOfType<MouseObjectTracker>();
        _killer = GetComponent<AsteroidKiller>();
        if (!_tracker)
        {
            enabled = false;
            return;
        }
        _tracker.ObjectClicked += OnObjectClicked;
	}

    private void OnObjectClicked(object sender, EventArgs e)
    {
        if (_tracker.CurrentObject != gameObject)
        {
            return;
        }
        _killer.MurderAsteroid();
    }
}
