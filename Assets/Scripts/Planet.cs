using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Planet : MonoBehaviour {
    private System.Action<Planet> _eventDamaged;
    private System.Action<Planet> _eventDestroyed;
    private System.Action<Planet> _eventTargetChanged;

    [SerializeField]
    private int _maxHealth = 10;
    private int _curHealth = 0;

    public bool IsAlive { get; private set; }
    public bool IsPlayer { get; private set; }
    public int MaxHealth { get { return _maxHealth; } }
    public int CurHealth { get { return _curHealth; } }

    [Header("Temp Assigner: Assigned by the Game Manager")]
    public Planet _temp_intial_target = null;
    // Is assigned from the SpawnPlanet method in GameManager
    public Planet TargetPlanet { get; private set; }

    public Image Bar;
    public float Fill;

    /// <summary>
    /// Triggers when the planet takes damage
    /// </summary>
    public event System.Action<Planet> EventDamaged {
        add { _eventDamaged += value; }
        remove { _eventDamaged -= value; }
    }

    /// <summary>
    /// Triggers when the planet is destroyed (Health == 0)
    /// </summary>
    public event System.Action<Planet> EventDestroyed {
        add { _eventDestroyed += value; }
        remove { _eventDestroyed -= value; }
    }

    /// <summary>
    /// Triggers when the planet's target is changed
    /// </summary>
    public event System.Action<Planet> EventTargetChanged {
        add { _eventTargetChanged += value; }
        remove { _eventTargetChanged -= value; }
    }

    private void Awake() {
        this.IsAlive = true;
        this._curHealth = this._maxHealth;

        // Temp Code: used to set target without uses of GameManager
        if (_temp_intial_target != null) {
            SetTargetPlanet(_temp_intial_target);
        }
    }

    public void SetPlayerStatus(bool isPlayer, bool force_events = false) {
        if (this.IsPlayer != IsPlayer || force_events) {
            this.IsPlayer = isPlayer;

            if (this.IsPlayer)
                MsgRelay.TriggerPlayerAssigned(this);
            else
                MsgRelay.TriggerPlayerUnassigned(this);
        }
    }

    public void SetTargetPlanet(Planet newTarget) {
        // Can't target self
        if (newTarget == this) return;

        if (this.TargetPlanet != newTarget) {
            this.TargetPlanet = newTarget;

            if (_eventTargetChanged != null)
                _eventTargetChanged.Invoke(newTarget);
        }
    }

    public void Damage(int amount) {
        if (amount <= 0) return;

        _curHealth -= amount;
        _curHealth = Mathf.Max(_curHealth, 0);

        Debug.LogFormat("[Hit = {0}] Damage {1}  Health {2}", this.name, amount, _curHealth);

        if (_eventDamaged != null)
            _eventDamaged.Invoke(this);
        MsgRelay.TriggerPlanetDamaged(this);

        if (_curHealth == 0) {
            IsAlive = false;
            if (_eventDestroyed != null)
                _eventDestroyed.Invoke(this);
            MsgRelay.TriggerPlanetDestroyed(this);
        }
    }

    void Start () {
        Fill = 1f;
    }

    protected virtual void Update () {

        Debug.Log("hi " + _curHealth);

        float healthTranslated = _curHealth * 0.1f;
        Fill = healthTranslated;

        Bar.fillAmount = Fill;
    }

}
