using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;
    [SerializeField] private MouseGraphicsController _mouseGraphics;
    [SerializeField] private TextMeshProUGUI _energyText;
    [SerializeField] private Slider _milestoneSlider;

    private void Awake() => i = this;

    public void PressSunlightButton() => PressAbilityButton(PlayerAbility.Sunlight);
    public void PressPressureButton() => PressAbilityButton(PlayerAbility.Pressure);
    public void PressFrostButton() => PressAbilityButton(PlayerAbility.Frost);
    public void PressGravityButton() => PressAbilityButton(PlayerAbility.Gravity);

    private void PressAbilityButton(PlayerAbility ability)
    {
        GameManager.i.SelectNewAbility(ability);
    }

    public void SetAbilityGraphics(PlayerAbility ability)
    {
        _mouseGraphics.SunGraphic.SetActive(ability == PlayerAbility.Sunlight);
        _mouseGraphics.FrostGraphic.SetActive(ability == PlayerAbility.Frost);
        _mouseGraphics.GravityGraphic.SetActive(ability == PlayerAbility.Gravity);
        _mouseGraphics.PressureGraphic.SetActive(ability == PlayerAbility.Pressure);
    }

    public void DisplayEnergy(int energyAmount)
    {
        _energyText.text = "Energy left: " + energyAmount;
    }

    public void SetMilestoneBar(float value) {
        _milestoneSlider.value = value;
    }
}
