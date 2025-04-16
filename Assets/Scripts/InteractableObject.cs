using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 交互物体数据模板
[CreateAssetMenu(fileName = "InteractableObject", menuName = "InteractableObject",order=0)]
public class InteractableObject : ScriptableObject
{
    public string objectName;
    public Sprite normalSprite;
    public Sprite highlightedSprite;
    public Vector2 position;
    [Range(0.5f, 2f)] public float zoomScale = 1.2f;
}
