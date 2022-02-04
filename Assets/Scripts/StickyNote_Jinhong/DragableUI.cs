using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

/// <summary>
/// �ݶ��̴��� �����ϴ� ���� �� ������Ʈ�� �巡�׷� �̵��� �� �ְ� �ϴ� Ŭ����
/// </summary>
public class DragableUI : MonoBehaviour
{
    [SerializeField]
    private StickyNote _stickyNote;
    private Vector3 _offset;
    private float _zCoord;
    private static PhotonView _view;

    void Start()
    {
        _view = GetComponent<PhotonView>();
        if(!StickyNoteNetworkManager.Instance.networked)
        {
            _view.enabled = false;
        }
    }
    
    void OnMouseDown()
    {
        _zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
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
        if(!_stickyNote.isLocked)
        {
            if(!StickyNoteNetworkManager.Instance.networked)
            {
                moveUI();
            }
            else
            {
                _view.RPC("moveUI",RpcTarget.All);
            } 
        }
    }
    [PunRPC]
    void moveUI()
    {
        transform.position = GetMouseAsWorldPoint() + _offset;
    }
}