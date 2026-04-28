using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    List<Battler> activeBattlers = new List<Battler>();
    Battler currentBattler;

    private void Start()
    {
        PopulateActiveBattlers();
        SelectCurrentBattler();
    }

    void PopulateActiveBattlers()
    {
        Battler tempBattler;
        foreach (GameObject battlerObj in GameObject.FindGameObjectsWithTag("Battler"))
        {
            if(battlerObj.TryGetComponent<Battler>(out tempBattler))
            activeBattlers.Add(tempBattler);
        }
    }

    void SelectCurrentBattler()
    {
        currentBattler = activeBattlers[0];

        foreach (Battler active in activeBattlers)
        {
            if (active.GetAgility() > currentBattler.GetAgility())
                currentBattler = active;
        }
    }

}
