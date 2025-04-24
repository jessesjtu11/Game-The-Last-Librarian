using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 书籍配置ScriptableObject
[CreateAssetMenu(menuName = "Room/Book Config",order=0)]
public class BookConfig : ScriptableObject
{
    public int bookID;
    public string bookName;
    [TextArea] public string description;
    [Range(0, 100)] public float spawnProbability = 30f;

    public float burnHeatValue = 0.1f;
    public float readHeatCost = 0.3f;
    public float readTimeCost = 10f;
    public float burnTimeCost = 20f;
}