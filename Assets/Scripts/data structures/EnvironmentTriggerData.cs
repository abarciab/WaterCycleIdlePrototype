using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Opperator { Greater, Lesser, Equal }

[System.Serializable]
public class EnvironmentSubTrigger
{
    public Resource Location;
    public WaterState State;
    public Opperator Opperator;
}

[System.Serializable]
public class EnvironmentTriggerData
{
    
}
