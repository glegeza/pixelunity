using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileCuller : MonoBehaviour
{
    [Tooltip("Lifetime of the tile once it is no longer visible to the camera.")]
    public float CullTime = 2.0f;

    private SpriteRenderer _renderer;
    private float _timeUnseen;
    
	void Start ()
    {
        _renderer = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        if (_renderer.isVisible)
        {
            _timeUnseen = 0.0f;
            return;
        }
        else
        {
            _timeUnseen += Time.deltaTime;
        }

        if (_timeUnseen > CullTime)
        {
            Destroy(gameObject);
        }
	}
}
