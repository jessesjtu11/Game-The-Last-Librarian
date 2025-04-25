using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skill Data",order=0)]
public class SkillData : ScriptableObject
{
    public int skillID;
    public string skillName;
    [TextArea] public string description;
    public int familarityLevel = 0; // 熟练度   0-50-100
    public float duration = 0; // 0表示永久生效
    public SkillEffect effect; // 技能效果数组
}
