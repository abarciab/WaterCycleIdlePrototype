using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[System.Serializable]
public class WaterTransitionData
{
    public WaterState StartState;
    public PlayerAbility Ability;
    public float Amount;

    [Header("Destination")]
    public WaterState FinalState;
    public Resource FinalResource;

    [Header("SecondaryDestination")]
    [Range(0, 1)]public float SecondaryChance;
    public WaterState SecondaryState;
    public Resource SecondayResource;


    public override string ToString()
    {
        return StartState + " + " + Ability + " => " + FinalState + " in " + FinalResource;
    }
}
