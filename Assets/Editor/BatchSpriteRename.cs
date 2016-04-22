using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class BatchSpriteRename : EditorWindow
{
    [Serializable]
    public class Model : ScriptableObject
    {
        public List<string> AsteroidTypes = new List<string>();
        public List<string> AsteroidCategories = new List<string>();
    }

    public Texture2D Texture;
    public int SpritesPerRow = 12;
    public Model Info;

    private SerializedObject _serializedInfo;

    void OnEnable()
    {
        Info = new Model();
        _serializedInfo = new SerializedObject(Info);
    }

    [MenuItem("Window/Stuff/Slice Renamer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BatchSpriteRename), false, "Sprite Renamer");
    }

    void OnGUI()
    {
        string textureName = Texture == null ? "No texture selected" : Texture.name;
        EditorGUILayout.LabelField(textureName, EditorStyles.boldLabel);
        Texture = EditorGUILayout.ObjectField("Texture", Texture, typeof(Texture2D), true) as Texture2D;

        EditorGUILayout.PropertyField(_serializedInfo.FindProperty("AsteroidTypes"), new GUIContent("Asteroid Types"), true);
        EditorGUILayout.PropertyField(_serializedInfo.FindProperty("AsteroidCategories"), new GUIContent("Asteroid Categories"), true);
        _serializedInfo.ApplyModifiedProperties();

        if (GUILayout.Button("Rename Slices"))
        {
            Apply();
        }
    }

    void Apply()
    {
        if (!Texture)
        {
            return;
        }
        if (Info.AsteroidTypes.Count < SpritesPerRow)
        {
            return;
        }

        var path = AssetDatabase.GetAssetPath(Texture);
        var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        SpriteMetaData[] sliceMetaData = textureImporter.spritesheet;

        if (sliceMetaData == null || sliceMetaData.Length == 0)
        {
            return;
        }

        var idx = 0;
        foreach (SpriteMetaData spriteData in sliceMetaData)
        {
            var comps = spriteData.name.Split('_');
            var num = Convert.ToInt32(comps[comps.Length - 1]);
            var category = num / 12;
            var type = num % 12;
            sliceMetaData[idx].name = String.Format("asteroid_{0}_{1}", Info.AsteroidTypes[type], Info.AsteroidCategories[category]);
            idx++;
        }

        textureImporter.spritesheet = sliceMetaData;
        EditorUtility.SetDirty(textureImporter);
        textureImporter.SaveAndReimport();

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    }
}