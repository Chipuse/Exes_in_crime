using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class MapEditorWindow : EditorWindow
{
    enum ToolPicker
    {
        Nothing,
        GroundPainter,
        GroundRotator,
        WallPainter,
        WallRotator,
        SecLvlPaint
    }

    SerializedProperty _mapData;

    private ToolPicker tool = ToolPicker.GroundPainter;

    [MenuItem("Tools/MapEditor")]
    public static void OpenMapEditor()
    {
        GetWindow<MapEditorWindow>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += DuringSceneGUI;

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    SerializedObject so;
    SerializedProperty propThing;
    int lvl = 1;
    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
        Undo.undoRedoPerformed -= OnUndoRedo;
    }

    private void OnGUI()
    {
        if (!MapManager._instance)
        {
            EditorGUILayout.HelpBox("No MapManager Found!", MessageType.Error);
            return;
        }


        var serializedObjectMapManager = new SerializedObject(MapManager._instance);
        _mapData = serializedObjectMapManager.FindProperty("_mapData");
        EditorGUILayout.PropertyField(_mapData, new GUIContent());
        serializedObjectMapManager.ApplyModifiedProperties();
        if (GUILayout.Button("Init MapData"))
        {
            MapManager activeMapmanager = FindObjectOfType<MapManager>();
            if(activeMapmanager != null)
            {
                if (MapManager._instance != activeMapmanager)
                {
                    DestroyImmediate(MapManager._instance);
                    MapManager._instance = activeMapmanager;
                }
            }
            MapManager._instance.mapInstance = new MapInstance(MapManager._instance._mapData);
        }

        EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Clear", "ToolbarButton", GUILayout.Width(45f)))
        {
            tool = ToolPicker.Nothing;
        }
        // Create space between Clear and Collapse button.
        //GUILayout.Space(5f);
        // Create toggles button.
        if (GUILayout.Toggle(tool == ToolPicker.GroundPainter, "GroundPainter", "ToolbarButton"))
        {
            tool = ToolPicker.GroundPainter;
        }
        /*
        if (GUILayout.Toggle(tool == ToolPicker.GroundRotator, "GroundRotator", "ToolbarButton"))
        {
            tool = ToolPicker.GroundRotator;
        }*/
        if (GUILayout.Toggle(tool == ToolPicker.WallPainter, "WallPainter", "ToolbarButton"))
        {
            tool = ToolPicker.WallPainter;
        }
        /*
        if (GUILayout.Toggle(tool == ToolPicker.WallRotator, "WallRotator", "ToolbarButton"))
        {
            tool = ToolPicker.WallRotator;
        }*/
        if (GUILayout.Toggle(tool == ToolPicker.SecLvlPaint, "SecLvlPainter", "ToolbarButton"))
        {
            tool = ToolPicker.SecLvlPaint;
        }
        // Push content to be what they should be. (ex. width)
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (tool == ToolPicker.GroundPainter || tool == ToolPicker.GroundRotator)
        {
            //picker for grounds
        }
        if (tool == ToolPicker.WallRotator || tool == ToolPicker.WallPainter)
        {
            //picker for walls
        }

        GUILayout.Label(lastSelectedGround);
        GUILayout.Label(lastSelectedWall);

        //picker menu item
        AssetFolderItemPicker();
        lvl = EditorGUILayout.IntField("Security Level:", lvl);
    }

    private int _selectedGround = -1;
    private int _selectedWall = -1;
    private int _selected 
    { 
        get
        {
            if (tool == ToolPicker.GroundPainter || tool == ToolPicker.GroundRotator)
            {
                return _selectedGround;
            }
            if (tool == ToolPicker.WallRotator || tool == ToolPicker.WallPainter)
            {
                return _selectedWall;
            }
            else
                return -1;
        }
        set
        {
            if (tool == ToolPicker.GroundPainter || tool == ToolPicker.GroundRotator)
            {
                _selectedGround = value;
            }
            if (tool == ToolPicker.WallRotator || tool == ToolPicker.WallPainter)
            {
                _selectedWall = value;
            }
        }
    }
    private string lastSelectedWall = "";
    private string lastSelectedGround = "";
    public string SelectedDisplay
    {
        get
        {
            if (_selected < 0 || _selected >= paths.Length)
            {
                return "NOTHING SELECTED";
            }
            string withEnding =  Selected.Split(new char[] { '/', '\\' }).Last();
            return withEnding.Split(new char[] { '.' })[0];
        }
    }
    public string Selected
    {
        get
        {
            if (_selected < 0 || _selected >= paths.Length)
            {
                return string.Empty;
            }
            return paths[_selected];
        }
    }

    public void AssetFolderItemPicker()
    {
        if (GUILayout.Button(SelectedDisplay))
        {
            GenericMenu dropdown = new GenericMenu();
            for (int i = 0; i < paths.Length; i++)
            {
                if (tool == ToolPicker.GroundPainter || tool == ToolPicker.GroundRotator)
                {
                    if (paths[i].StartsWith("Assets/Resources/Tiles/"))
                    {
                        dropdown.AddItem(
                            //Add the assetpath minus the "Asset/"-part
                            new GUIContent(paths[i].Remove(0, 17)),
                            //show the currently selected item as selected
                            i == _selected,
                            //lambda to set the selected item to the one being clicked
                            selectedIndex => _selected = (int)selectedIndex,
                            //index of this menu item, passed on to the lambda when pressed.
                            i
                       );
                    }
                }
                if (tool == ToolPicker.WallRotator || tool == ToolPicker.WallPainter)
                {
                    if (paths[i].StartsWith("Assets/Resources/Walls/"))
                    {
                        dropdown.AddItem(
                            //Add the assetpath minus the "Asset/"-part
                            new GUIContent(paths[i].Remove(0, 17)),
                            //show the currently selected item as selected
                            i == _selected,
                            //lambda to set the selected item to the one being clicked
                            selectedIndex => _selected = (int)selectedIndex,
                            //index of this menu item, passed on to the lambda when pressed.
                            i
                       );
                    }
                }
            }
            dropdown.ShowAsContext(); //finally show the dropdown
        }
        if (_selected >= 0 && _selected < paths.Length)
        {
            Object selectedAsset = AssetDatabase.LoadAssetAtPath(Selected, typeof(GameObject));
            Texture t = AssetPreview.GetAssetPreview(selectedAsset);
            if (t != null)
                GUI.DrawTexture(GUILayoutUtility.GetRect(200, 200), t);
            if(selectedAsset != null)
            {
                if (tool == ToolPicker.GroundPainter || tool == ToolPicker.GroundRotator)
                {
                    lastSelectedGround = Selected;
                }
                if (tool == ToolPicker.WallRotator || tool == ToolPicker.WallPainter)
                {
                    lastSelectedWall = Selected;
                }
            }       
        }
    }

    string[] paths;
    private void OnFocus()
    {
        paths = AssetDatabase.GetAllAssetPaths();
    }


    void DuringSceneGUI(SceneView sceneView)
    {
        switch (tool)
        {
            case ToolPicker.Nothing:
                ShowTilePosition(sceneView);
                break;
            case ToolPicker.GroundPainter:
                DoGroundPaint(sceneView);
                break;
            case ToolPicker.GroundRotator:
                DoGroundRotate(sceneView);
                break;
            case ToolPicker.WallPainter:
                DoWallPaint(sceneView);
                break;
            case ToolPicker.WallRotator:
                DoWallRotate(sceneView);
                break;
            case ToolPicker.SecLvlPaint:
                DoSecPaint(sceneView);
                break;
            default:
                break;
        }        
    }

    void ShowTilePosition(SceneView sceneView)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (MapManager._instance.mapSurface.Raycast(ray, out RaycastHit hit, 200) && hit.collider.gameObject.layer == 20)
        {
            Handles.color = Color.red;
            Vector3 groundPos = MapManager._instance.GroundGridPosToWorldPos(MapManager._instance.WorldPosToGroundGridPos(hit.point));
            groundPos.y += 0.5f;
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                padding = new RectOffset(),
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };
            style.normal.textColor = Color.black;
            foreach (var item in MapManager._instance._mapData.tiles)
            {
                Handles.Label(MapManager._instance.GroundGridPosToWorldPos(item.positionKey) - new Vector3(0.15f, 0, 0) + Vector3.up * 0.5f, item.positionKey.x.ToString() + "|" + item.positionKey.y.ToString(), style);
            }
            style.normal.textColor = Color.white;
            foreach (var item in MapManager._instance._mapData.tiles)
            {
                Handles.Label(MapManager._instance.GroundGridPosToWorldPos(item.positionKey) - new Vector3(0.05f, 0, 0) + Vector3.up * 0.5f, item.positionKey.x.ToString() + "|" + item.positionKey.y.ToString(), style);
            }
        }
    }

    void DoGroundPaint(SceneView sceneView)
    {
        Event e = Event.current;
        Vector3 mousePos = e.mousePosition;

        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = sceneView.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (MapManager._instance.mapSurface.Raycast(ray, out RaycastHit hit, 200) && hit.collider.gameObject.layer == 20)
        {
            Handles.color = Color.red;
            Vector3 groundPos = MapManager._instance.GroundGridPosToWorldPos(MapManager._instance.WorldPosToGroundGridPos(hit.point));
            groundPos.y += 0.5f;
            Handles.DrawAAPolyLine(10, groundPos, groundPos + hit.normal);            
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    string tempPath = lastSelectedGround.Remove(0, 17); //resources folder must be directly in Asset folder!!!
                    tempPath = tempPath.Split(new char[] { '.' })[0];
                    PositionKey tempKey = MapManager._instance.WorldPosToGroundGridPos(hit.point);
                    Undo.RecordObject(MapManager._instance._mapData, "Placed Tile");
                    MapManager._instance.mapInstance.RefreshTile(new SerializableData { assetPath = tempPath, direction = Direction.North, positionKey = tempKey });

                    for (int i = 0; i < MapManager._instance._mapData.tiles.Count; i++)
                    {
                        if (MapManager._instance._mapData.tiles[i].positionKey.Equals(tempKey))
                        {
                            MapManager._instance._mapData.tiles.RemoveAt(i);
                            break;
                        }
                    }
                    MapManager._instance._mapData.tiles.Add(new SerializableData { positionKey = tempKey, assetPath = tempPath, direction = Direction.North });

                    // Now flag the object as "dirty" in the editor so it will be saved
                    //! it seems that undo already does all the Dirty stuff...
                    //EditorUtility.SetDirty(MapManager._instance._mapData);
                    
                    //Save scriptable object back to filesystem
                    AssetDatabase.SaveAssets();
                    

                    e.Use();
                }
                else if (e.button == 1)
                {
                    Undo.RecordObject(MapManager._instance._mapData, "Removed Tile");
                    PositionKey tempKey = MapManager._instance.WorldPosToGroundGridPos(hit.point);
                    MapManager._instance.mapInstance.DeleteTile(tempKey);
                    for (int i = 0; i < MapManager._instance._mapData.tiles.Count; i++)
                    {
                        if (MapManager._instance._mapData.tiles[i].positionKey.Equals(tempKey))
                        {
                            MapManager._instance._mapData.tiles.RemoveAt(i);
                            break;
                        }
                    }
                    AssetDatabase.SaveAssets();
                    e.Use();
                }
            }
        }
        EditorApplication.QueuePlayerLoopUpdate();
    }

    void DoGroundRotate(SceneView sceneView)
    {

    }

    void DoWallPaint(SceneView sceneView)
    {
        Event e = Event.current;
        Vector3 mousePos = e.mousePosition;

        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = sceneView.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (MapManager._instance.mapSurface.Raycast(ray, out RaycastHit hit, 200) && hit.collider.gameObject.layer == 20)
        {
            Handles.color = Color.blue;
            Vector3 wallPos = MapManager._instance.WallGridPosToWorldPos(MapManager._instance.WorldPosToWallGridPos(hit.point));
            Handles.DrawAAPolyLine(10, wallPos, wallPos + hit.normal);
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    string tempPath = lastSelectedWall.Remove(0, 17); //resources folder must be directly in Asset folder!!!
                    tempPath = tempPath.Split(new char[] { '.' })[0];
                    PositionKey tempKey = MapManager._instance.WorldPosToWallGridPos(hit.point);
                    Undo.RecordObject(MapManager._instance._mapData, "Placed Wall");
                    MapManager._instance.mapInstance.RefreshWall(new SerializableData { assetPath = tempPath, direction = Direction.North, positionKey = tempKey });

                    for (int i = 0; i < MapManager._instance._mapData.walls.Count; i++)
                    {
                        if (MapManager._instance._mapData.walls[i].positionKey.Equals(tempKey))
                        {
                            MapManager._instance._mapData.walls.RemoveAt(i);
                            break;
                        }
                    }
                    MapManager._instance._mapData.walls.Add(new SerializableData { positionKey = tempKey, assetPath = tempPath, direction = Direction.North });

                    EditorUtility.SetDirty(MapManager._instance._mapData);
                    AssetDatabase.SaveAssets();


                    e.Use();
                }
                else if (e.button == 1)
                {
                    Undo.RecordObject(MapManager._instance._mapData, "Removed Wall");
                    PositionKey tempKey = MapManager._instance.WorldPosToWallGridPos(hit.point);
                    MapManager._instance.mapInstance.DeleteWall(tempKey);
                    for (int i = 0; i < MapManager._instance._mapData.walls.Count; i++)
                    {
                        if (MapManager._instance._mapData.walls[i].positionKey.Equals(tempKey))
                        {
                            MapManager._instance._mapData.walls.RemoveAt(i);
                            break;
                        }
                    }
                    EditorUtility.SetDirty(MapManager._instance._mapData);
                    AssetDatabase.SaveAssets();
                    e.Use();
                }
            }
        }
        EditorApplication.QueuePlayerLoopUpdate();
    }

    void DoWallRotate(SceneView sceneView)
    {

    }

    void DoSecPaint(SceneView sceneView)
    {
        Event e = Event.current;
        Vector3 mousePos = e.mousePosition;

        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = sceneView.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (MapManager._instance.mapSurface.Raycast(ray, out RaycastHit hit, 200) && hit.collider.gameObject.layer == 20)
        {
            Handles.color = Color.blue;
            Vector3 tilePos = MapManager._instance.GroundGridPosToWorldPos(MapManager._instance.WorldPosToGroundGridPos(hit.point));
            Handles.DrawAAPolyLine(10, tilePos, tilePos + hit.normal);
            if (e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {                    
                    PositionKey tempKey = MapManager._instance.WorldPosToGroundGridPos(hit.point);
                    Undo.RecordObject(MapManager._instance._mapData, "Placed SecLevel");
                    MapManager._instance.mapInstance.RefreshSecurityLvl(new SecurityLevelData {positionKey = tempKey, level = lvl });

                    for (int i = 0; i < MapManager._instance._mapData.secLvl.Count; i++)
                    {
                        if (MapManager._instance._mapData.secLvl[i].positionKey.Equals(tempKey))
                        {
                            MapManager._instance._mapData.secLvl.RemoveAt(i);
                            break;
                        }
                    }
                    MapManager._instance._mapData.secLvl.Add(new SecurityLevelData { positionKey = tempKey, level = lvl });

                    EditorUtility.SetDirty(MapManager._instance._mapData);
                    AssetDatabase.SaveAssets();

                    e.Use();
                }
                else if (e.button == 1)
                {
                    Undo.RecordObject(MapManager._instance._mapData, "Removed SecLevel");
                    PositionKey tempKey = MapManager._instance.WorldPosToGroundGridPos(hit.point);
                    MapManager._instance.mapInstance.DeleteSecurityLvl(tempKey);
                    for (int i = 0; i < MapManager._instance._mapData.secLvl.Count; i++)
                    {
                        if (MapManager._instance._mapData.secLvl[i].positionKey.Equals(tempKey))
                        {
                            MapManager._instance._mapData.secLvl.RemoveAt(i);
                            break;
                        }
                    }
                    EditorUtility.SetDirty(MapManager._instance._mapData);
                    AssetDatabase.SaveAssets();
                    e.Use();
                }
            }
        }
        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            padding = new RectOffset(),
            fontSize = 15,
            fontStyle = FontStyle.Bold
        };
        style.normal.textColor = Color.black;
        foreach (var item in MapManager._instance._mapData.secLvl)
        {
            Handles.Label(MapManager._instance.GroundGridPosToWorldPos(item.positionKey) - new Vector3(0.05f, 0, 0), item.level.ToString(), style);
        }
        style.normal.textColor = Color.white;
        foreach (var item in MapManager._instance._mapData.secLvl)
        {
            Handles.Label(MapManager._instance.GroundGridPosToWorldPos(item.positionKey) + new Vector3(0.05f,0,0), item.level.ToString(), style);
        }

        EditorApplication.QueuePlayerLoopUpdate();
    }

    void OnUndoRedo()
    {
        if (MapManager._instance)
        {
            if(MapManager._instance.mapInstance != null)
            {
                EditorUtility.SetDirty(MapManager._instance._mapData);
                MapManager._instance.mapInstance.RefreshMap();
                AssetDatabase.SaveAssets();
            }
        }
    }

}
