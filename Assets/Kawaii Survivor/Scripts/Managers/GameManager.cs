using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Actions")]
    public static Action onGamePaused;
    public static Action onGameResumed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        SetGmaeState(GameState.MENU);
    }

    public void StartGame() => SetGmaeState(GameState.GAME);
    public void StartWeaponSelection() => SetGmaeState(GameState.WEAPONSELECTION);
    public void StartShop() => SetGmaeState(GameState.SHOP);


    public void SetGmaeState(GameState state)
    {
        IEnumerable<IGameStateListener> gameStateListeners =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGameStateListener>();

        foreach (IGameStateListener listener in gameStateListeners)
        {
            listener.GmaeStateChangeCallback(state);
        }

    }

    public void WaveCompletedCallback()
    {
        if (Player.instance.HasLeveledUp() || WaveTransitionManager.instance.HasCollectedChest())
        {
            SetGmaeState(GameState.WAVETRANSITION);
        }
        else
        {
            SetGmaeState(GameState.SHOP);
        }
    }

    public void ManageGameOver()
    {
        // LeanTween.delayedCall(2, () => SceneManager.LoadScene("0"));
        SceneManager.LoadScene(0);
    }

    public void PauseButtonCallback()
    {
        Time.timeScale = 0;
        onGamePaused?.Invoke();
    }

    public void ResumeButtonCallback()
    {
        Time.timeScale = 1;
        onGameResumed?.Invoke();
    }   

    public void RestartFromPause()
    {
        Time.timeScale = 1;
        ManageGameOver();
    }
}

public interface IGameStateListener
{
    void GmaeStateChangeCallback(GameState gameState);
}
