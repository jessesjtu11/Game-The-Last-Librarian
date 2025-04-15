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

    public void Initialize(Room room, InteractableObject data)
    {
        parentRoom = room;
        objData = data;
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        parentRoom.OnObjectClicked(objData);
    }

    private void OnMouseEnter()
    {
        sr.sprite = objData.highlightedSprite;
    }

    private void OnMouseExit()
    {
        sr.sprite = objData.normalSprite;
    }
}

