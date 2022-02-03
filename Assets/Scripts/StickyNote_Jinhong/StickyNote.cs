using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using DG.Tweening;
using TMPro;


public enum StickyNoteState
{
    Edit,
    Move,
    Idle
}

public class StickyNote : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField]
    private ContentCanvas _contentCanvas;
    public ContentCanvas ContentCanvas { get { return _contentCanvas; } }
    [SerializeField]
    private ControllerCanvas _controllerCanvas;
    public ControllerCanvas ControllerCanvas { get { return _controllerCanvas; } }
    [SerializeField]
    private EditCanvas _editCanvas;
    public EditCanvas EditCanvas { get { return _editCanvas; } }
    private bool _isEditing;
    public bool isEditing{get { return _isEditing; } }
    private bool _isLocked;
    public bool isLocked { get { return _isLocked; } }

    public event Action onLock;
    public event Action onUnlock;

    private StickyNoteState _currentState;
    public StickyNoteState CurrentState { get; set; }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _contentCanvas.Initialize(this);
        _controllerCanvas.Initialize(this);
        _editCanvas.Initialize(this);

        _currentState = StickyNoteState.Idle;
    }


    public void Lock()
    {
        if (onLock != null)
        {
            onLock();
        }
        _isLocked = true;
    }

    public void Unlock()
    {
        if (onUnlock != null)
        {
            onUnlock();
        }
        _isLocked = false;
    }
}

/*
public class StickyNote : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField]
    private RectTransform _controllerTarget;
    [SerializeField]
    private Image _controllerImage;

    [Header("Content")]
    [SerializeField]
    private RectTransform _contentTransform;
    [SerializeField]
    private BoxCollider2D _contentCollider;
    [SerializeField]
    private Image _contentImage;
    [SerializeField]
    private TMP_Text _contentText;

    [Header("Edit")]
    [SerializeField]
    private GameObject _editCanvas;
    [SerializeField]
    private TMP_InputField _editInputField;

    [Header("Colors")]
    [SerializeField]
    private Color[] _backgroundColors;
    [SerializeField]
    private Image _colorChangerIcon;

    private bool _hoveringOnController;
    private bool _hoveringOnContent;
    private int _colorIndex = 0;
    private bool _editing = false;
    private static PhotonTransformView _transformView;
    private static PhotonView _view;

    // Start is called before the first frame update
    void Start()
    {
        _transformView = GetComponent<PhotonTransformView>();
        _view = GetComponent<PhotonView>();
        if(!StickyNoteNetworkManager.Instance.networked)
        {
            _transformView.enabled = false;
            _view.enabled = false;
        }
        _contentCollider.size = new Vector2(_contentTransform.rect.width, _contentTransform.rect.height);
        _controllerImage.transform.DOScale(0, 0);
        _hoveringOnController = false;
        _hoveringOnContent = false;

        _colorChangerIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
        _colorChangerIcon.DOFade(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        _contentCollider.size = new Vector2(_contentTransform.rect.width, _contentTransform.rect.height);
        _controllerImage.transform.position = Camera.main.WorldToScreenPoint(_controllerTarget.position);
    }

    // ��ƼŰ��Ʈ �����ϴ� ��ǲ�ʵ� ���� �ٲ� ������ ���� ��ƼŰ��Ʈ�� �ݿ��ϴ� �Լ�
    public void OnValueChanged(string value)
    {
        if(StickyNoteNetworkManager.Instance.networked)
        {
            _view.RPC("TextUpdate",RpcTarget.All,value);
        }
        else{
            _contentText.text = value;
        }
    }


    #region PointerEvents
    // ���콺 �����Ͱ� ��ƼŰ��Ʈ ��Ʈ�ѷ� ������ ���� ��
    public void OnPointerEnterController()
    {
        _hoveringOnController = true;
    }

    // ���콺 �����Ͱ� ��ƼŰ��Ʈ ��Ʈ�ѷ� ������ ������ ��
    public void OnPointerExitController()
    {
        _hoveringOnController = false;
        StartCoroutine(HideController());
    }

    // ���콺 �����Ͱ� ��ƼŰ��Ʈ ������ ���� ��
    public void OnPointerEnterContent()
    {
        _hoveringOnContent = true;
        _controllerImage.transform.DOScale(1, 0.4f);
        _colorChangerIcon.DOFade(1, 0.4f);
    }

    // ���콺 �����Ͱ� ��ƼŰ��Ʈ ������ ������ ��
    public void OnPointerExitContent()
    {
        _hoveringOnContent = false;
        StartCoroutine(HideController());
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ� UI ����� �ڷ�ƾ
    IEnumerator HideController()
    {
        yield return new WaitForSeconds(0.5f);
        
        // ���콺 �����Ͱ� ��ƼŰ��Ʈ ��Ʈ�ѷ��� ��ƼŰ��Ʈ ��ü���� ����� ���, ��ƼŰ��Ʈ ��Ʈ�ѷ��� ����
        if (_hoveringOnController || _hoveringOnContent)
        {
            yield return null;
        }
        else
        {
            _controllerImage.transform.DOScale(0, 0.4f);
            _colorChangerIcon.DOFade(0, 0.4f);
        }
    }
    #endregion

    #region ControllerCallbacks
    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� Edit ��ư�� ������ ��
    public void OnClick_Edit()
    {
        if(_editing) return;
        _editCanvas.SetActive(true);
        if(StickyNoteNetworkManager.Instance.networked){
            _view.RPC("SwitchEditingTrue",RpcTarget.All);
        }
        _editInputField.text = _contentText.text;
    }

    // ��ƼŰ��Ʈ ��Ʈ�ѷ��� Remove ��ư�� ������ ��
    public void OnClick_Remove()
    {
        if(StickyNoteNetworkManager.Instance.networked){
            PhotonNetwork.Destroy(gameObject);
        }
        else Destroy(gameObject);
    }
    #endregion

    #region ButtonCallbacks
    // ��ƼŰ��Ʈ ���� ���� �� Confirm ��ư�� ������ ��
    public void OnClick_Confirm()
    {
        if(StickyNoteNetworkManager.Instance.networked){
            _view.RPC("SwitchEditingFalse",RpcTarget.All);
        }
        _editCanvas.SetActive(false);
    }

    // ��ƼŰ��Ʈ�� ���� ���� ��ư�� ������ ��
    public void OnClick_Color()
    {
        _colorIndex = GetNextColorIndex(_colorIndex);

        _contentImage.color = _backgroundColors[_colorIndex];
        _colorChangerIcon.color = _backgroundColors[GetNextColorIndex(_colorIndex)];
    }

    private int GetNextColorIndex(int i)
    {
        if (i+1 > _backgroundColors.Length - 1)
        {
            return 0;
        }
        else
        {
            return i + 1;
        }
    }
    #endregion
    [PunRPC]
    public void TextUpdate(string text)
    {
        _contentText.text = text;
    }
    private void SwitchEditingTrue()
    {
        _editing = true;
    }
    private void SwitchEditingFalse()
    {
        _editing = false;
    }
    
}*/
