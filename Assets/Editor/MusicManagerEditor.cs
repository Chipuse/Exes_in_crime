using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MusicManager))]
public class MusicManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MusicManager myScript = (MusicManager)target;
        if (GUILayout.Button("Reset AudioSources"))
        {
            myScript.UpdateSounds();
        }
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
