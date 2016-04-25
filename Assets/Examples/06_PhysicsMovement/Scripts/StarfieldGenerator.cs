using UnityEngine;

public class StarfieldGenerator : MonoBehaviour
{
    public float Parralax = 2.0f;
    public float MinIntensity = 0.25f;
    public float MaxIntensity = 0.6f;
    public int Padding = 2;
    public int Stars = 50;
    public Transform FollowTarget;
    public bool Static;

    private int _width = 50;
    private int _height = 50;
    private Texture2D _starfieldTexture;
    private Material _material;

    void Start()
    {
        _width = Screen.width;
        _height = Screen.height;
        CreateTexture();
        var renderer = GetComponent<MeshRenderer>();
        _material = renderer.material;
        _material.mainTexture = _starfieldTexture;
        transform.position = new Vector3(0.0f, 0.0f, transform.position.z);
        transform.localScale = new Vector3(_width * 1 / 16.0f, _height * 1 / 16.0f, 1.0f);
	}
	
    private void CreateTexture()
    {
        _starfieldTexture = new Texture2D(_width, _height);
        var texColors = new Color32[_width * _height];
        for (var i = 0; i < texColors.Length; i++)
        {
            texColors[i] = Color.clear;
        }
        _starfieldTexture.SetPixels32(texColors);
        var xMin = Padding;
        var xMax = _width - Padding;
        var yMin = Padding;
        var yMax = _height - Padding;
        for (var i = 0; i < Stars; i++)
        {
            var intensity = Random.Range(MinIntensity, MaxIntensity);
            var x = Random.Range(xMin, xMax);
            var y = Random.Range(yMin, yMax);
            _starfieldTexture.SetPixel(x, y, new Color(intensity, intensity, intensity));
        }
        _starfieldTexture.Apply();
        _starfieldTexture.filterMode = FilterMode.Point;
    }

	void Update ()
    {
        if (Static)
        {
            return;
        }
        var offset = new Vector2();
        offset.x = transform.position.x / transform.localScale.x / Parralax;
        offset.y = transform.position.y / transform.localScale.y / Parralax;
        _material.mainTextureOffset = offset;
	}
}
