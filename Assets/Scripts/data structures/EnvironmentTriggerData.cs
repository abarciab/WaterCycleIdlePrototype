using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public enum Opperator { Greater, Lesser, Equal }
public enum EffectType { ActiveState, Material}

[System.Serializable]
public class EnvironmentSubTrigger
{
    [SerializeField] private Resource _location;
    [SerializeField] private WaterState _state;
    [SerializeField] private Opperator _opperator;
    [SerializeField] private float _amount;

    public bool True() {
        float currentAmount = GameManager.i.GetResourceController(_location).GetStateTotal(_state);
        switch (_opperator) {
            case Opperator.Greater:
                return currentAmount > _amount;
            case Opperator.Lesser:
                return currentAmount < _amount;
            case Opperator.Equal:
                return currentAmount == _amount;
        }
        Debug.LogError("AHH");
        return true;
    }
}

[System.Serializable]
public class EnvironmentTriggerData
{
    [SerializeField] private List<EnvironmentSubTrigger> _triggers = new List<EnvironmentSubTrigger>();
    [SerializeField] private GameObject _object;
    [SerializeField] private bool permenant;
    [SerializeField] private EffectType _type;
    [SerializeField, ConditionalField(nameof(_type), false, EffectType.ActiveState)] private bool _targetState;
    [SerializeField, ConditionalField(nameof(_type), false, EffectType.Material)] private Material _activeMaterial;
    [SerializeField, ConditionalField(nameof(_type), false, EffectType.Material)] private Material _inactiveMaterial;
    bool _triggered;

    public void Check() {
        bool passedCheck = true;
        foreach (var t in _triggers) {
            if (!t.True()) {
                passedCheck = false;
            }
        }

        if (!passedCheck) {
            if (!permenant) Reverse();
            return;
        }

        if (!_triggered) {
            GameManager.i.CompleteMilestone();
            _triggered = true;
        }

        Execute();
    }

    private void Execute() {
        if (_type == EffectType.ActiveState) _object.SetActive(_targetState);
        else if (_type == EffectType.Material && _object.GetComponent<Renderer>()) _object.GetComponent<Renderer>().material = _activeMaterial;
        else if (_type == EffectType.Material) _object.GetComponent<Terrain>().materialTemplate = _activeMaterial;
    }

    private void Reverse() {
        if (_type == EffectType.ActiveState) _object.SetActive(!_targetState);
        else if (_type == EffectType.Material && _object.GetComponent<Renderer>()) _object.GetComponent<Renderer>().material = _inactiveMaterial;
        else if (_type == EffectType.Material) _object.GetComponent<Terrain>().materialTemplate = _inactiveMaterial;
    }
}
