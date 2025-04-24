using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillEffect
{
    void ApplyEffect();
    void StackEffect();
}

public abstract class SkillEffect : ScriptableObject, ISkillEffect
{
    public abstract void ApplyEffect();
    public virtual void StackEffect() { }
}


[CreateAssetMenu(menuName = "Skills/Effects/HealEffect")]
public class HealEffect : SkillEffect
{
    [SerializeField] float healAmount;

    public override void ApplyEffect( )
    {
        Player.Instance.currentBodyTemp += healAmount;
    }
}

