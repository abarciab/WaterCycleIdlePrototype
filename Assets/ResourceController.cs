using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum WaterState { Liqiud, Solid, Gas}
public enum Resource { Ocean, Atmosphere, Groundwater, River}

public class ResourceController : MonoBehaviour
{
    public Resource Type;
    [SerializeField] private List<WaterTransitionData> _transitions = new List<WaterTransitionData>();

    [Header("Referneces")]
    [SerializeField] private Button _liquidButton;
    [SerializeField] private Button _solidButton;
    [SerializeField] private Button _gasButton;

    private Dictionary<WaterState, float> _stateTotals = new Dictionary<WaterState, float>();

    private WaterTransitionData _liquidTransition;
    private WaterTransitionData _solidTransition;
    private WaterTransitionData _gasTransition;

    private void Start()
    {
        _stateTotals.Add(WaterState.Liqiud, 0f);
        _stateTotals.Add(WaterState.Solid, 0f);
        _stateTotals.Add(WaterState.Gas, 0f);

        GameManager.i.OnAbilityChange.AddListener(RefreshButtons);
        GameManager.i.Resources.Add(this);
        RefreshVisuals();
    }

    private void RefreshButtons()
    {
        if (Type == Resource.Ocean) _stateTotals[WaterState.Liqiud] = 1;
        List<WaterTransitionData> currentTransitions = _transitions.Where(x => x.Ability == GameManager.i.CurrentAbility).ToList();
        _liquidTransition = currentTransitions.Where(x => x.StartState == WaterState.Liqiud).FirstOrDefault();
        _solidTransition = currentTransitions.Where(x => x.StartState == WaterState.Solid).FirstOrDefault();
        _gasTransition = currentTransitions.Where(x => x.StartState == WaterState.Gas).FirstOrDefault();

        _liquidButton.enabled = _liquidTransition != null;
        _solidButton.enabled = _solidTransition != null;
        _gasButton.enabled = _gasTransition != null;
        gameObject.SetActive(_stateTotals[WaterState.Liqiud] + _stateTotals[WaterState.Solid] + _stateTotals[WaterState.Gas] > 0.01f);

        //print("current ability: " + GameManager.i.CurrentAbility);
        //print("liquid transition: " + _liquidTransition + ", solidTransition: " +  _solidTransition + ", gasTransition: " + _gasTransition);
    }

    public void PressLiquidButton() => ActivateTransition(_liquidTransition);
    public void PressSolidBUtton() => ActivateTransition(_solidTransition);
    public void PressGasButton() => ActivateTransition(_gasTransition);

    private void ActivateTransition(WaterTransitionData transition)
    {
        if (transition == null || GameManager.i.CurrentEnergy <= 0) return;
        float totalAvaliable = _stateTotals[transition.StartState];

        float amount = totalAvaliable >= transition.Amount ? transition.Amount : totalAvaliable;
        _stateTotals[transition.StartState] -= amount;

        var dest = GameManager.i.GetResourceController(transition.FinalResource);
        dest.AddResource(transition.FinalState, amount);
        GameManager.i.DecrementEnergy();
        RefreshVisuals();
    }

    public void AddResource(WaterState type, float amount)
    {
        _stateTotals[type] = Mathf.Min(1, _stateTotals[type] + amount);
        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        RefreshButtons();
        _liquidButton.gameObject.SetActive(_stateTotals[WaterState.Liqiud] > 0);
        _liquidButton.GetComponentInChildren<Slider>().value = _stateTotals[WaterState.Liqiud];

        _solidButton.gameObject.SetActive(_stateTotals[WaterState.Solid] > 0);
        _solidButton.GetComponentInChildren<Slider>().value = _stateTotals[WaterState.Solid];

        _gasButton.gameObject.SetActive(_stateTotals[WaterState.Gas] > 0);
        _gasButton.GetComponentInChildren<Slider>().value = _stateTotals[WaterState.Gas];
    }

    [ButtonMethod]
    private void printContents()
    {
        print(ContentsToString());
    }

    private string ContentsToString()
    {
        return "liquid water: " + _stateTotals[WaterState.Liqiud] + ", ice: " + _stateTotals[WaterState.Solid] + ", vapor: " + _stateTotals[WaterState.Gas];
    }
}
