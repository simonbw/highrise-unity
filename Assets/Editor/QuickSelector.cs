using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public abstract class QuickSelector<T> : EditorWindow {
  protected string searchString = "";
  protected int selectedIndex;
  [SerializeField]
  protected T[] items = new T[0];
  protected static readonly List<T> recentlyOpened = new List<T>();

  protected GUIStyle normalItemStyle;
  protected GUIStyle selectedItemStyle;
  protected int listSize = 10;


  // What to do when an item is selected
  protected abstract void OnSelect(T item);
  // Get the text for an item
  protected abstract string GetButtonText(T item);
  // Get the list of things to display
  protected abstract T[] GetItems();

  protected static void ShowWindow<C>(string windowTitle = "Select an item") where C : QuickSelector<T> {
    if (HasOpenInstances<C>()) {
      var window = GetWindow<C>();
      window.Close();
    } else {
      var window = GetWindow<C>();
      window.titleContent = new GUIContent(windowTitle);
      window.Show();
    }
  }

  private void MakeStyles() {
    var padding = new RectOffset(8, 8, 4, 4);
    normalItemStyle = new GUIStyle();
    normalItemStyle.fontSize = 20;
    normalItemStyle.padding = padding;
    normalItemStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
    normalItemStyle.hover.textColor = new Color(1, 1, 1, 0.7f);

    selectedItemStyle = new GUIStyle();
    selectedItemStyle.fontSize = 20;
    selectedItemStyle.padding = padding;
    selectedItemStyle.normal.textColor = new Color(1f, 1f, 1f);
  }


  void OnGUI() {
    HandleKeyPresses();
    MakeStyles();

    GUI.SetNextControlName("SearchBox");
    var newSearchString = EditorGUILayout.TextField("Search String", searchString);
    EditorGUI.FocusTextInControl("SearchBox");

    if (newSearchString != searchString || searchString == "" && !items.Any()) {
      selectedIndex = 0;
      searchString = newSearchString;
      items = GetItems();
    }


    GUILayout.Space(16);

    var numEntries = items.Length;
    selectedIndex = Mathf.Clamp(selectedIndex, 0, numEntries - 1);
    for (int i = 0; i < items.Length && i < numEntries; i++) {
      var item = items[i];
      var selected = i == selectedIndex;

      var rect = EditorGUILayout.BeginVertical();
      if (selected) {
        EditorGUI.DrawRect(rect, new Color(0.2f, 0.6f, 0.7f));
      } else {
        EditorGUI.DrawRect(rect, new Color(0.1f, 0.1f, 0.1f));
      }
      var style = selected ? selectedItemStyle : normalItemStyle;
      if (GUILayout.Button(GetButtonText(item), style)) {
        SelectItem(item);
        return;
      }
      EditorGUILayout.EndVertical();
    }
  }

  void HandleKeyPresses() {
    if (Event.current.type == EventType.KeyDown) {
      switch (Event.current.keyCode) {
        case KeyCode.DownArrow:
          selectedIndex += 1;
          Repaint();
          break;
        case KeyCode.UpArrow:
          selectedIndex -= 1;
          Repaint();
          break;
        case KeyCode.Return:
          SelectItem(items[selectedIndex]);
          return;
        case KeyCode.Escape:
          Close();
          return;
      }
    }
  }

  void SelectItem(T item) {
    OnSelect(item);
    recentlyOpened.Remove(item);
    recentlyOpened.Insert(0, item);
    GetWindow<QuickSelector<T>>().Close();
  }
}