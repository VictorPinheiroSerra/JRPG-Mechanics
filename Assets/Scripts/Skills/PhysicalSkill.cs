using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Physical Skill")]
public class PhysicalSkill : Skill 
{
    [SerializeField] int damage;
    [SerializeField] int accuracy;
    [SerializeField] int critChance;

    public override void Execute(Battler[] target, Battler user)
    {
        user.TakeDamagePercent(cost);
        foreach (Battler single in target)
        {
            if (Random.Range(0, 100) <= accuracy)
            {

            }
        }
    }
}
