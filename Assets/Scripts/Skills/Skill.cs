using System;
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
    Ailment, //11
    Allmighty, //12
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
    [SerializeField] protected int damage;
    [SerializeField] protected int accuracy;

    protected Dictionary<int, float> levelMod = new Dictionary<int, float> 
    {
      {-13, 0.5f}, {-12, 0.51f}, {-11, 0.53f}, {-10, 0.59f},
      {-9, 0.66f}, {-8, 0.75f}, {-7, 0.84f}, {-6, 0.91f},
      {-5, 0.97f}, {-4, 0.99f}, {-3, 1.0f}, {-2, 1.0f},
      {-1, 1.0f}, {0, 1.0f}, {1, 1.01f}, {2, 1.03f},
      {3, 1.09f}, {4, 1.16f}, {5, 1.25f}, {6, 1.34f},
      {7, 1.41f}, {8, 1.47f}, {9, 1.49f}, {10, 1.5f}
    };

    public string GetName() { return skillName; }
    public virtual bool Execute(Battler[] target, Battler user, bool isRepelled)
    {
        return false;
    }

    protected virtual int CalculateDamage(float multiplier, Battler user, Battler target, bool isRepelled, bool isCrit)
    {
        return 0;
    }
}
