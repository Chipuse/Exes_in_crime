using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SoundManager myScript = (SoundManager)target;
        if (GUILayout.Button("Soft Load Sounds"))
        {
            myScript.UpdateSounds();
        }
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();
    }
}
