using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManger : MonoBehaviour, IGameStateListener
{
    [Header("Elements")]
    [SerializeField] private Player player;
    private WaveManagerUI ui;

    [Header("Settings")]
    [SerializeField] private float waveDuration;
    private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;

    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    private List<float> localCounters = new List<float>();

    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartWave(currentWaveIndex);
    }

    // Update is called once per frame
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
            StartWaveTransition();
    }

    private void StartWave(int waveIndex)
    {
        ui.UpdateWaveText("Wave" + (currentWaveIndex + 1) + " / " + waves.Length);

        localCounters.Clear();
        foreach(WaveSegment segment in waves[waveIndex].segments)
            localCounters.Add(1);
        
        timer = 0f;
        isTimerOn = true;

        localCounters.Clear();
        for (int i = 0; i < waves[waveIndex].segments.Count; i++)
        {
            localCounters.Add(1);
        }
    }

    private void StartWaveTransition()
    {
        isTimerOn = false;
        DefeatAllEnemies();

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Length)
        {
            ui.UpdateTimerText("");
            ui.UpdateWaveText("Stage Complete!");
        }
        else
            GameManager.instance.WaveCompletedCallback();
        
    }

    private void StartNextWave()
    {
        StartWave(currentWaveIndex);
    }

    private void DefeatAllEnemies()
    {
        transform.Clear();
    }

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
            }

        }

        timer += Time.deltaTime;
    }

    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offset = direction.normalized * Random.Range(6f, 10f);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -18f, 18f);
        targetPosition.x = Mathf.Clamp(targetPosition.x, -8f, 8f);
        
        return targetPosition;
    }

    public void GmaeStateChangeCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                StartNextWave();
                break;

            case GameState.WAVETRANSITION:
                isTimerOn = false;
                ui.UpdateTimerText("");
                ui.UpdateWaveText("Wave Transition");
                break;

            case GameState.SHOP:
                isTimerOn = false;
                ui.UpdateTimerText("");
                ui.UpdateWaveText("Shop");
                break;

            case GameState.GAME:
                StartWave(currentWaveIndex);
                break;

            default:
                break;
        }
    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;
    public float spawnFrequency;
    public GameObject prefab;

}
