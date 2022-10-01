using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor.SceneManagement;

public class SceneLoader : OdinEditorWindow
{
    [MenuItem("Tools/Scene loader")]
    private static void OpenWindow()
    {
        GetWindow<SceneLoader>().Show();
    }

    [HideInInspector]
    public bool isAdditive = false;
    [PropertyOrder(-9)]
    [Button("IsAdditive")]
    private void IsAdditive()
    {
        isAdditive = !isAdditive;
    }

    [PropertyOrder(-10)]
    [Button(ButtonSizes.Medium)]
    public void LoadScenes()
    {
        if(scenes == null)
        {
            scenes = new List<SceneHolder>();
        }
        scenes.Clear();
        string[] paths = AssetDatabase.FindAssets("l: Scene", new string[1]{ "Assets"});
        for (int i = 0; i < paths.Length; i++)
        {
            scenes.Add(new SceneHolder(this,AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(paths[i])), AssetDatabase.GUIDToAssetPath(paths[i])));
        }
        try
        {
            scenes.Sort((x, y) => x.sceneAsset.name.CompareTo(y.sceneAsset.name));
        }
        catch
        {
            LoadScenes();
        }
    }
    [TableList]
    public List<SceneHolder> scenes = new List<SceneHolder>();

    private void OnValidate()
    {
        LoadScenes();
    }
}

public class SceneHolder
{
    private SceneLoader sceneLoader = null;
    [HideInInspector]
    public SceneAsset sceneAsset;
    private string path = "";

    [GUIColor("IsValid")]
    [Button(ButtonSizes.Medium,Name ="@sceneAsset.name")]
    private void LoadScene()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(path, sceneLoader.isAdditive? OpenSceneMode.Additive : OpenSceneMode.Single);
    }

    private Color IsValid()
    {
        return path.Length > 0 ? sceneLoader.isAdditive? new Color(0.8f, 0.8f, 0.9f) : Color.white : Color.red;
    }

    public SceneHolder(SceneLoader sceneLoader, SceneAsset sceneAsset, string path)
    {
        this.sceneLoader = sceneLoader;
        this.sceneAsset = sceneAsset;
        this.path = path;
    }
}
