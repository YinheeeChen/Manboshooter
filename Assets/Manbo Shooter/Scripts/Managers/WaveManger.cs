using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Manages timed enemy wave spawning during the gameplay phase.
/// Implements IGameStateListener to respond to game state transitions.
/// </summary>
[RequireComponent(typeof(WaveManagerUI))]
public class WaveManger : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Player player;                 // Reference to the player for spawn positioning
    private WaveManagerUI ui;                               // UI manager for displaying wave info and timer

    [Header("Settings")]
    [SerializeField] private float waveDuration;            // Total duration of each wave in seconds
    private float timer;                                    // Tracks elapsed time within the current wave
    private bool isTimerOn;                                 // Flag to determine if the wave timer is active
    private int currentWaveIndex;                           // Index of the current wave

    [Header("Waves")]
    [SerializeField] private Wave[] waves;                  // List of waves, each containing multiple segments
    private List<float> localCounters = new List<float>();  // Counters for each segment to control spawn frequency

    /// <summary>
    /// Initializes the UI reference.
    /// </summary>
    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }

    void Update()
    {
        if (!isTimerOn) return;

        if (timer < waveDuration)
        {
            ManageCurrentWave();

            string timeString = ((int)(waveDuration - timer)).ToString();
            ui.UpdateTimerText(timeString);
        }
        else
        {
            StartWaveTransition();
        }
    }

    /// <summary>
    /// Begins a specific wave and initializes segment counters and timer.
    /// </summary>
    private void StartWave(int waveIndex)
    {
        ui.UpdateWaveText("Wave " + (currentWaveIndex + 1) + " / " + waves.Length);

        localCounters.Clear();
        foreach (WaveSegment segment in waves[waveIndex].segments)
            localCounters.Add(0f);

        timer = 0f;
        isTimerOn = true;
    }

    /// <summary>
    /// Handles wave end logic and transitions to next wave or stage completion.
    /// </summary>
    private void StartWaveTransition()
    {
        isTimerOn = false;
        DefeatAllEnemies(); // Clear existing enemies without dropping rewards

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            ui.UpdateTimerText("");
            ui.UpdateWaveText("Stage Complete!");
            GameManager.instance.SetGmaeState(GameState.STAGECOMPLETE);
        }
        else
        {
            GameManager.instance.WaveCompletedCallback();
        }
    }

    /// <summary>
    /// Triggers the start of the next wave.
    /// </summary>
    private void StartNextWave()
    {
        StartWave(currentWaveIndex);
    }

    /// <summary>
    /// Forces all active enemies under this manager to die silently (no drops).
    /// </summary>
    private void DefeatAllEnemies()
    {
        foreach (Enemy enemy in transform.GetComponentsInChildren<Enemy>())
            enemy.PassAwayAfterWave();
    }

    /// <summary>
    /// Manages spawn logic for the current wave's segments.
    /// </summary>
    private void ManageCurrentWave()
    {
        Wave currentWave = waves[currentWaveIndex];

        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegment segment = currentWave.segments[i];

            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;

            if (timer < tStart || timer > tEnd)
                continue;

            float timeSinceSegmentStart = timer - tStart;
            float spawnDelay = 1f / segment.spawnFrequency;

            if (timeSinceSegmentStart / spawnDelay >= localCounters[i])
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;

                if (segment.spawnOnce)
                    localCounters[i] = Mathf.Infinity; // Prevent further spawns from this segment
            }
        }

        timer += Time.deltaTime;
    }

    /// <summary>
    /// Returns a valid spawn position near the player, with optional arena clamping.
    /// </summary>
    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = UnityEngine.Random.onUnitSphere;
        Vector2 offset = direction.normalized * UnityEngine.Random.Range(6f, 10f);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        if (!GameManager.instance.UseInfiniteMap)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, -Constants.arenaSize.x / 2, Constants.arenaSize.x / 2);
            targetPosition.y = Mathf.Clamp(targetPosition.y, -Constants.arenaSize.y / 2, Constants.arenaSize.y / 2);
        }

        return targetPosition;
    }

    /// <summary>
    /// Responds to game state transitions. Starts or stops wave timer accordingly.
    /// </summary>
    public void GmaeStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                StartNextWave();
                break;

            case GameState.GAMEOVER:
                isTimerOn = false;
                DefeatAllEnemies();
                break;

            default:
                break;
        }
    }
}

/// <summary>
/// Represents a full wave of enemy segments.
/// </summary>
[System.Serializable]
public struct Wave
{
    public string name;                         // Display name of the wave
    public List<WaveSegment> segments;          // List of spawn segments that make up the wave
}

/// <summary>
/// Defines a segment of enemy spawning logic within a wave.
/// </summary>
[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;  // Start and end times (as % of waveDuration)
    public float spawnFrequency;                      // Enemies spawned per second
    public GameObject prefab;                         // Enemy prefab to spawn
    public bool spawnOnce;                            // Whether to spawn only once during the segment
}
