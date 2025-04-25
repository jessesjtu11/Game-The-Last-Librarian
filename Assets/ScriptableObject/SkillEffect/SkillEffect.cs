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


