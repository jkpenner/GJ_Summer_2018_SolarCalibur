#define SHOW_DEBUG_LOGS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgRelay : Singleton<MsgRelay> {
    private void Awake() {
        if (this.IsActiveInstance == false) {
            Debug.LogFormat("[{0}]: Another instance of MsgRelay is already in the scene. " +
                "Destroying additional copy.", this.name);
            Destroy(this.gameObject);
        }
    }

    #region General Game Events
    private event System.Action _eventGameStart;
    private event System.Action _eventGamePause;
    private event System.Action _eventGameResume;
    private event System.Action _eventGameComplete;

    /// <summary>
    /// Triggers when the game logic should start
    /// </summary>
    static public event System.Action EventGameStart {
        add { Instance._eventGameStart += value; }
        remove { Instance._eventGameStart -= value; }
    }

    /// <summary>
    /// Triggers when the game logic should pause
    /// </summary>
    static public event System.Action EventGamePause {
        add { Instance._eventGamePause += value; }
        remove { Instance._eventGamePause -= value; }
    }

    /// <summary>
    /// Triggers when the game logic should resume
    /// </summary>
    static public event System.Action EventGameResume {
        add { Instance._eventGameResume += value; }
        remove { Instance._eventGameResume -= value; }
    }

    /// <summary>
    /// Triggers when the game is complete. End of a fight
    /// </summary>
    static public event System.Action EventGameComplete {
        add { Instance._eventGameComplete += value; }
        remove { Instance._eventGameComplete -= value; }
    }

    static public void TriggerGameStart() {
#if SHOW_DEBUG_LOGS
        Debug.Log("Game Start Triggered");
#endif

        if (Exists && Instance._eventGameStart != null) {
            Instance._eventGameStart.Invoke();
        }
    }

    static public void TriggerGamePause() {
#if SHOW_DEBUG_LOGS
        Debug.Log("Game Pause Triggered");
#endif

        if (Exists && Instance._eventGamePause != null) {
            Instance._eventGamePause.Invoke();
        }
    }

    static public void TriggerGameResume() {
#if SHOW_DEBUG_LOGS
        Debug.Log("Game Resume Triggered");
#endif

        if (Exists && Instance._eventGameResume != null) {
            Instance._eventGameResume.Invoke();
        }
    }

    static public void TriggerGameComplete() {
#if SHOW_DEBUG_LOGS
        Debug.Log("Game Complete Triggered");
#endif

        if (Exists && Instance._eventGameComplete != null) {
            Instance._eventGameComplete.Invoke();
        }
    }
    #endregion

    #region Planet Events
    private System.Action<Planet> _eventPlanetSpawned;
    private System.Action<Planet> _eventPlanetDamaged;
    private System.Action<Planet> _eventPlanetDestroyed;
    private System.Action<Planet> _eventPlayerAssigned;
    private System.Action<Planet> _eventPlayerUnassigned;

    /// <summary>
    /// Triggers when the game is complete. End of a fight
    /// </summary>
    static public event System.Action<Planet> EventPlanetSpawned {
        add { Instance._eventPlanetSpawned += value; }
        remove { Instance._eventPlanetSpawned -= value; }
    }

    /// <summary>
    /// Triggers when the game is complete. End of a fight
    /// </summary>
    static public event System.Action<Planet> EventPlanetDamaged {
        add { Instance._eventPlanetDamaged += value; }
        remove { Instance._eventPlanetDamaged -= value; }
    }

    /// <summary>
    /// Triggers when the game is complete. End of a fight
    /// </summary>
    static public event System.Action<Planet> EventPlanetDestroyed {
        add { Instance._eventPlanetDestroyed += value; }
        remove { Instance._eventPlanetDestroyed -= value; }
    }

    /// <summary>
    /// Triggers when a planet's isplayer property is set to true
    /// </summary>
    static public event System.Action<Planet> EventPlayerAssigned {
        add { Instance._eventPlayerAssigned += value; }
        remove { Instance._eventPlayerAssigned -= value; }
    }

    /// <summary>
    /// Triggers when a planet's isplayer property is set to false from true
    /// </summary>
    static public event System.Action<Planet> EventPlayerUnassigned {
        add { Instance._eventPlayerUnassigned += value; }
        remove { Instance._eventPlayerUnassigned -= value; }
    }

    static public void TriggerPlanetSpawned(Planet planet) {
#if SHOW_DEBUG_LOGS
        Debug.LogFormat("[{0}]: Planet Spawned", planet.name);
#endif

        if (Exists && Instance._eventPlanetSpawned != null) {
            Instance._eventPlanetSpawned.Invoke(planet);
        }
    }

    static public void TriggerPlanetDamaged(Planet planet) {
#if SHOW_DEBUG_LOGS
        Debug.LogFormat("[{0}]: Planet Damaged", planet.name);
#endif

        if (Exists && Instance._eventPlanetDamaged != null) {
            Instance._eventPlanetDamaged.Invoke(planet);
        }
    }

    static public void TriggerPlanetDestroyed(Planet planet) {
#if SHOW_DEBUG_LOGS
        Debug.LogFormat("[{0}]: Planet Destroyed", planet.name);
#endif

        if (Exists && Instance._eventPlanetDestroyed != null) {
            Instance._eventPlanetDestroyed.Invoke(planet);
        }
    }

    static public void TriggerPlayerAssigned(Planet planet) {
#if SHOW_DEBUG_LOGS
        Debug.LogFormat("[{0}]: Assigned as player", planet.name);
#endif

        if (Exists && Instance._eventPlayerAssigned != null) {
            Instance._eventPlayerAssigned.Invoke(planet);
        }
    }

    static public void TriggerPlayerUnassigned(Planet planet) {
#if SHOW_DEBUG_LOGS
        Debug.LogFormat("[{0}]: Unassigned as player", planet.name);
#endif

        if (Exists && Instance._eventPlayerUnassigned != null) {
            Instance._eventPlayerUnassigned.Invoke(planet);
        }
    }
    #endregion
}
