using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public static SkillSystem Instance { get; private set; }

    [SerializeField] private SkillDataBase skillDatabase;
    private Dictionary<int, ActiveSkill> activeSkills = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AcquireSkillFromBook(int bookID)
    {
        if (skillDatabase.TryGetSkillByBook(bookID, out SkillData data))
        {
            ApplySkill(data);
        }
        else 
            Debug.Log($"Skill with book ID {bookID} not found in database.");
    }

    private void ApplySkill(SkillData data)
    {
        if (activeSkills.ContainsKey(data.skillID))
        {
            if (data.familarityLevel < 100)
            {
                activeSkills[data.skillID].Stack();
            }
            return;
        }

        var skillObject = new GameObject($"Skill_{data.skillID}");
        var activeSkill = skillObject.AddComponent<ActiveSkill>();
        activeSkill.Initialize(data);   //应用技能
        
        activeSkills.Add(data.skillID, activeSkill);
    }

    public bool HasSkill(int skillID)
    {
        return activeSkills.ContainsKey(skillID);
    }
}