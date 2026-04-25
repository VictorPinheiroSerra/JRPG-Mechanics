using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Slash, //0
    Strike, //1
    Pierce, //2
    Fire, //3
    Ice, //4
    Elec, //5
    Wind, //6
    Nuke, //7
    Psy, //8
    Bless, //9
    Curse, //10
    Allmighty, //11
    Ailment, //12
    Healing, //13
    Buffs, //14
    Passive //15
}

public abstract class Skill : ScriptableObject
{
    [SerializeField] string skillName;
    public Element element;
    public bool multiTarget;
    [SerializeField] string description;
    [SerializeField] protected int cost;

    public string GetName() { return skillName; }
    public virtual void Execute(Battler[] target, Battler user)
    {

    }
}
