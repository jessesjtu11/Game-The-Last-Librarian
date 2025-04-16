using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Room : MonoBehaviour
{
    [Header("基础配置")]
    [SerializeField] protected Sprite roomBackground;
    [SerializeField] protected List<InteractableObject> interactables = new List<InteractableObject>();
    
    [Header("组件引用")]
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private Transform interactiveObjectsContainer;
    
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    public bool allowInteraction=true;

    protected virtual void Start()
    {
        InitializeRoom();
    }

    protected virtual void InitializeRoom()
    {
        // 显示房间背景
        Load_room_scene();
        
        // 生成交互物体
        foreach (var obj in interactables)
        {
            CreateInteractableObject(obj);
        }
    }

    private void CreateInteractableObject(InteractableObject objData)
    {
        GameObject obj = new GameObject(objData.objectName);
        obj.transform.SetParent(interactiveObjectsContainer);
        obj.transform.localPosition = objData.position;
        
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = objData.normalSprite;
        sr.sortingOrder = 1;
        
        // 添加碰撞体
        BoxCollider2D collider = obj.AddComponent<BoxCollider2D>();
        collider.size = sr.bounds.size;
        
        // 添加点击处理器
        Clickable clickable = obj.AddComponent<Clickable>();
        clickable.Initialize(this, objData);
        
        spawnedObjects.Add(objData.objectName, obj);
    }

    public virtual void Load_room_scene()
    {
        backgroundRenderer.sprite = roomBackground;
        interactiveObjectsContainer.gameObject.SetActive(true);
    }

    public virtual void Unload_room_scene()
    {
        backgroundRenderer.sprite = null;
        interactiveObjectsContainer.gameObject.SetActive(false);
    }

    public virtual void OnObjectClicked(InteractableObject objData)
    {
        Debug.Log($"Clicked on {objData.objectName}");
        if (!allowInteraction)
        {
            Debug.Log("Interaction is disabled for this room.");
            return;
        }
        Interact(objData.objectName);        
    }

    protected abstract void Interact(string objectName);
}

