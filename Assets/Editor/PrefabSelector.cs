using System.Linq;
using UnityEditor;
using UnityEngine;


public class PrefabSelector : QuickSelector<GameObject> {
  [MenuItem("Window/Prefab Selector %o")]
  private static void ShowPrefabSelector() {
    ShowWindow<PrefabSelector>("Select a Prefab");
  }

  protected override void OnSelect(GameObject item) {
    Selection.activeGameObject = item;
    AssetDatabase.OpenAsset(item);
  }

  protected override string GetButtonText(GameObject item) {
    return item.name;
  }

  private static readonly string[] folders = new string[] { "Assets/Prefabs" };
  protected override GameObject[] GetItems() {
    return AssetDatabase.FindAssets($"{searchString} t:Prefab", folders)
        .Select(AssetDatabase.GUIDToAssetPath)
        .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
        .OrderBy(prefab => {
          var index = recentlyOpened.IndexOf(prefab);
          if (index == -1)
            return 9999999;
          else
            return index;
        })
        .Take(listSize)
        .ToArray();
  }

}