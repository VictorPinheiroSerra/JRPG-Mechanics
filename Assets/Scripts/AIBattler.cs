using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

[System.Serializable]
public class BattlerMemory
{
    [SerializeField] Affinity[] knownAffinities = {  
        Affinity.Unknown, Affinity.Unknown, Affinity.Unknown, Affinity.Unknown,
        Affinity.Unknown, Affinity.Unknown, Affinity.Unknown, Affinity.Unknown,
        Affinity.Unknown, Affinity.Unknown, Affinity.Unknown
    };
    [SerializeField] List<Skill> knownSkills = new List<Skill>();

    public Affinity GetAffinity(Element element)
    {
        return knownAffinities[(int)element];
    }

    public void AddKnownAffinity(Element element, Affinity seen)
    {
        knownAffinities[(int)element] = seen;
    }

    public void AddKnownSkill(Skill seen)
    {
        knownSkills.Add(seen);
    }
}

public class AIBattler : MonoBehaviour
{
    public Battler battlerData;

    [Header("——-Action Weights ——-")]
    /*
    Variable that represents how much priority the AI gives to using skills that do damage:
    0 = never goes for damage
    1 = always goes for damage
    */
    [SerializeField, Range(0f, 1f)] float weightOffensiveSkills;
    /*
    Variable that represents how much priority the AI gives to using attacks that hit weaknesses:
    0 = never hits weakness
    1 = always hits weakness
    */
    [SerializeField, Range(0f, 1f)] float weightHitWeakness;
    /*
    Variable that represents how much priority the AI gives to using SP or conserving it:
    0 = only attacks that don’t consume sp
    1 = only attacks that consume SP
    */
    [SerializeField, Range(0f, 1f)] float weightSpendSP;
    /*
    Variable that represents how much priority the AI gives to attacks that hit multiple targets:
    0 = only single-target attacks
    1 = only multi-target attacks
    */
    [SerializeField, Range(0f, 1f)] float weightMultiTarget;

    [SerializeField] 
    SerializableDictionary<Battler, BattlerMemory> knownData = new SerializableDictionary<Battler, BattlerMemory>();

    public void DoTurnActions()
    {
        if (battlerData.isDowned)
            battlerData.isDowned = false;

        float comparisonParameter = float.MinValue;
        Skill selected = null;
        Battler primaryTarget = null;
        List<Battler> targetTeam = new List<Battler>();

        foreach (Skill skill in battlerData.GetSkillList())
        {
            if (skill == null) continue;

            foreach (Battler target in knownData.Keys())
            {
                if (target == null) continue;

                float temp = EvaluateSkill(skill, target);
                if(temp > comparisonParameter)
                {
                    comparisonParameter = temp;
                    selected = skill;
                    primaryTarget = target;
                }
            }
        }

        if (selected.multiTarget)
        {
            foreach (Battler target in knownData.Keys())
            {
                if (target.isEnemy == primaryTarget.isEnemy)
                    targetTeam.Add(target);
            }
        }
        else
            targetTeam.Add(primaryTarget);

        Debug.Log(this.name + " selected to use " + selected.GetName());
        selected.Execute(targetTeam.ToArray(), battlerData, false);
    }

    private void Start()
    {
        foreach (GameObject battlerObj in GameObject.FindGameObjectsWithTag("Battler"))
        {
            if (battlerObj != gameObject)
            {
                Battler tempBattler;
                if (battlerObj.TryGetComponent<Battler>(out tempBattler))
                {
                    knownData.Add(tempBattler, new BattlerMemory());
                }
            }
        }
    }

    public float EvaluateSkill(Skill skill, Battler target)
    {
        float score = 10f;
        bool isSameTeam = battlerData.isEnemy == target.isEnemy;

        Affinity knownAffinity = knownData[target].GetAffinity(skill.element);
        if (knownAffinity == Affinity.Weak) score += (10f * weightHitWeakness);
        if (knownAffinity == Affinity.Resist) score -= 15f;
        if (knownAffinity == Affinity.Repel || knownAffinity == Affinity.Absorb || knownAffinity == Affinity.Null) score -= 100f;

        if (skill is MagicSkill || skill is PhysicalSkill)
        {
            if(isSameTeam)
               return -9999f;
            score *= weightOffensiveSkills;
        }
        if (skill.multiTarget)
        {
            score *= weightMultiTarget;
        }
        if(skill is MagicSkill /* Also will eventually include buffs and debuffs + healing */)
        {
            float costFactor = (float)skill.GetCost() / battlerData.GetCurrentSP();
            score -= (costFactor * 50f * weightSpendSP);
        }

        return score;
    }
 }
