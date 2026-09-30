using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Playables;
using UnityEngine;

[System.Serializable]
public class Persona
{
    [SerializeField] PersonaData data;
    [Header("----- Battle Stats -----")]
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
    [SerializeField] Affinity[] affinities = new Affinity[11];
    [SerializeField] Skill[] skills = new Skill[8];

    public Persona(PersonaData source)
    {
        data = source;
        strength = source.strength;
        magic = source.magic;
        endurance = source.endurance;
        agility = source.agility;
        luck = source.luck;
        affinities = source.affinities;
    }

    //Getters for stats
    public int GetStrength() { return strength; }
    public int GetMagic() { return magic; }
    public int GetEndurance() { return endurance; }
    public int GetAgility() { return agility; }
    public int GetLuck() { return luck; }

    //Getters for other stuff
    public Affinity GetAffinity(Element element)
    {
        if ((int)element > 11)
            return Affinity.Neutral;
        return affinities[(int)element];
    }
    public virtual Skill[] GetSkillList() { return skills; }
}
