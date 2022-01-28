using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StickyNoteButton : MonoBehaviour
{
    public GameObject stickyNotePrefab;
    private static PhotonTransformView transformView;
    private static PhotonView view;

    public void CreateStickyNote()
    {
        Instantiate(stickyNotePrefab, Vector3.zero, Quaternion.identity);
    }
}
