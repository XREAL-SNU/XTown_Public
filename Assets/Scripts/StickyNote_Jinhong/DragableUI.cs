using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// �ݶ��̴��� �����ϴ� ���� �� ������Ʈ�� �巡�׷� �̵��� �� �ְ� �ϴ� Ŭ����
/// </summary>
public class DragableUI : MonoBehaviour
{
    private Vector3 _offset;
    private float _zCoord;
    private static PhotonTransformView _transformView;
    private static PhotonView _view;

    void Start()
    {
        _transformView = GetComponent<PhotonTransformView>();
        _view = GetComponent<PhotonView>();
        if(!StickyNoteNetworkManager.Instance.networked)
        {
            _transformView.enabled = false;
            _view.enabled = false;
        }
    }
    
    void OnMouseDown()
    {
        _zCoord = Camera.main.WorldToScreenPoint( transform.parent.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        _offset = transform.parent.transform.position - GetMouseAsWorldPoint();
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
        transform.parent.transform.position = GetMouseAsWorldPoint() + _offset;
    }
}