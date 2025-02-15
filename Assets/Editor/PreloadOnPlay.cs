#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class PreloadOnPlay
{
    static PreloadOnPlay()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
    }
}

#endif