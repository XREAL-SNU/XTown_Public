using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using StarterAssets;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] CharacterPrefabs;
    public Transform SpawnPoint;
    public CinemachineFreeLook FreeLookCam;
    private Transform FollowTarget;
    private GameObject Player;
    public ThirdPersonControllerMulti PlayerControl
    {
        get => Player.GetComponent<ThirdPersonControllerMulti>();
        private set
        {
            return;
        }
    }
    private GameObject _prefab;
    public static LoadCharacter Instance = null;

    // singleton
    private void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Monobeviour callbacks
    /// </summary>
    void OnEnable()
    {
        Debug.Log("Load character enabled");
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        _prefab = CharacterPrefabs[selectedCharacter];
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Debug.Log("Load Character disabled");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player");
            if (Player != null)
            {
                //FollowTarget = Player.transform;
                FollowTarget = GameObject.Find("FollowTarget").transform;
                FreeLookCam.Follow = Player.transform;
                FreeLookCam.LookAt = FollowTarget;
            }
        }
    }

    /// <summary>
    /// Scene callbacks
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("LoadCharacter/OnSceneLoaded: " + scene.name);
        // disable currentRoomCanvas
        RoomsCanvases.Instance.CurrentRoomCanvas.Hide();

        if (GameObject.Find("SpawnPoint") is null)
        {
            Debug.Log("no character spawns on this scene");
            return;
        }
        SpawnPoint = GameObject.Find("SpawnPoint").transform;
        FreeLookCam = GameObject.Find("CharacterCam").GetComponent<CinemachineFreeLook>();


        InitCharacter();
    }

    /// <summary>
    /// Private members
    /// </summary>

    void InitCharacter()
    {
        Debug.Log("LoadCharacter/InitCharacter without network");
        if(!PhotonNetwork.InRoom || !PhotonNetwork.IsConnected)
        {// instantiate locally
            Player = Instantiate(_prefab, SpawnPoint.position, Quaternion.identity);
        }
        else
        {
            // instantiate over the network
            Debug.Log("LoadCharacter/Instantiating player over the network");
            Player = PhotonNetwork.Instantiate("CharacterPrefab", SpawnPoint.position, Quaternion.identity);
        }

        FollowTarget = Player.transform.Find("FollowTarget");
        FreeLookCam.Follow = Player.transform;
        FreeLookCam.LookAt = FollowTarget;
    }

}
