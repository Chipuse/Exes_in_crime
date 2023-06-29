using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class DialogEditorWindow : EditorWindow
{
    [MenuItem("Tools/DialogEditor")]
    public static void OpenMapEditor()
    {
        GetWindow<DialogEditorWindow>();
    }
    private void OnEnable()
    {
        SceneView.duringSceneGui += DuringSceneGUI;

        Undo.undoRedoPerformed += OnUndoRedo;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
        Undo.undoRedoPerformed -= OnUndoRedo;
    }

    public DialogObject _fallbackData;
    SerializedProperty _dialogData;

    public string _newFileName = "new Filename";
    SerializedProperty _newFileNameProp;

    Vector2 scrollPosition;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, 25));
        SerializedObject serializedObjectConversation = null;
        if (!ConversationManager._instance)
        {
            //EditorGUILayout.HelpBox("No ConversationManager Found!", MessageType.Error);
            serializedObjectConversation = new SerializedObject(this);
            _dialogData = serializedObjectConversation.FindProperty("_fallbackData");
        }
        else
        {
            serializedObjectConversation = new SerializedObject(ConversationManager._instance);
            _dialogData = serializedObjectConversation.FindProperty("currentDialog");
        }        
        EditorGUILayout.PropertyField(_dialogData, new GUIContent());
        if (serializedObjectConversation != null)
        {
            serializedObjectConversation.ApplyModifiedProperties();
        }
        GUILayout.EndArea();
        if (_fallbackData == null && (ConversationManager._instance == null || ConversationManager._instance.currentDialog == null))
            return;
        PerformDialogObjectEdit();
    }

    void PerformDialogObjectEdit()
    {
        DialogObject tempObj = null;
        if (ConversationManager._instance)
        {
            tempObj = ConversationManager._instance.currentDialog;
        }
        else
        {
            tempObj = _fallbackData;
        }
        //if (!ConversationManager._instance)
        //{
        //    Undo.RecordObject(ConversationManager._instance.currentDialog, "Edited Dialog");
        //}
        //else
        //{
        //    Undo.RecordObject(_fallbackData, "Edited Dialog");
        //}
        GUILayout.BeginArea(new Rect(0, 25, Screen.width, 25));
        EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("+ new Conversation", "Button"))
        {
            EditorUtility.SetDirty(tempObj);
            SaveAssets();
            //tool = ToolPicker.Nothing;
        }
        SerializedObject serializedNewFileName = new SerializedObject(this); ;
        _newFileNameProp = serializedNewFileName.FindProperty("_newFileName");
        EditorGUILayout.PropertyField(_newFileNameProp, new GUIContent());
        serializedNewFileName.ApplyModifiedProperties();
        if (GUILayout.Button("Save", "Button"))
        {
            EditorUtility.SetDirty(tempObj);
            SaveAssets();
        }
        if (GUILayout.Button("Delete", "Button"))
        {
            //tool = ToolPicker.Nothing;
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(0, 50, Screen.width, Screen.height - 100));
        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, true, true, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 100));
        for (int i = 0; i < tempObj.Textboxen.Count; i++)
        {
            //GUILayout.BeginArea(new Rect(0, 100 + 50 * i + scrollPosition.y, Screen.width, 50));
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            TextBox tempBox = tempObj.Textboxen[i];
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Speaker", GUILayout.Width(50));
            tempBox.Speaker = (DialogChar)EditorGUILayout.EnumPopup("", tempBox.Speaker, GUILayout.Height(50), GUILayout.Width(50));
            EditorGUILayout.EndVertical();

            //ToDo item picker for folder for the audiosource! for some reason this works now lol
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Voiceline", GUILayout.Width(60));
            tempBox.Voiceline = (AudioClip)EditorGUILayout.ObjectField(tempBox.Voiceline, objType: typeof(AudioClip), GUILayout.Width(60));
            EditorGUILayout.EndVertical();

            //EditorGUILayout.HelpBox("Voiceline", MessageType.Error);

            GUIStyle style = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            tempBox.Text = EditorGUILayout.TextArea(tempBox.Text, style, GUILayout.Height(50), GUILayout.Width(Screen.width - 320));
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Image L", GUILayout.Width(50));
            tempBox.ImageL = (DialogChar)EditorGUILayout.EnumPopup("", tempBox.ImageL, GUILayout.Height(50), GUILayout.Width(50));
            EditorGUILayout.EndVertical();
            tempBox.FocusL = EditorGUILayout.Toggle(tempBox.FocusL, GUILayout.Height(50), GUILayout.Width(20));
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Image R", GUILayout.Width(50));
            tempBox.ImageR = (DialogChar)EditorGUILayout.EnumPopup("", tempBox.ImageR, GUILayout.Height(50), GUILayout.Width(50));
            EditorGUILayout.EndVertical();
            tempBox.FocusR = EditorGUILayout.Toggle(tempBox.FocusR, GUILayout.Height(50), GUILayout.Width(20));
            tempObj.Textboxen[i] = tempBox;

            EditorGUILayout.BeginVertical();
            if(i > 0)
            {
                if (GUILayout.Button("^", "Button", GUILayout.Width(25)))
                {
                    tempObj.Textboxen.Remove(tempBox);
                    tempObj.Textboxen.Insert(i - 1, tempBox);
                    EditorUtility.SetDirty(tempObj);
                    SaveAssets();
                    break;
                }
            }
            else
            {
                EditorGUILayout.LabelField(" ^", GUILayout.Width(25));
            }
            if(i < tempObj.Textboxen.Count - 1)
            {
                if (GUILayout.Button("v", "Button", GUILayout.Width(25)))
                {
                    tempObj.Textboxen.Remove(tempBox);
                    tempObj.Textboxen.Insert(i + 1, tempBox);
                    SaveAssets();
                    break;
                }
            }
            else
            {
                EditorGUILayout.LabelField(" v", GUILayout.Width(25));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("-!!!-", GUILayout.Width(25));
            if (GUILayout.Button("X", "Button", GUILayout.Width(25)))
            {
                tempObj.Textboxen.Remove(tempBox);
                EditorUtility.SetDirty(tempObj);
                SaveAssets();
                break;
            }
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            //GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(0, Screen.height - 50, Screen.width, 50));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+ New TextBox", "Button"  ))
        {
            if(tempObj.Textboxen.Count > 0)
            {
                tempObj.Textboxen.Add(new TextBox { 
                    ImageL = tempObj.Textboxen[tempObj.Textboxen.Count - 1].ImageL, 
                    ImageR = tempObj.Textboxen[tempObj.Textboxen.Count - 1].ImageR,
                    Speaker = tempObj.Textboxen[tempObj.Textboxen.Count - 1].Speaker,
                    FocusL = tempObj.Textboxen[tempObj.Textboxen.Count - 1].FocusL,
                    FocusR = tempObj.Textboxen[tempObj.Textboxen.Count - 1].FocusR
                });
            }
            else
            {
                tempObj.Textboxen.Add(new TextBox {Text = "put text here" });
            }
            EditorUtility.SetDirty(tempObj);
            SaveAssets();
        }
        if (ConversationManager._instance != null)
        {
            if(GUILayout.Button("Try Conversation", "Button"))
            {
                ConversationManager._instance.StartConversation(ConversationManager._instance.currentDialog);
                EditorUtility.SetDirty(tempObj);
                SaveAssets();
            }
        }

        if (GUILayout.Button("Save", "Button"))
        {
            EditorUtility.SetDirty(tempObj);
            SaveAssets();
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        EditorApplication.QueuePlayerLoopUpdate();
    }
    //first buttons for new conversation object and to save it / maybe delete it


    //foreach textbox: -> char picker, voice line picker -> maybe with file browser like in mapeditor for tiles, char picker, checkbox for focus, char picker, checkbox for focus
    //maybe button to delete textbox?
    //button to add new textbox

    //AssetDatabase.SaveAssets();





    void DuringSceneGUI(SceneView sceneView)
    { 
        
    }

    void OnUndoRedo()
    {
        if (ConversationManager._instance)
        {
            EditorUtility.SetDirty(ConversationManager._instance.currentDialog);
        }
        else if(_fallbackData != null)
        {
            EditorUtility.SetDirty(_fallbackData);
        }
        AssetDatabase.SaveAssets();
    }

    void SaveAssets()
    {
        AssetDatabase.SaveAssets();
    }
}
