using System;
using System.Collections;
using System.Collections.Generic;
using Tabsil.Sijil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the game's settings UI, including SFX and music toggles, and credits panel.
/// Implements IWantToBeSaved to persist toggle states using the Sijil save system.
/// </summary>
public class SettingsManager : MonoBehaviour, IWantToBeSaved
{
    [Header("Elements")]
    [SerializeField] private Button sfxButton;                // Button to toggle SFX on/off
    [SerializeField] private Button musicButton;              // Button to toggle music on/off
    [SerializeField] private Button creditsButton;            // Button to open credits panel
    [SerializeField] private GameObject creditsPanel;         // UI panel showing credits

    [Header("Settings Panel")]
    [SerializeField] private Color onColor;                   // Color to indicate toggle ON
    [SerializeField] private Color offColor;                  // Color to indicate toggle OFF

    [Header("Data")]
    private bool sfxState;                                    // Current SFX state
    private bool musicState;                                  // Current music state

    [Header("Actions")]
    public static Action<bool> onSFXStateChanged;             // Event invoked when SFX state changes
    public static Action<bool> onMusicStateChanged;           // Event invoked when music state changes

    /// <summary>
    /// Sets up button listeners.
    /// </summary>
    private void Awake()
    {
        sfxButton.onClick.RemoveAllListeners();
        sfxButton.onClick.AddListener(SFXButtonCallback);

        musicButton.onClick.RemoveAllListeners();
        musicButton.onClick.AddListener(MusicButtonCallback);

        creditsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.AddListener(CreditsButtonCallback);
    }

    /// <summary>
    /// Initializes settings and closes credits panel on start.
    /// Broadcasts current audio states.
    /// </summary>
    void Start()
    {
        CloseCreditsPanel();

        onSFXStateChanged?.Invoke(sfxState);
        onMusicStateChanged?.Invoke(musicState);
    }

    void Update()
    {
        // No per-frame logic
    }

    /// <summary>
    /// Toggles SFX state, updates visuals, saves, and invokes change event.
    /// </summary>
    private void SFXButtonCallback()
    {
        sfxState = !sfxState;
        UpdateSFXVisuals();
        Save();
        onSFXStateChanged?.Invoke(sfxState);
    }

    /// <summary>
    /// Toggles music state, updates visuals, saves, and invokes change event.
    /// </summary>
    private void MusicButtonCallback()
    {
        musicState = !musicState;
        UpdateMusicVisuals();
        Save();
        onMusicStateChanged?.Invoke(musicState);
    }

    /// <summary>
    /// Opens the credits panel.
    /// </summary>
    private void CreditsButtonCallback()
    {
        creditsPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the credits panel.
    /// </summary>
    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }

    /// <summary>
    /// Updates SFX button visuals based on current state.
    /// </summary>
    private void UpdateSFXVisuals()
    {
        if (sfxState)
        {
            sfxButton.image.color = onColor;
            sfxButton.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }
        else
        {
            sfxButton.image.color = offColor;
            sfxButton.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
        }
    }

    /// <summary>
    /// Updates music button visuals based on current state.
    /// </summary>
    private void UpdateMusicVisuals()
    {
        if (musicState)
        {
            musicButton.image.color = onColor;
            musicButton.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }
        else
        {
            musicButton.image.color = offColor;
            musicButton.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
        }
    }

    /// <summary>
    /// Loads saved settings using Sijil. Defaults to true if no saved values exist.
    /// </summary>
    public void Load()
    {
        sfxState = true;
        musicState = true;

        if (Sijil.TryLoad(this, "sfx", out object sfxStateObject))
        {
            sfxState = (bool)sfxStateObject;
        }

        if (Sijil.TryLoad(this, "music", out object musicStateObject))
        {
            musicState = (bool)musicStateObject;
        }

        UpdateSFXVisuals();
        UpdateMusicVisuals();
    }

    /// <summary>
    /// Saves the current SFX and music states using Sijil.
    /// </summary>
    public void Save()
    {
        Sijil.Save(this, "sfx", sfxState);
        Sijil.Save(this, "music", musicState);
    }
}
