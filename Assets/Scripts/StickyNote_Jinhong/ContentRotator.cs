
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContentRotator : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private RectTransform _contentTransform;

    // �� ���� Ŭ���� ȸ���� �ʿ��� ���콺 �������� �� Ŀ��. 
    private float _rotationSensitivity = 1;

    // �� ���� ȸ���ϴ� ����
    private int _rotationAngle = 10;

    private int _minRotation = -60;
    private int _maxRotation = 60;

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.delta.x > _rotationSensitivity)
        {
            _contentTransform.eulerAngles = new Vector3(_contentTransform.eulerAngles.x, _contentTransform.eulerAngles.y - _rotationAngle, _contentTransform.eulerAngles.z);
        }
        else if (eventData.delta.x < -1 * _rotationSensitivity)
        {
            _contentTransform.eulerAngles = new Vector3(_contentTransform.eulerAngles.x, _contentTransform.eulerAngles.y + _rotationAngle, _contentTransform.eulerAngles.z);
        }
        // x�� ȸ���� ��¦ ������ �־ ����
        /*
        if (eventData.delta.y > _rotationSensitivity)
        {
            _contentTransform.eulerAngles = new Vector3(Mathf.Clamp(_contentTransform.eulerAngles.x + _rotationAngle, _minRotation, _maxRotation), _contentTransform.eulerAngles.y, _contentTransform.eulerAngles.z);
        }
        else if (eventData.delta.y < -1 * _rotationSensitivity)
        {
            _contentTransform.eulerAngles = new Vector3(Mathf.Clamp(_contentTransform.eulerAngles.x - _rotationAngle, _minRotation, _maxRotation), _contentTransform.eulerAngles.y, _contentTransform.eulerAngles.z);
        }
        */
    }
}

