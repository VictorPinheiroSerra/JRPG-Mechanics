using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Magic Skill")]
public class MagicSkill : Skill
{
    public override bool Execute(Battler[] target, Battler user, bool isRepelled)
    {
        bool secondTurn = false;
        if (!isRepelled)
            user.UseSp(cost);
        foreach (Battler single in target)
        {
            //checks if attack will hit
            if (Random.Range(0, 100) < accuracy || isRepelled)
            {
                switch (single.GetAffinity(element))
                {
                    case Affinity.Neutral:
                        single.TakeDamage(CalculateDamage(1, user, single, isRepelled, false));
                        break;
                    case Affinity.Weak:
                        if (!isRepelled)
                            secondTurn = true;
                        single.TakeDamage(CalculateDamage(1.5f, user, single, isRepelled, false));
                        break;
                    case Affinity.Resist:
                        single.TakeDamage(CalculateDamage(0.5f, user, single, isRepelled, false));
                        break;
                    case Affinity.Absorb:
                        single.TakeDamage(CalculateDamage(-1, user, single, isRepelled, false));
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
            statCalc = (int)Mathf.Sqrt(damage * 15f * user.GetMagic() / target.GetEndurance()) * 2;
            levelModifier = levelMod[Mathf.Clamp(user.GetLevel() - target.GetLevel(), -13, 10)];
        }
        else
            statCalc = (int)Mathf.Sqrt(damage * 15f * target.GetMagic() / target.GetEndurance()) * 2;

        return 
            (int)
             (statCalc  
             * levelModifier //modifier based on the level difference between the user and target
             * multiplier //1x for Neutral, 1.5x for Weakness, 0.5x for resistence, -1x for draining (heals target)
             * UnityEngine.Random.Range(0.95f, 1.05f) //random variance
             );
    }
}
