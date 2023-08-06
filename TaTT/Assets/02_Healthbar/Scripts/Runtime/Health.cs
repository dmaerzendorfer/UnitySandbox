using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

//todo: health script with:
//- dmg method
//- heal method
//- event for on health change
//- event for on dmg
//- event for on heal
//- event for death
//- a set invincible for seconds method
//- display mode ->bar, circle, hearts
//- a custom editor for setting the display mode? depends on if needed
// -> in the editor a button for testing the dmg and heal methods
//- anything else that comes to mind


public class Health : MonoBehaviour
{
    [Header("Health Settings")] [Description("The maximum Health.")]
    public float maxHealth = 5f;

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
                //todo: handle enabling/disabling of the ui.
            }
        }
    }

    public enum DeathResponse
    {
        DoNothing,
        DisableGameObject,
        DestroyGameObject
    }

    //todo: make this into a foldout -> customEditor?! -> other soultion would be to make my own attribute -> too much work, or use a package that does it for me eG Odin
    //see: https://docs.unity3d.com/ScriptReference/EditorGUILayout.Foldout.html
    //fuck it, make everything in myself in the custom editor
    [Header("Death Settings")] [Description("What happens per on death, the death event is always fired.")]
    public DeathResponse deathHandling = DeathResponse.DisableGameObject;

    [Header("Event Settings")] [Description("Fired when health reaches <=0.")]
    public UnityEvent onDeath;

    [Description("Fired whenever someone calls the takeDamage function with a value >0")]
    public UnityEvent onDamage;

    [Description("Fired whenever someone calls the healDamange function with a value >0")]
    public UnityEvent onHeal;

    [Description("Fired whenever health changes.")]
    public UnityEvent onHealthChange;

    public enum DisplayOption
    {
        Hidden,
        Bar,
        Circle,
        Hearts
    }

    //todo: show that in the custom editor under the display options header (and implement the display^^')
    private DisplayOption _healthDisplay = DisplayOption.Bar;


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
                //todo: check if it is invoked on start since we set currentHealth to maxHealth.
                onHealthChange?.Invoke();

                //todo: update the display

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
        CurrentHealth = maxHealth;
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

        CurrentHealth += healAmount;
    }


    private void InitEvents()
    {
        onDamage ??= new UnityEvent();
        onDeath ??= new UnityEvent();
        onHeal ??= new UnityEvent();
        onHealthChange ??= new UnityEvent();
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