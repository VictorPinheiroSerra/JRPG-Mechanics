using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Physical Skill")]
public class PhysicalSkill : Skill 
{
    [SerializeField] int critChance;

    public override bool Execute(Battler[] target, Battler user, bool isRepelled)
    {
        bool secondTurn = false;
        bool crit;
        if(!isRepelled)
           user.TakeDamagePercent(cost);
        foreach (Battler single in target)
        {
            crit = false;
            //checks if attack will hit
            if (Random.Range(0, 100) < accuracy || isRepelled)
            {
                Affinity targetAffinity = single.GetAffinity(element);
                if (!isRepelled &&
                   (targetAffinity == Affinity.Neutral ||
                    targetAffinity == Affinity.Weak ||
                    targetAffinity == Affinity.Resist))
                {
                    if (Random.Range(0, 100) < critChance * (user.GetLuck()/(float)single.GetLuck()))
                        crit = true;
                }
                   switch (targetAffinity)
                    {
                    case Affinity.Neutral:
                        single.TakeDamage(CalculateDamage(1, user, single, isRepelled, crit));
                        if (crit)
                            secondTurn = true;
                        break;
                    case Affinity.Weak:
                        single.TakeDamage(CalculateDamage(1.5f, user, single, isRepelled, crit));
                        if (!isRepelled)
                            secondTurn = true;
                        break;
                    case Affinity.Resist:
                        if (crit)
                        {
                            single.TakeDamage(CalculateDamage(1, user, single, isRepelled, crit));
                            secondTurn = true;
                        }
                        else
                            single.TakeDamage(CalculateDamage(0.5f, user, single, isRepelled, crit));
                        break;
                    case Affinity.Absorb:
                        single.TakeDamage(CalculateDamage(-1, user, single, isRepelled, crit));
                        break;
                    case Affinity.Repel:
                        if (!isRepelled) 
                        {
                            Battler[] repelTarget = { user };
                            Execute(repelTarget, single, true);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        return secondTurn;
    }

    protected override int CalculateDamage(float multiplier, Battler user, Battler target, bool isRepelled, bool isCrit)
    {
        int statCalc = 0;
        float levelModifier = 1;
        if (!isRepelled)
        {
            //applying the user and target's offensive and defensive stats
            statCalc = (int)Mathf.Sqrt(damage * 15f * user.GetStrength() / target.GetEndurance()) * 2;
            levelModifier = levelMod[Mathf.Clamp(user.GetLevel() - target.GetLevel(), -13, 10)];
        }
        else
            statCalc = (int)Mathf.Sqrt(damage * 15f * target.GetStrength() / target.GetEndurance()) * 2;

        if (isCrit)
            statCalc = (int) (statCalc * 1.5f); //applying the damage boost if the attack was critical
        return (int)
             (statCalc //applying the user and target's offensive and defensive stats 
             * levelModifier //modifier based on the level difference between the user and target
             * multiplier //1x for Neutral, 1.5x for Weakness, 0.5x for resistence, -1x for draining (heals target)
             * UnityEngine.Random.Range(0.95f, 1.05f) //random variance
             );
    }
}
