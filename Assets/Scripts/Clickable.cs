using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 点击处理组件
public class Clickable : MonoBehaviour
{
    private Room parentRoom;
    private InteractableObject objData;
    private SpriteRenderer sr;
    private bool allowInteraction;

    public void Initialize(Room room, InteractableObject data)
    {
        parentRoom = room;
        objData = data;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
       allowInteraction = parentRoom.allowInteraction;
    }

    public void OnMouseDown()
    {
        if (!allowInteraction) return; // 如果不允许交互，直接返回
        parentRoom.OnObjectClicked(objData);
    }

    private void OnMouseEnter()
    {
        if (!allowInteraction) return; // 如果不允许交互，直接返回
        sr.sprite = objData.highlightedSprite;
    }

    private void OnMouseExit()
    {
        if (!allowInteraction) return; // 如果不允许交互，直接返回
        sr.sprite = objData.normalSprite;
    }
}

