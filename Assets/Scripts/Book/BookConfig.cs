using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// 书籍配置ScriptableObject
[CreateAssetMenu(menuName = "Room/Book Config",order=0)]
public class BookConfig : ScriptableObject
{
    public string bookName;
    [TextArea] public string description;
    [Range(0, 100)] public float spawnProbability = 30f;
    public SkillType associatedSkill;
}