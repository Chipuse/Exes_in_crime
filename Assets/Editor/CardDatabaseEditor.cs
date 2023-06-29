using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardDatabase))]
public class CardDatabaseEditor : Editor
{

    SerializedProperty cardDataProp;
    SerializedProperty cardObjsProp;
    SerializedProperty gunProp;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        cardDataProp = serializedObject.FindProperty("cards");
        cardObjsProp = serializedObject.FindProperty("cardObjs");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("H'llo! this is the card databse object. Please refresh this when changes towards the card data list are happening", MessageType.Info);
        DrawDefaultInspector();
        CardDatabase myScript = (CardDatabase)target;
        if (GUILayout.Button("Reload Card Data"))
        {
            myScript.ReloadCardData();
        }
        serializedObject.ApplyModifiedProperties();
    }


    /*
    [MenuItem("Tools/ReloadCardData")]
    private static void ReloadCardData()
    {
        cards = CardTester.FetchCardListStatic();
    }
    */
}
