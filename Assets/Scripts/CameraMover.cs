using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public static CameraMover _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }
    public float minHeight = 1f;
    public float maxHeight = 15f;
    public float scrollSpeed = 1;

    public float dragSpeed = 2;
    public float autoSpeed = 2;
    private Vector3 dragOrigin;
    public  bool auto = false;
    PositionKey autoTarget = new PositionKey { x = 0, y = 0 };
    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.y -= Input.mouseScrollDelta.y * scrollSpeed;
        if (newPos.y < minHeight)
            newPos.y = minHeight;
        if (newPos.y > maxHeight)
            newPos.y = maxHeight;
        transform.position = newPos;
        if(auto)
        {
            if(CameraGridFocus() == autoTarget)
            {
                auto = false;
            }
            else
            {
                Vector3 dir = new Vector3((float)autoTarget.x - (float)CameraGridFocus().x, 0, (float)autoTarget.y - (float)CameraGridFocus().y);
                transform.Translate(dir.normalized * autoSpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(2))
            {
                dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(2)) return;

            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

            transform.Translate(move, Space.World);
        }        
    }

    public void MoveCamera(PositionKey _newTarget)
    {
        auto = true;
        autoTarget = _newTarget;
    }

    PositionKey CameraGridFocus()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (MapManager._instance.mapSurface.Raycast(ray, out RaycastHit hit, 20) && hit.collider.gameObject.layer == 20)
        {
            PositionKey temp = MapManager._instance.WorldPosToGroundGridPos(hit.point);
            return temp;
        }
        return new PositionKey { x = 0, y = 0 };
    }
}
