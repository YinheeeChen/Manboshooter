using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        if (Player.instance.HasLeveledUp())
        {
            SetGmaeState(GameState.SHOP);
        }
        else
        {
            SetGmaeState(GameState.WAVETRANSITION);
        }
    }

    public void ManageGameOver()
    {
        // LeanTween.delayedCall(2, () => SceneManager.LoadScene("0"));
        SceneManager.LoadScene(0);
    }
}

public interface IGameStateListener
{
    void GmaeStateChangeCallback(GameState gameState);
}
