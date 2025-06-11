using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all UI panels based on game state transitions and pause/resume events.
/// Implements IGameStateListener to react to GameState changes from the GameManager.
/// </summary>
public class UIMnanager : MonoBehaviour, IGameStateListener
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;                    // Main menu panel
    [SerializeField] private GameObject weaponSelectionPanel;        // Weapon selection panel
    [SerializeField] private GameObject gamePanel;                   // In-game HUD panel
    [SerializeField] private GameObject gameOverPanel;               // Game over screen
    [SerializeField] private GameObject stageCompletePanel;          // Stage complete screen
    [SerializeField] private GameObject waveTransitionPanel;         // Panel shown during wave transitions
    [SerializeField] private GameObject shopPanel;                   // In-game shop UI
    [SerializeField] private GameObject pausePanel;                  // Pause menu panel
    [SerializeField] private GameObject restartConfirmationPanel;    // Restart confirmation popup
    [SerializeField] private GameObject characterSelectionPanel;     // Character selection UI
    [SerializeField] private GameObject settingsPanel;               // Settings UI panel

    private List<GameObject> panels = new List<GameObject>();        // List of all primary game state panels

    /// <summary>
    /// Initializes UI panel list and subscribes to pause/resume events.
    /// </summary>
    private void Awake()
    {
        panels.AddRange(new GameObject[]
        {
            menuPanel,
            weaponSelectionPanel,
            gamePanel,
            gameOverPanel,
            stageCompletePanel,
            waveTransitionPanel,
            shopPanel
        });

        GameManager.onGamePaused += GamePausedCallback;
        GameManager.onGameResumed += GameResumedCallback;

        pausePanel.SetActive(false);
        HideRestartConfirmationPanel();
        HideCharacterSelectionPanel();
        HideSettingsPanel();
    }

    /// <summary>
    /// Unsubscribes from game pause/resume events.
    /// </summary>
    private void OnDestroy()
    {
        GameManager.onGamePaused -= GamePausedCallback;
        GameManager.onGameResumed -= GameResumedCallback;
    }

    /// <summary>
    /// Responds to GameState changes and activates the appropriate UI panel.
    /// </summary>
    /// <param name="gameState">The current game state.</param>
    public void GmaeStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                ShowPanel(menuPanel);
                break;
            case GameState.WEAPONSELECTION:
                ShowPanel(weaponSelectionPanel);
                break;
            case GameState.GAME:
                ShowPanel(gamePanel);
                break;
            case GameState.GAMEOVER:
                ShowPanel(gameOverPanel);
                break;
            case GameState.STAGECOMPLETE:
                ShowPanel(stageCompletePanel);
                break;
            case GameState.WAVETRANSITION:
                ShowPanel(waveTransitionPanel);
                break;
            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;
        }
    }

    /// <summary>
    /// Activates the specified panel and deactivates all others in the main panel list.
    /// </summary>
    /// <param name="panel">The panel to show.</param>
    private void ShowPanel(GameObject panel)
    {
        foreach (GameObject p in panels)
            p.SetActive(p == panel);
    }

    /// <summary>
    /// Shows the pause panel when the game is paused.
    /// </summary>
    private void GamePausedCallback()
    {
        pausePanel.SetActive(true);
    }

    /// <summary>
    /// Hides the pause panel when the game resumes.
    /// </summary>
    private void GameResumedCallback()
    {
        pausePanel.SetActive(false);
    }

    /// <summary>
    /// Shows the restart confirmation popup.
    /// </summary>
    public void ShowRestartConfirmationPanel()
    {
        restartConfirmationPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the restart confirmation popup.
    /// </summary>
    public void HideRestartConfirmationPanel()
    {
        restartConfirmationPanel.SetActive(false);
    }

    /// <summary>
    /// Shows the character selection panel.
    /// </summary>
    public void ShowCharacterSelectionPanel()
    {
        characterSelectionPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the character selection panel.
    /// </summary>
    public void HideCharacterSelectionPanel()
    {
        characterSelectionPanel.SetActive(false);
    }

    /// <summary>
    /// Shows the settings panel.
    /// </summary>
    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the settings panel.
    /// </summary>
    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
}
