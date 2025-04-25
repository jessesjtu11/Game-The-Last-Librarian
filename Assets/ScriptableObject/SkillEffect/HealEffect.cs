using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Skills/Effects/HealEffect")]
public class HealEffect : SkillEffect
{
    [SerializeField] float healAmount;

    public override void ApplyEffect( )
    {
        Player.Instance.currentBodyTemp += healAmount;
    }
}


