using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomizableElement
{
    // material to be edited
    Material _material;
    public static int MaterialCount;


    public CustomizableElement(Material presetMaterial)
    {
        Debug.Log("customizableElement/ constructor");
        if (presetMaterial is null)
        {
            Debug.LogError("customizableElement/ null preset");
            return;
        }
        if (_material is null)
        {
            // copy construct preset.
            _material = new Material(presetMaterial);
            Debug.Log("Material Count: " + MaterialCount++);
        }
    }

    public void SetMaterialBaseColor(Color col)
    {
        Debug.Log("CustomizableElement/ " + col.ToString());
        _material.SetColor("_Color", col);
    }

    // consider using byte array
    [PunRPC]
    public void SetMaterialBaseColor(float r, float g, float b, float a)
    {
        Debug.Log("CustomizableElement/ color networked");
        Color col = new Color();
        col.r = r; col.g = g; col.b = b; col.a = a;
        _material.SetColor("_Color", col);
    }


    public void Apply(GameObject obj)
    {
        if (_material is null) return;
        // apply changes to the material.
        Debug.Log("Apply mat " + _material.name + " to " + obj.name);
        obj.GetComponent<Renderer>().material = _material;
    }

    public void Sync(PlayerAvatar avatar)
    {
        if (_material is null) return;
        Debug.Log("CustomizableElement/Sync");
        // 1. sync base color
        Color col = _material.GetColor("_Color");
        avatar.PhotonView.RPC("SetMaterialBaseColor", RpcTarget.Others, col.r, col.g, col.b, col.a);
    }


}
