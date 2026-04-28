using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//StateMachine for if a particular stat is normal, reduced or increased by skills like Tarukaja and Tarunda
public enum BuffState
{
    Regular,
    Buffed,
    Debuffed
}

public class Battler : MonoBehaviour
{
    [Header("----- Character Data -----")]
    //Name of the character
    [SerializeField] string charName;
    [SerializeField] protected int level = 1;


    [Header("----- Battle Stats -----")]
    //Maximum amount of Health Points
    [SerializeField] int maxHp;
    //Current amount of Health Points (always above 0 and at most equal to the maximum)
    [SerializeField] int currHp;
    //Maximum amount of Stamina Points
    [SerializeField] int maxSp;
    //Current amount of Stamina Points (always above 0 and at most equal to the maximum)
    [SerializeField] int currSp;
    //Used as the attack value in damage calculation for Physical skills
    [SerializeField] int strength;
    //Used as the attack value in damage calculation for Magic skills
    [SerializeField] int magic;
    //Used as the defense value in damage calculation
    [SerializeField] int endurance;
    //used to determine turn order for characters and in the dodge calculation
    [SerializeField] int agility;
    //used in crit-rate calculation
    [SerializeField] int luck;
    //determines interactions with each elemental affinity
    [SerializeField] Affinity[] affinities = new Affinity[11];

    [Header("----- Stat Changes -----")]
    //StateMachine for Attack buffs and debuffs (Tarukaja and Tarunda)
    [SerializeField] protected BuffState atkBuff;
    //StateMachine for Defense buffs and debuffs (Rakukaja and Rakunda)
    [SerializeField] protected BuffState defBuff;
    //StateMachine for Hit Rate/Evasion buffs and debuffs (Sukukaja and Sukunda)
    [SerializeField] protected BuffState evaBuff;
    //Checks if this character is currently downed after a hit to its weakness or a critical hit
    public bool isDowned;
    //Checks if the character is currently guarding
    public bool isGuarding;
    //Checks if the character has an active use of Charge
    public bool isCharged;
    //Checks if the characyer has an active use of Concentrate
    public bool isConcentrated;

    //Getters for stats
    public virtual int GetStrength()
    {
        if (atkBuff == BuffState.Buffed)
            return (int)(strength * 1.4f);
        else if (atkBuff == BuffState.Debuffed)
            return (int)(strength * 0.6f);
        else
            return strength;
    }
    public virtual int GetDisplayStrength() { return strength; }
    public virtual int GetMagic()
    {
        if (atkBuff == BuffState.Buffed)
            return (int)(magic * 1.4f);
        else if (atkBuff == BuffState.Debuffed)
            return (int)(magic * 0.6f);
        else
            return magic;
    }
    public virtual int GetDisplayMagic() { return magic; }
    public virtual int GetEndurance()
    {
        if (defBuff == BuffState.Buffed)
            return (int)(endurance * 1.4f);
        else if (defBuff == BuffState.Debuffed)
            return (int)(endurance * 0.6f);
        else
            return endurance;
    }
    public virtual int GetDisplayEndurance() { return endurance; }
    public virtual int GetAgility() { return agility; }
    public virtual int GetLuck() { return luck; }

    //Getters for other stuff
    public virtual int GetLevel() { return level; }
    //Getters for other stuff
    public virtual Affinity GetAffinity(Element element) 
    {
        if ((int)element > 11)
            return Affinity.Neutral;
        return affinities[(int)element]; 
    }

    //HP-Control Methods
    public void TakeDamage(int amount)
    {
        currHp -= amount;
        Mathf.Clamp(currHp, 0, maxHp);
    }
    public void TakeDamagePercent(int percent)
    {
        currHp -= (maxHp * percent);
        Mathf.Clamp(currHp, 0, maxHp);
    }

    public void UseSp(int amount)
    {
        currSp -= amount;
        Mathf.Clamp(currSp, 0, maxSp);
    }
}
