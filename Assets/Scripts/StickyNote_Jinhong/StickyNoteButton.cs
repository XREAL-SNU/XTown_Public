using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StickyNoteButton : MonoBehaviour
{
    public GameObject stickyNotePrefab;
    public void CreateStickyNote()
    {
        if(StickyNoteNetworkManager.Instance.networked)
            PhotonNetwork.Instantiate(stickyNotePrefab.name, Vector3.zero, Quaternion.identity);
        else
            Instantiate(stickyNotePrefab, Vector3.zero, Quaternion.identity);
    }
}
