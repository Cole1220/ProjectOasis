using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorInspector : Editor
{
    private MeshGenerator creator;

    private void OnEnable()
    {
        creator = target as MeshGenerator;
        Undo.undoRedoPerformed += RefreshCreator;
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= RefreshCreator;
    }

    private void RefreshCreator()
    {
        if (Application.isPlaying)
        {
            creator.Refresh();
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
        {
            RefreshCreator();
        }
    }
}
