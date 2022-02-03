using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �ݶ��̴��� �����ϴ� ���� �� ������Ʈ�� �巡�׷� �̵��� �� �ְ� �ϴ� Ŭ����
/// </summary>
public class DragableUI : MonoBehaviour
{
    [SerializeField]
    private StickyNote _stickyNote;

    private Vector3 _offset;
    private float _zCoord;

    void OnMouseDown()
    {
        _zCoord = Camera.main.WorldToScreenPoint( gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        _offset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = _zCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        if (!_stickyNote.isLocked)
        {
            transform.position = GetMouseAsWorldPoint() + _offset;
        }
    }
}