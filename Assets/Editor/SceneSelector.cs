using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;


public class SceneSelector : QuickSelector<string> {

  [MenuItem("Window/Scene Selector %&o")]
  private static void ShowSceneSelector() {
    ShowWindow<SceneSelector>("Select a Scene");
  }

  override protected void OnSelect(string scene) {
    EditorSceneManager.OpenScene(scene);
  }

  protected override string GetButtonText(string path) {
    return path.Split('.', '/').Reverse().Skip(1).First();
  }

  private static readonly string[] folders = new string[] { "Assets/Scenes" };
  protected override string[] GetItems() {
    return AssetDatabase.FindAssets($"{searchString} t:Scene", folders)
        .Select(AssetDatabase.GUIDToAssetPath)
        .OrderBy(prefab => {
          var index = recentlyOpened.IndexOf(prefab);
          if (index == -1)
            return int.MaxValue;
          else
            return index;
        })
        .Take(listSize)
        .ToArray();
  }
}