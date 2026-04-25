using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class PersonaUser : Battler
{
    [SerializeField] Persona activePersona;
    public override int GetStrength()
    {
        int str = activePersona.GetStrength();

        if (atkBuff == BuffState.Buffed)
            str = (int)(str * 1.4f);
        else if (atkBuff == BuffState.Debuffed)
            str = (int)(str * 0.6f);

        if (isCharged)
            str = (int)(str * 2.5f);

        return str;
    }
    public override int GetDisplayStrength() { return activePersona.GetStrength(); }
    public override int GetMagic()
    {
        int magic = activePersona.GetMagic(); 

        if (atkBuff == BuffState.Buffed)
            magic = (int)(magic*1.4f);
        else if (atkBuff == BuffState.Debuffed)
            magic = (int)(magic * 0.6f);

        if (isConcentrated)
            magic = (int)(magic * 2.5f);

        return magic;
    }
    public override int GetDisplayMagic() { return activePersona.GetMagic(); }
    public override int GetEndurance()
    {
        int end = activePersona.GetEndurance();
        if (defBuff == BuffState.Buffed)
            return (int)(end * 1.4f);
        else if (defBuff == BuffState.Debuffed)
            return (int)(end * 0.6f);
        else
            return end;
    }
    public override int GetDisplayEndurance() { return activePersona.GetEndurance(); }
    public override int GetAgility() { return activePersona.GetAgility(); }
    public override int GetLuck() { return activePersona.GetLuck(); }
    public override Affinity GetAffinity(Element element) { return activePersona.GetAffinity(element); }
}
