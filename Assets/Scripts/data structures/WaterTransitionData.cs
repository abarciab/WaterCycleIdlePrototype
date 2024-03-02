using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaterTransitionData
{
    public WaterState StartState;
    public PlayerAbility Ability;
    public float Amount;

    [Header("Destination")]
    public WaterState FinalState;
    public Resource FinalResource;

    public override string ToString()
    {
        return StartState + " + " + Ability + " => " + FinalState + " in " + FinalResource;
    }
}
