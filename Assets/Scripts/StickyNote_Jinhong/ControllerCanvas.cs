using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class ControllerCanvas : MonoBehaviour
{
    private StickyNote _stickyNote;

    [SerializeField]
    private Image _background;
    [SerializeField]
    private Button _editButton;
    [SerializeField]
    private Button _scaleButton;
    [SerializeField]
    private Button _rotateButton;
    [SerializeField]
    private Button _removeButton;

    private bool _hovering;


    void Start()
    {
        // �̺�Ʈ Ʈ���ſ� ���콺 ������ Enter/Exit �Լ� ���ε�
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        pointerExitEntry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        trigger.triggers.Add(pointerEnterEntry);
        trigger.triggers.Add(pointerExitEntry);

        // Edit ��ư�� ���� ���� �Լ� ���ε�
        _editButton.onClick.AddListener(OnClick_Edit);

        // Remove ��ư�� ��ƼŰ��Ʈ ���� �Լ� ���ε�
        _removeButton.onClick.AddListener(OnClick_Remove);

        // Scale ��ư �̺�Ʈ Ʈ���ſ� ������ �Լ� ���ε�
        EventTrigger scaleTrigger = _scaleButton.GetComponent<EventTrigger>();
        EventTrigger.Entry scaleEntry = new EventTrigger.Entry();
        scaleEntry.eventID = EventTriggerType.Drag;
        scaleEntry.callback.AddListener((data) => { OnDrag_Scale((PointerEventData)data); });
        scaleTrigger.triggers.Add(scaleEntry);

        // Rotate ��ư �̺�Ʈ Ʈ���ſ� ȸ�� �Լ� ���ε�
        EventTrigger rotateTrigger = _rotateButton.GetComponent<EventTrigger>();
        EventTrigger.Entry rotateEntry = new EventTrigger.Entry();
        rotateEntry.eventID = EventTriggerType.Drag;
        rotateEntry.callback.AddListener((data) => { OnDrag_Rotate((PointerEventData)data); });
        rotateTrigger.triggers.Add(rotateEntry);

        _background.transform.DOScale(0, 0);
        _hovering = false;
    }

    void Update()
    {
        _background.transform.position = Camera.main.WorldToScreenPoint(_stickyNote.ContentCanvas.ControllerTarget.position);
    }

    public void Initialize(StickyNote stickyNote)
    {
        _stickyNote = stickyNote;
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ� UI�� ����� �ڷ�ƾ
    public IEnumerator HideController()
    {
        yield return new WaitForSeconds(0.5f);

        // ���콺 �����Ͱ� ��ƼŰ��Ʈ ��Ʈ�ѷ��� ��ƼŰ��Ʈ ��ü���� ����� ���, ��ƼŰ��Ʈ ��Ʈ�ѷ��� ����
        if (_hovering || _stickyNote.ContentCanvas.hovering)
        {
            yield return null;
        }
        else
        {
            _background.transform.DOScale(0, 0.4f);
            _stickyNote.ContentCanvas.HideColorIcon();
        }
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� ���콺 �����Ͱ� ������ ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��κ��� ���콺 �����Ͱ� ������ ��
    private void OnPointerExit(PointerEventData eventData)
    {
        _hovering = false;
        StartCoroutine(HideController());
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� Scale ��ư�� �巡������ ��
    public void OnDrag_Scale(PointerEventData eventData)
    {
        _stickyNote.ContentCanvas.Scale(eventData.delta);
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� Rotate ��ư�� �巡������ ��
    public void OnDrag_Rotate(PointerEventData eventData)
    {
        _stickyNote.ContentCanvas.Rotate(eventData.delta);
    }


    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� Edit ��ư�� ������ ��
    public void OnClick_Edit()
    {
        _stickyNote.EditCanvas.Show();
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� Remove ��ư�� ������ ��
    public void OnClick_Remove()
    {
        Destroy(_stickyNote.gameObject);
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ� UI�� ��Ÿ���� �ϴ� �Լ�
    public void ShowController()
    {
        _background.transform.DOScale(1, 0.4f);
    }
}
