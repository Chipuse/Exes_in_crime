using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cursor : MonoBehaviour
{
    public Camera cam;
    [Range(0, 2)]
    public float cursorHeight = 0.5f;
    public PositionKey mouseGridPos;
    public TMP_Text secLvl;
    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mouseGridPos = GetMousePositionOnMap();
        string tempText = "";
        if(InputManager._instance.currentMode == InputMode.deployUnits)
        {
            if(MapManager._instance.GetSecurityLevel(mouseGridPos) < 0)
            {
                tempText = "Deploy: " + GameDataManager._instance.characterToDeploy.data.name;
            }
        }
        else
        {
            tempText = "Security Lvl: " + MapManager._instance.GetSecurityLevel(mouseGridPos).ToString();
        }
        secLvl.text = tempText;
    }

    PositionKey GetMousePositionOnMap()
    {
        if (cam == null)
            return InvalidKey.Key;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (MapManager._instance.mapSurface.Raycast(ray, out RaycastHit hit, 20) && hit.collider.gameObject.layer == 20)
        {
            PositionKey temp = MapManager._instance.WorldPosToGroundGridPos(hit.point);
            transform.position = MapManager._instance.GroundGridPosToWorldPos(temp);
            transform.Translate(Vector3.up * cursorHeight, Space.World);
            return temp;
        }
        return InvalidKey.Key;
    }
}
