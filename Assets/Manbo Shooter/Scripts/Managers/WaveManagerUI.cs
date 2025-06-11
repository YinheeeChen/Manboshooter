using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManagerUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timerText;

    public void UpdateWaveText(string waveString) => waveText.text = waveString;
    public void UpdateTimerText(string timeString) => timerText.text = timeString;

    
    
}
