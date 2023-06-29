using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(MapData))]
public class MapDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("H'llo! NVM", MessageType.Info);
    }
}
