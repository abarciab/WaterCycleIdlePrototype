using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum PlayerAbility { None, Sunlight, Pressure, Gravity, Frost}
public class GameManager : MonoBehaviour
{
    public static GameManager i;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Fade fade;
    [SerializeField] MusicPlayer music;
    [SerializeField] private PlayerAbility _currentAbility;
    [SerializeField] private int _dailyEnergy;
    [SerializeField, ReadOnly] private int _energy;

    [HideInInspector] public UnityEvent OnAbilityChange;
    [HideInInspector] public PlayerAbility CurrentAbility => _currentAbility;
    [HideInInspector] public int CurrentEnergy => _energy;
    [HideInInspector] public List<ResourceController> Resources = new();

    private void Start()
    {
        fade.Hide();
        StartNewDay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void DecrementEnergy()
    {
        _energy -= 1;
        UIManager.i.DisplayEnergy(_energy);
    }

    public ResourceController GetResourceController(Resource type)
    {
        return Resources.Where(x => x.Type == type).First();
    }

    public void StartNewDay()
    {
        _energy = _dailyEnergy;
        UIManager.i.DisplayEnergy(_energy);
        SelectNewAbility(PlayerAbility.None);
    }

    public void SelectNewAbility(PlayerAbility ability)
    {
        _currentAbility = ability;
        UIManager.i.SetAbilityGraphics(ability);
        OnAbilityChange.Invoke();
    }

    void TogglePause()
    {
        if (Time.timeScale == 0) Resume();
        else Pause();
    }

    private void Awake()
    {
        i = this;
    }


    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioManager.i.Resume();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        AudioManager.i.Pause();
    }

    [ButtonMethod]
    public void LoadMenu()
    {
        Resume();
        StartCoroutine(FadeThenLoadScene(0));
    }

    [ButtonMethod]
    public void EndGame()
    {
        Resume();
        StartCoroutine(FadeThenLoadScene(2));
    }

    IEnumerator FadeThenLoadScene(int num)
    {
        fade.Appear(); 
        music.FadeOutCurrent(fade.fadeTime);
        yield return new WaitForSeconds(fade.fadeTime + 0.5f);
        Destroy(AudioManager.i.gameObject);
        SceneManager.LoadScene(num);
    }

}
