using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : MonoBehaviour
{
    public SkillData Data { get; private set; }

    public void Initialize(SkillData data)
    {
        Data = data;
        ApplyEffectToPlayer();
       // StartCoroutine(DurationCheck());
    }

    private void ApplyEffectToPlayer()
    {
        Debug.Log($"Applying skill: {Data.skillName}");
        Data.effect.ApplyEffect(); 
    }

    public void Stack()
    {


    }


/*    private IEnumerator DurationCheck()
    {
        
        if (Data.duration > 0)
        {
            yield return new WaitForSeconds(Data.duration);
            RemoveSkill();
        }
        
    } */

    private void RemoveSkill()
    {


    }
}

