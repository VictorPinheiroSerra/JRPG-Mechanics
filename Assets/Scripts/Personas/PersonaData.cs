using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Arcana
{
    Fool,
    Magician,
    Chariot,
    Strength,
    Sun,
    Priestess,
    Hierophant,
    Justice,
    Emperor,
    Emperess,
    Star,
    Moon,
    Hanged,
    Devil,
    Tower,
    Hermit,
    Lovers,
    Fortune,
    Temperance,
    Judgement,
    Death,
    World
}

public enum Affinity
{
    Neutral,
    Resist,
    Weak,
    Absorb,
    Repel,
    Null,
    Unknown
}

[CreateAssetMenu(fileName = "New Persona", menuName = "Persona")]
public class PersonaData : ScriptableObject
{
    [Header("----- Identification -----")]
    [SerializeField] string personaName;
    [SerializeField] Arcana arcana;
    public Affinity[] affinities = new Affinity[11];
    [SerializeField] string background;
    [Header("----- Battle Stats -----")]
    //Used as the attack value in damage calculation for Physical skills
    public int strength;
    //Used as the attack value in damage calculation for Magic skills
    public int magic;
    //Used as the defense value in damage calculation
    public int endurance;
    //used to determine turn order for characters and in the dodge calculation
    public int agility;
    //used in crit-rate calculation
    public int luck;
}
