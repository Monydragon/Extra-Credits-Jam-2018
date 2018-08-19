using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Barrel))]
public class BarrelEditor : Editor
{
    Barrel b;

    void OnEnable()
    {
        b = (Barrel)target;
    }

    void OnSceneGUI()
    {
        Handles.DrawWireDisc(b.transform.position, Vector3.forward, b.effectRadius);
    }
}
