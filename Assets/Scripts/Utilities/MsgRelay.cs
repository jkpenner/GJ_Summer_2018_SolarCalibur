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
#endregion
}
