using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Settings")] [Description("The maximum Health.")]
    private float _maxHealth = 5f;

    public float MaxHealth
    {
        get { return _maxHealth; }
        set
        {
            _maxHealth = value;
            UpdateDisplay();
        }
    }

    [Description("Wether the health should be displayed in the ui.")]
    private bool _displayBar = true;

    public bool DisplayBar
    {
        get { return _displayBar; }
        set
        {
            if (_displayBar != value)
            {
                _displayBar = value;
                displayCanvas.SetActive(_displayBar);
            }
        }
    }

    public enum DeathResponse
    {
        DoNothing = 0,
        DisableGameObject = 1,
        DestroyGameObject = 2
    }

    [Header("Death Settings")] [Description("What happens per on death, the death event is always fired.")]
    public DeathResponse deathHandling = DeathResponse.DisableGameObject;

    [Description("Fired when health reaches <=0.")]
    public UnityEvent onDeath;

    [Description("Fired whenever someone calls the takeDamage function with a value >0")]
    public UnityEvent onDamage;

    [Description("Fired whenever someone calls the healDamange function with a value >0")]
    public UnityEvent onHeal;

    [Description("Fired whenever health changes.")]
    public UnityEvent onHealthChange;

    [Header("Display Settings")] [Description("Wether to show the healthbar if health is full or not.")]
    public bool hideOnFullHealth = false;

    #region EditorData

    [HideInInspector] public bool showEventFoldout = true;
    [HideInInspector] public bool showDisplayFoldout = true;

    #endregion

    public enum DisplayOption
    {
        Hidden,
        Bar,
        Hearts,
        Shader // see https://www.reddit.com/r/Unity3D/comments/wcxgbt/simple_procedural_health_bars_shaderproject_link/

    }

    public DisplayOption healthDisplay = DisplayOption.Bar;

    //for bar version of display
    public GameObject displayCanvas;
    public Slider healthBarSlider;
    public Gradient healthBarColor;

    public Image healthBarFill;

    //for individual hearts version of display
    public HealthHeartBar healthHeartBar;

    //for shader version of healthbar
    public HealthBar shaderHealthBar;

    /// <summary>
    /// Do not use this! use the public property instead so the ui is updated correctly!
    /// </summary>
    private float _currentHealth;

    public float CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            if (_currentHealth != value)
            {
                _currentHealth = value;
                onHealthChange?.Invoke();

                UpdateDisplay();

                if (_currentHealth <= 0)
                {
                    HandleDeath();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = _maxHealth;
        InitEvents();
    }

    public void TakeDamage(float damageAmount)
    {
        if (damageAmount > 0)
        {
            onDamage.Invoke();
        }

        CurrentHealth -= damageAmount;
    }

    public void HealHealth(float healAmount)
    {
        if (healAmount > 0)
        {
            onHeal.Invoke();
        }

        if (!IsFullHealth())
            CurrentHealth += healAmount;
    }

    public bool IsFullHealth()
    {
        return _currentHealth == _maxHealth;
    }


    private void InitEvents()
    {
        onDamage ??= new UnityEvent();
        onDeath ??= new UnityEvent();
        onHeal ??= new UnityEvent();
        onHealthChange ??= new UnityEvent();
    }

    private void UpdateDisplay()
    {
        if (!_displayBar)
            return;
        if (hideOnFullHealth && IsFullHealth())
            return;

        switch (healthDisplay)
        {
            case DisplayOption.Hidden:
                DisplayBar = false;
                break;
            case DisplayOption.Bar:
                if (healthBarFill && healthBarSlider)
                {
                    healthBarSlider.maxValue = _maxHealth;
                    healthBarSlider.value = _currentHealth;
                    healthBarFill.color = healthBarColor.Evaluate(healthBarSlider.normalizedValue);
                }

                break;
            case DisplayOption.Hearts:
                if (healthHeartBar && Application.isPlaying)
                    healthHeartBar.DisplayHearts((int)_currentHealth);
                break;
            case DisplayOption.Shader:
                if (shaderHealthBar)
                    shaderHealthBar.HealthNormalized = _currentHealth / _maxHealth;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HandleDeath()
    {
        onDeath?.Invoke();
        switch (deathHandling)
        {
            case DeathResponse.DoNothing:
                break;
            case DeathResponse.DestroyGameObject:
                Destroy(gameObject);
                break;
            case DeathResponse.DisableGameObject:
                gameObject.SetActive(false);
                break;
        }
    }
}