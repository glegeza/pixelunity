using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HoverColorizer : MonoBehaviour
{
    public Color BaseColor = Color.white;
    public Color HoverColor = Color.red;

    private MouseObjectTracker _tracker;
    private SpriteRenderer _spriteRenderer;
    
	void Start ()
    {
        _tracker = FindObjectOfType<MouseObjectTracker>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        if (!_tracker)
        {
            return;
        }
        if (_tracker.CurrentObject == gameObject)
        {
            _spriteRenderer.color = HoverColor;
        }
        else
        {
            _spriteRenderer.color = BaseColor;
        }
    }
}
