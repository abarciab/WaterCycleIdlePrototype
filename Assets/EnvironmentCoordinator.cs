using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCoordinator : MonoBehaviour
{
    [SerializeField] private List<EnvironmentTriggerData> _conditions;

    private void Start() {
        GameManager.i.OnDayEnd.AddListener(CheckConditions);
    }

    private void CheckConditions() {
        foreach (var c in _conditions) c.Check();
    }
}
