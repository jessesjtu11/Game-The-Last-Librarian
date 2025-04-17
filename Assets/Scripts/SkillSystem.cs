using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    public static SkillSystem Instance { get; private set; }

  /*  [Header("技能配置")]
    [SerializeField] private SkillTreeConfig skillTree;
    
    private HashSet<SkillType> unlockedSkills = new HashSet<SkillType>();
    private Dictionary<SkillType, SkillEffect> activeEffects = new Dictionary<SkillType, SkillEffect>();

    #region 事件系统
    public delegate void SkillUpdate(SkillType skill);
    public event SkillUpdate OnSkillUnlocked;
    public event SkillUpdate OnSkillUpgraded;
    #endregion

    private void Awake()
    {
        InitializeSingleton();
        LoadSkillData();
    }

    private void InitializeSingleton()
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

    public void UnlockSkill(SkillType skill)
    {
        if (unlockedSkills.Add(skill))
        {
            ApplySkillEffect(skill);
            OnSkillUnlocked?.Invoke(skill);
            SaveSkillData();
        }
    }

    public void UpgradeSkill(SkillType skill)
    {
        if (skillTree.CanUpgrade(skill, unlockedSkills))
        {
            // 具体升级逻辑...
            OnSkillUpgraded?.Invoke(skill);
            SaveSkillData();
        }
    }

    public bool HasSkill(SkillType skill) => unlockedSkills.Contains(skill);

    public int GetSkillLevel(SkillType skill)
    {
        // 返回技能当前等级
        return 0;
    }

    private void ApplySkillEffect(SkillType skill)
    {
        SkillEffect effect = skillTree.GetEffect(skill);
        if (effect != null)
        {
            activeEffects[skill] = effect;
            effect.Activate();
        }
    }

    #region 数据持久化
    private void SaveSkillData()
    {
        string skills = string.Join(",", unlockedSkills);
        PlayerPrefs.SetString("UnlockedSkills", skills);
    }

    private void LoadSkillData()
    {
        string[] skills = PlayerPrefs.GetString("UnlockedSkills").Split(',');
        foreach (string s in skills)
        {
            if (System.Enum.TryParse(s, out SkillType skill))
            {
                unlockedSkills.Add(skill);
                ApplySkillEffect(skill);
            }
        }
    }
    #endregion
}





// 新增的配置类
[CreateAssetMenu(menuName = "Game/SkillTree")]
public class SkillTreeConfig : ScriptableObject
{
    [System.Serializable]
    public class SkillNode
    {
        public SkillType skill;
        public List<SkillType> prerequisites;
        public SkillEffect effect;
        public int maxLevel;
    }

    [SerializeField] private List<SkillNode> skillNodes = new List<SkillNode>();

    public SkillEffect GetEffect(SkillType skill)
    {
        return skillNodes.Find(n => n.skill == skill)?.effect;
    }

    public bool CanUpgrade(SkillType targetSkill, HashSet<SkillType> unlocked)
    {
        SkillNode node = skillNodes.Find(n => n.skill == targetSkill);
        return node != null && unlocked.IsSupersetOf(node.prerequisites);
    }
}



// 技能效果基类
public abstract class SkillEffect : ScriptableObject
{
    public abstract void Activate();
    public abstract void Deactivate();
}



// 示例：防火技能
[CreateAssetMenu(menuName = "Game/Skills/FireResistance")]
public class FireResistanceEffect : SkillEffect
{
    [SerializeField] private float damageReduction = 0.7f;

    public override void Activate()
    {
        PlayerManager.Instance.OnTakeDamage += ReduceFireDamage;
    }

    public override void Deactivate()
    {
        PlayerManager.Instance.OnTakeDamage -= ReduceFireDamage;
    }

    private void ReduceFireDamage(ref DamageInfo damage)
    {
        if (damage.type == DamageType.Fire)
        {
            damage.amount *= damageReduction;
        }
    }

    */
}
