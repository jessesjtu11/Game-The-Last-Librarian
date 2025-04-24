using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/Skill Data Base",order=0)]
public class SkillDataBase : ScriptableObject
{
    public List<SkillBookPair> skillBookMappings;

    public bool TryGetSkillByBook(int bookID, out SkillData data)
    {
        var mapping = skillBookMappings.Find(m => m.bookID == bookID);
        data = mapping?.skillData;
        return data != null;
    }

    [System.Serializable]
    public class SkillBookPair
    {
        public int bookID;
        public SkillData skillData;
    }
}