using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemInstance))]
public class ItemCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set Item"))
            ((ItemInstance)target).SetItem();
    }
}
