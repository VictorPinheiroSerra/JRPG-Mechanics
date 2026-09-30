using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] List<AIBattler> activeBattlers = new List<AIBattler>();
    [SerializeField] AIBattler currentBattler;

    private void Start()
    {
        PopulateActiveBattlers();
        SelectCurrentBattler();
    }

    void PopulateActiveBattlers()
    {
        AIBattler tempBattler;
        foreach (GameObject battlerObj in GameObject.FindGameObjectsWithTag("Battler"))
        {
            if(battlerObj.TryGetComponent<AIBattler>(out tempBattler))
            activeBattlers.Add(tempBattler);
        }
    }

    void SelectCurrentBattler()
    {
        currentBattler = activeBattlers[0];

        foreach (AIBattler active in activeBattlers)
        {
            if (active.battlerData.GetAgility() > currentBattler.battlerData.GetAgility())
                currentBattler = active;
        }

        currentBattler.DoTurnActions(); //later on this will return something telling me if this battler should get another turn or if a shift happened
        //needs to wait for turn to be over (this Remove call might be moved to an EndTurn public method)
        activeBattlers.Remove(currentBattler);
    }

}
